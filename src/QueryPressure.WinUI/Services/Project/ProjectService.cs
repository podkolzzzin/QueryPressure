using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.Services.Project;

public class ProjectService : IProjectService
{
  private readonly ISubject<ProjectModel?> _subject;

  public ProjectService(ISubject<ProjectModel?> subject)
  {
    _subject = subject;
    Project = null;
  }

  public Task OpenProjectAsync(string parameter, CancellationToken token)
  {
    throw new NotImplementedException();
  }

  public void CreateNew()
  {
    Project = new ProjectModel
    {
      Id = Guid.NewGuid(),
      Name = "New Project"
    };

    _subject.Notify(Project);
  }

  public ProjectModel? Project { get; private set; }
}
