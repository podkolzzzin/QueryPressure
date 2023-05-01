using Microsoft.Extensions.Logging;

namespace QueryPressure.WinUI.Common.Commands;

public abstract class AsyncCommandBase<TParameter> : CommandBase<TParameter>
{
  protected AsyncCommandBase(ILogger logger) : base(logger)
  {
  }

  protected sealed override void ExecuteInternal(TParameter parameter)
  {
    Task.Run(async () => await ExecuteAsync(parameter, CancellationToken.None))
      .Wait(TimeSpan.FromSeconds(30), CancellationToken.None);
  }

  protected abstract Task ExecuteAsync(TParameter parameter, CancellationToken token);
}

