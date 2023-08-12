using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.Services.Execute;

public interface IExecutionService
{
  Guid Execute(ScenarioModel parameter);
}
