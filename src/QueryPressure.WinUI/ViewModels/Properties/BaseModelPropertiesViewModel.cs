using QueryPressure.WinUI.Commands.App;
using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Models;
using System.Linq.Expressions;
using System.Reflection;
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

  protected bool SetModelField<T>(ref T field, T value, Expression<Func<TModel, T>> modelPropertySelector, [CallerMemberName] string? propertyName = null)
  {
    return SetModelField(ref field, value, (m, v) => SetModelProperty(m, modelPropertySelector, v), propertyName);
  }

  protected bool SetModelField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
  {
    return SetModelField(ref field, value, (m, v) => SetModelProperty(m, propertyName, v), propertyName);
  }

  private TModel SetModelProperty<T>(TModel model, Expression<Func<TModel, T>> modelPropertySelector, T? value)
  {
    var memberExpression = (MemberExpression)modelPropertySelector.Body;
    var propertyInfo = (PropertyInfo)memberExpression.Member;

    propertyInfo.SetValue(value, value);
    return model;
  }

  private TModel SetModelProperty<T>(TModel model, string? propertyName, T? value)
  {
    if (string.IsNullOrEmpty(propertyName))
    {
      throw new ArgumentNullException(nameof(propertyName));
    }

    var type = model.GetType();
    var property = type.GetProperty(propertyName);

    if (property != null && property.CanWrite)
    {
      property.SetValue(model, value);
    }

    return model;
  }
}
