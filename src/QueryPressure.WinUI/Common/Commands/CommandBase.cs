using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.Logging;

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
    protected readonly ILogger Logger;

    protected CommandBase(ILogger logger)
    {
      Logger = logger;
    }

    public sealed override bool CanExecute(object? parameter)
    {
      try
      {
        return parameter?.GetType() == typeof(TParameter) && CanExecuteInternal((TParameter)parameter);
      }
      catch (Exception exception)
      {
        Logger.LogWarning(exception, "Failed to execute a 'CanExecute' of a command");
        return false;
      }
    }

    public sealed override void Execute(object? parameter)
    {
      if (parameter == null)
      {
        throw new NullReferenceException(nameof(parameter));
      }

      try
      {
        ExecuteInternal((TParameter)parameter);
      }
      catch (Exception exception)
      {
        MessageBox.Show($"Failed to execute command. {exception.Message}", "Unhandled Error", MessageBoxButton.OK, MessageBoxImage.Error);
        Logger.LogError(exception, "Failed to execute command");
      }
    }

    protected virtual bool CanExecuteInternal(TParameter? parameter)
    {
      return parameter != null;
    }
    protected abstract void ExecuteInternal(TParameter parameter);
  }
}
