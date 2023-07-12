using Microsoft.Extensions.Logging;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Execute;

namespace QueryPressure.WinUI.Commands.Scenario;

public class ExecuteScenarioCommand : CommandBase<ScenarioModel>
{
  private readonly IExecutionService _exucutionService;

  public ExecuteScenarioCommand(ILogger<ExecuteScenarioCommand> logger, IExecutionService exucutionService) : base(logger)
  {
    _exucutionService = exucutionService;
  }

  protected override void ExecuteInternal(ScenarioModel parameter)
  {
    _exucutionService.Execute(parameter);
  }
}
