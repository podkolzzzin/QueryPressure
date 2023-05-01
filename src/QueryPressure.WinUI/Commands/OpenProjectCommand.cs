using System.IO;
using Microsoft.Extensions.Logging;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Services.Project;

namespace QueryPressure.WinUI.Commands;

public class OpenProjectCommand : AsyncCommandBase<string>
{
  private readonly IProjectService _projectService;

  public OpenProjectCommand(ILogger<OpenProjectCommand> logger, IProjectService projectService) : base(logger)
  {
    _projectService = projectService;
  }

  protected override bool CanExecuteInternal(string? parameter)
  {
    var isCorrectExtension = Path.GetExtension(parameter)?
      .Equals(Constants.ProjectExtension, StringComparison.OrdinalIgnoreCase) ?? false;

    return isCorrectExtension && File.Exists(parameter);
  }

  protected override async Task ExecuteAsync(string parameter, CancellationToken token)
  {
    await _projectService.OpenProjectAsync(parameter, token);
  }
}
