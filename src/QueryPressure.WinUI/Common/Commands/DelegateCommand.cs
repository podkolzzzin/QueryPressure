using System.Windows.Input;

namespace QueryPressure.WinUI.Common.Commands;

public class DelegateCommand : ICommand
{
  private readonly Action _execute;
  private readonly Predicate<object?>? _canExecute;

  public DelegateCommand(Action execute, Predicate<object?>? canExecute = null)
  {
    _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    _canExecute = canExecute;
  }

  public bool CanExecute(object? parameter)
  {
    return _canExecute is null || _canExecute(parameter);
  }

  public void Execute(object? parameter)
  {
    if (CanExecute(parameter))
    {
      _execute();
    }
  }

  public event EventHandler? CanExecuteChanged
  {
    add => CommandManager.RequerySuggested += value;
    remove => CommandManager.RequerySuggested -= value;
  }
}

public class DelegateCommand<TParameter> : ICommand
{
  private readonly Action<TParameter> _execute;
  private readonly Predicate<TParameter>? _canExecute;

  public DelegateCommand(Action<TParameter> execute, Predicate<TParameter>? canExecute = null)
  {
    _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    _canExecute = canExecute;
  }

  public bool CanExecute(object? parameter)
  {
    if (_canExecute is null)
    {
      return true;
    }

    if (parameter == null && typeof(TParameter).IsValueType)
    {
      var obj = default(TParameter);
      return obj is not null && _canExecute(obj);
    }

    if (parameter is TParameter param)
    {
      return _canExecute(param);
    }

    return false;
  }

  public void Execute(object? parameter)
  {
    if (CanExecute(parameter))
    {
      _execute((TParameter)parameter!);
    }
  }

  public event EventHandler? CanExecuteChanged
  {
    add => CommandManager.RequerySuggested += value;
    remove => CommandManager.RequerySuggested -= value;
  }
}
