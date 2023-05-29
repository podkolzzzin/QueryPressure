using Microsoft.Extensions.Logging;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Services.Project;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.Commands.Project;

public class RenameProjectCommand : CommandBase<string>
{
  private readonly IProjectService _projectService;
  private readonly ISubscriptionManager _subscriptionManager;


  public RenameProjectCommand(ILogger<RenameProjectCommand> logger, IProjectService projectService, ISubscriptionManager subscriptionManager) : base(logger)
  {
    _projectService = projectService;
    _subscriptionManager = subscriptionManager;
  }

  protected override void ExecuteInternal(string parameter)
  {
    var currentProject = _projectService.Project;

    if (currentProject == null)
    {
      return;
    }

    currentProject.Name = $"New Name ({DateTime.Now:G}) + {parameter}";
    _subscriptionManager.Notify(this, ModelAction.Edit, currentProject);
  }
}
