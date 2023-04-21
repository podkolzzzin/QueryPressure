using System.Windows.Input;

namespace QueryPressure.WinUI.Common.Commands;

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

    switch (parameter)
    {
      case null when typeof(TParameter).IsValueType:
      {
        var obj = default(TParameter);
        return obj is not null && _canExecute(obj);
      }
      case TParameter param:
        return _canExecute(param);
      default:
        return false;
    }
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
