using System.Windows.Input;
using QueryPressure.WinUI.Commands.Scenario;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.ViewModels.ProjectTree;

public class ScenarioNodeViewModel : BaseNodeViewModel, IDisposable
{
  private readonly OpenScenarioScriptCommand _openScenarioScriptCommand;
  private readonly ISubscription _subscription;

  public ScenarioNodeViewModel(ISubscriptionManager subscriptionManager, OpenScenarioScriptCommand openScenarioScriptCommand, ScenarioModel scenarioModel) : base(scenarioModel, true)
  {
    _openScenarioScriptCommand = openScenarioScriptCommand;
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

  public override void Click(MouseButtonEventArgs args, bool isDoubleClick = false)
  {
    if (isDoubleClick)
    {
      _openScenarioScriptCommand.Execute(ScenarioModel);
    }
  }

  public void Dispose()
  {
    _subscription.Dispose();
  }
}
