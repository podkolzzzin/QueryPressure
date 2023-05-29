using System.Windows.Threading;

namespace QueryPressure.WinUI.Services;

public interface IDispatcherService
{

  Task<TResult> InvokeAsync<TResult>(Func<TResult> function);
  Task InvokeAsync(Action action);
}

public class DispatcherService : IDispatcherService
{
  private readonly Dispatcher _dispatcher;

  public DispatcherService()
  {
    _dispatcher = Dispatcher.CurrentDispatcher;
  }

  public Task InvokeAsync(Action action)
  {
    if (!_dispatcher.CheckAccess())
    {
      return _dispatcher.InvokeAsync(action).Task;
    }
      
    action();
    return Task.CompletedTask;
  }

  public Task<TResult> InvokeAsync<TResult>(Func<TResult> function)
  {
    return _dispatcher.CheckAccess() ? Task.FromResult(function()) : _dispatcher.InvokeAsync(function).Task;
  }
}


