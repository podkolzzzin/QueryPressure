using System.Runtime.CompilerServices;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.Services.Subscriptions;

public class SubscriptionManager : ISubscriptionManager, IDisposable
{
  private readonly Dictionary<SubscriptionKey, Subject<IModel>> _subjects;

  public SubscriptionManager()
  {
    _subjects = new Dictionary<SubscriptionKey, Subject<IModel>>();
  }

  public IObservableItem<IModel> On(ModelAction action, IModel model, [CallerFilePath] string? caller = null)
  {
    var key = new SubscriptionKey(caller ?? string.Empty, GetWhat(action, model));

    if (_subjects.ContainsKey(key))
    {
      throw new InvalidOperationException($"The subscription key '{key}' already exist");
    }

    var subject = new Subject<IModel>();
    _subjects.Add(key, subject);

    return new ObservableModel(this, key, subject);
  }

  private static string GetWhat(ModelAction action, IModel model)
  {
    return $"{model.GetType().Name} - [{action.ToString().ToUpperInvariant()}] - {model.Id}";
  }

  public void Notify(object? sender, ModelAction action, IModel model)
  {
    var what = GetWhat(action, model);

    var keys = _subjects.Where(x => x.Key.What.Equals(what, StringComparison.Ordinal));

    foreach (var key in keys)
    {
      key.Value.Notify(sender, model);
    }
  }

  public void Remove(SubscriptionKey key)
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
