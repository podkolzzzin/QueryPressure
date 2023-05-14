using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.ViewModels.ProjectTree;

public class ScenarioNodeViewModel : BaseNodeViewModel, IDisposable
{
  private readonly ISubscription _subscription;

  public ScenarioNodeViewModel(ISubscriptionManager subscriptionManager, ScenarioModel scenarioModel) : base(scenarioModel, true)
  {
    _subscription = subscriptionManager
      .On(ModelAction.Edit, scenarioModel)
      .Subscribe(OnModelEdit);
  }

  private void OnModelEdit(IModel value)
  {
    Model = value;
    OnOtherPropertyChanged(nameof(Title));
  }

  public string Title => ScenarioModel.Name;

  private ScenarioModel ScenarioModel => (ScenarioModel)Model;

  public void Dispose()
  {
    _subscription.Dispose();
  }
}
