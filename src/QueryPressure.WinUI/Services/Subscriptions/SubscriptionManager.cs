using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.Services.Subscriptions;

public class SubscriptionManager : ISubscriptionManager, IDisposable
{
  private readonly Dictionary<string, Subject<IModel>> _subjects;

  public SubscriptionManager()
  {
    _subjects = new Dictionary<string, Subject<IModel>>();
  }

  public IObservableItem<IModel> On(ModelAction action, IModel model)
  {
    var key = GetKey(action, model);

    if (_subjects.ContainsKey(key))
    {
      throw new InvalidOperationException($"The subscription key '{key}' already exist");
    }

    var subject = new Subject<IModel>();
    _subjects.Add(key, subject);

    return new ObservableModel(this, subject);
  }

  private static string GetKey(ModelAction action, IModel model)
  {
    return $"{model.GetType().Name} - [{action.ToString().ToUpperInvariant()}] - {model.Id}";
  }

  public void Notify(ModelAction action, IModel model)
  {
    var key = GetKey(action, model);

    if (!_subjects.ContainsKey(key))
    {
      throw new InvalidOperationException($"The subscription key '{key}' does not exist");
    }

    _subjects[key].Notify(model);
  }

  public void Remove(string key)
  {
    if (!_subjects.ContainsKey(key))
    {
      throw new InvalidOperationException($"The subscription key '{key}' does not exist");
    }
    _subjects.Remove(key);
  }

  public void Dispose()
  {
    foreach (var value in _subjects.Values)
    {
      value.Dispose();
    }
  }
}
