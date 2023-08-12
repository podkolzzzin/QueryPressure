using System.IO;
using Microsoft.Extensions.Logging;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Services.Project;

namespace QueryPressure.WinUI.Commands.Project;

public class OpenProjectCommand : CommandBase<string>
{
  private readonly IProjectService _projectService;
  private readonly CloseProjectCommand _closeProjectCommand;

  public OpenProjectCommand(ILogger<OpenProjectCommand> logger, IProjectService projectService, CloseProjectCommand closeProjectCommand) : base(logger)
  {
    _projectService = projectService;
    _closeProjectCommand = closeProjectCommand;
  }

  protected override bool CanExecuteInternal(string? parameter)
  {
    var isCorrectExtension = Path.GetExtension(parameter)?
      .Equals(Constants.ProjectExtension, StringComparison.OrdinalIgnoreCase) ?? false;

    return isCorrectExtension && File.Exists(parameter);
  }

  protected override void ExecuteInternal(string parameter)
  {
    if (_projectService.Project is not null)
    {
      _closeProjectCommand.Execute(null);
    }

    if (_projectService.Project is not null)  // TBD TODO check if _closeProjectCommand closed the project
    {
      throw new InvalidOperationException("Failed to close project");
    }

    _projectService.OpenAsync(parameter);
  }
}
