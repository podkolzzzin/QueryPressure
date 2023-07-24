namespace QueryPressure.UI.Extensions;

internal static class ProviderManagerExtensions
{
  public static void CancelExecution(this ProviderManager manager, Guid executionId)
    => manager.GetProvider(executionId)?.CancelExecution(executionId);
}
