using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Services.Project;

namespace QueryPressure.WinUI.Commands.Project
{
  public class AddScenarioCommand : CommandBase
  {
    private readonly IProjectService _projectService;

    public AddScenarioCommand(IProjectService projectService)
    {
      _projectService = projectService;
    }

    public override bool CanExecute(object? parameter) => _projectService.Project != null;

    protected override void ExecuteInternal(object? parameter)
    {
      _projectService.AddNewScenario();
    }
  }
}
