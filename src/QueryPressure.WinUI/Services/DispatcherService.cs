using System.Windows.Threading;

namespace QueryPressure.WinUI.Services;

public interface IDispatcherService
{
  void Invoke(Action action);
  TResult Invoke<TResult>(Func<TResult> function);

  Task<TResult> InvokeAsync<TResult>(Func<TResult> function, CancellationToken token);
  Task InvokeAsync(Action action, CancellationToken token);
}

public class DispatcherService : IDispatcherService
{
  private readonly Dispatcher _dispatcher;

  public DispatcherService()
  {
    _dispatcher = Dispatcher.CurrentDispatcher;
  }

  public void Invoke(Action action)
  {
    if (!_dispatcher.CheckAccess())
    {
      _dispatcher.Invoke(action, DispatcherPriority.Background);
      return;
    }

    action();
  }

  public TResult Invoke<TResult>(Func<TResult> function)
  {
    if (!_dispatcher.CheckAccess())
    {
      return _dispatcher.Invoke(function, DispatcherPriority.Background);
    }

    return function();
  }

  public Task InvokeAsync(Action action, CancellationToken token)
  {
    if (!_dispatcher.CheckAccess())
    {
      return _dispatcher.InvokeAsync(action, DispatcherPriority.Background, token).Task;
    }

    action();
    return Task.CompletedTask;
  }

  public Task<TResult> InvokeAsync<TResult>(Func<TResult> function, CancellationToken token)
  {
    return _dispatcher.CheckAccess()
      ? Task.FromResult(function())
      : _dispatcher.InvokeAsync(function, DispatcherPriority.Background, token).Task;
  }
}


