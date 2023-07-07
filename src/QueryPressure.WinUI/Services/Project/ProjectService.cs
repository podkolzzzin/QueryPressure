using System.IO;
using System.IO.Compression;
using System.Text.Json;
using QueryPressure.App.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.Services.Project;

public class ProjectService : IProjectService
{
  private readonly ISubject<ProjectModel?> _subject;
  private readonly ISubscriptionManager _subscriptionManager;
  private readonly ILanguageService _languageService;

  public ProjectService(ISubject<ProjectModel?> subject, ISubscriptionManager subscriptionManager, ILanguageService languageService)
  {
    _subject = subject;
    _subscriptionManager = subscriptionManager;
    _languageService = languageService;

    Project = null;
  }

  public ProjectModel? Project { get; private set; }

  public async Task OpenAsync(string path, CancellationToken token)
  {
    if (Project is not null)
    {
      throw new ArgumentException(nameof(Project));
    }

    if (string.IsNullOrEmpty(path))
    {
      throw new ArgumentNullException(nameof(path));
    }

    var fileInfo = new FileInfo(path);

    if (!fileInfo.Exists)
    {
      throw new FileNotFoundException();
    }

    await using var compressedFileStream = fileInfo.OpenRead();
    await using var compressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
    Project = await JsonSerializer.DeserializeAsync<ProjectModel>(compressor, cancellationToken: token);

    if (Project is null)
    {
      throw new ArgumentNullException(nameof(Project));
    }

    Project.Path = fileInfo;

    _subject.Notify(this, Project);
  }

  public async Task SaveAsync(string path, CancellationToken token)
  {
    if (Project is null)
    {
      throw new ArgumentNullException(nameof(Project));
    }

    if (string.IsNullOrEmpty(path))
    {
      throw new ArgumentNullException(nameof(path));
    }

    var fileInfo = new FileInfo(path);

    await using var compressedFileStream = fileInfo.Create();
    await using var compressor = new GZipStream(compressedFileStream, CompressionMode.Compress);
    await JsonSerializer.SerializeAsync(compressor, Project, cancellationToken: token);

    Project.Path = fileInfo;
    _subscriptionManager.Notify(this, ModelAction.Edit, Project);
  }

  public async Task SaveAsync(CancellationToken token)
  {
    await SaveAsync(Project?.Path?.Name ?? string.Empty, token);
  }

  public void CreateNew()
  {
    var strings = _languageService.GetStrings();
    Project = new ProjectModel
    {
      Id = Guid.NewGuid(),
      Name = strings["labels.project.new-name"]
    };

    var newScenarioNamePrefix = strings["labels.scenario.new-name"];

    var scenario1 = CreateDefaultScenario($"{newScenarioNamePrefix} 1");

    Project.Scenarios.Add(scenario1);

    _subject.Notify(this, Project);
  }

  private static ScenarioModel CreateDefaultScenario(string scenarioName) => new ScenarioModel
  {
    Id = Guid.NewGuid(),
    Name = scenarioName,
    ConnectionString = string.Empty,
    Provider = "Postgres",
    Profile = new FlatArgumentsSection
    {
      Type = "sequential",
      Arguments = new List<ArgumentFlat>()
    },
    Limit = new FlatArgumentsSection
    {
      Type = "queryCount",
      Arguments = new List<ArgumentFlat>()
        {
          new ArgumentFlat()
          {
            Name = "limit",
            Value = "1",
            Type = "int"
          }
        }
    }
  };

  public void Close()
  {
    Project = null;
    _subject.Notify(this, Project);
  }
}
