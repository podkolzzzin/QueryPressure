using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QueryPressure.WinUI.Common;
public abstract class ViewModelBase : INotifyPropertyChanged
{
  public event PropertyChangedEventHandler? PropertyChanged;

  protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  protected void OnOtherPropertyChanged(string propertyName)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
  {
    if (EqualityComparer<T>.Default.Equals(field, value)) return false;
    field = value;
    OnPropertyChanged(propertyName);
    return true;
  }
}
