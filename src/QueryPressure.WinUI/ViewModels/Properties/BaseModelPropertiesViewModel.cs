using QueryPressure.WinUI.Commands.App;
using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Models;
using System.Runtime.CompilerServices;

namespace QueryPressure.WinUI.ViewModels.Properties;

public delegate TModel UpdateModel<in T, TModel>(TModel model, T value);

public abstract class BaseModelPropertiesViewModel<TModel> : ViewModelBase where TModel : IModel
{
  private readonly EditModelCommand _editModelCommand;
  private readonly TModel _model;

  protected BaseModelPropertiesViewModel(EditModelCommand editModelCommand, TModel model)
  {
    _editModelCommand = editModelCommand;
    _model = model;
  }

  protected bool SetModelField<T>(ref T field, T value, UpdateModel<T, TModel> getUpdatedModel, [CallerMemberName] string? propertyName = null)
  {
    if (EqualityComparer<T>.Default.Equals(field, value)) return false;
    field = value;
    OnPropertyChanged(propertyName);
    _editModelCommand.DeBounce(new EditModelCommandParameter(this, getUpdatedModel.Invoke(_model, value)));
    return true;
  }
}
