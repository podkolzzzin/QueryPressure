using System.Windows.Input;

namespace QueryPressure.WinUI.Common.Commands
{
  public abstract class CommandBase : ICommand
  {
    public abstract bool CanExecute(object? parameter);

    public abstract void Execute(object? parameter);

    public event EventHandler? CanExecuteChanged
    {
      add => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }
  }

  public abstract class CommandBase<TParameter> : CommandBase
  {
    public sealed override bool CanExecute(object? parameter)
    {
      return parameter?.GetType() == typeof(TParameter) && CanExecuteInternal((TParameter)parameter);
    }

    public sealed override void Execute(object? parameter)
    {
      if (parameter == null)
      {
        throw new NullReferenceException(nameof(parameter));
      }

      ExecuteInternal((TParameter)parameter);
    }

    protected virtual bool CanExecuteInternal(TParameter? parameter)
    {
      return parameter != null;
    }
    protected abstract void ExecuteInternal(TParameter parameter);
  }
}
