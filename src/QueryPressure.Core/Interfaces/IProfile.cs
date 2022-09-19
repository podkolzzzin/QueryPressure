namespace QueryPressure.Core.Interfaces;

public interface IProfile
{
    Task<IExecutionDescriptor> WhenNextCanBeExecutedAsync(CancellationToken cancellationToken);
    Task OnQueryExecutedAsync(IExecutionDescriptor descriptor, CancellationToken cancellationToken);
}

public interface IExecutionDescriptor {}

public static class ExecutionDescriptor
{
    private class Stub : IExecutionDescriptor { }
    
    public static IExecutionDescriptor Empty = new Stub();
}