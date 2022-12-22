using QueryPressure.App.Arguments;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.App.Interfaces;

public interface IScenarioBuilder
{
  Task<QueryExecutor> BuildAsync(
    ApplicationArguments arguments,
    IExecutionResultStore store, 
    IEnumerable<IExecutionHook> otherHooks,
    CancellationToken cancellationToken);
}
