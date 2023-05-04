using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.ViewModels.ProjectTree;

public class ProfileNodeViewModel : BaseNodeViewModel, IDisposable
{
  private readonly ISubscription _subscription;

  public ProfileNodeViewModel(ISubscriptionManager subscriptionManager, ProfileModel profileModel) : base(profileModel, true)
  {
    _subscription = subscriptionManager
      .On(ModelAction.Edit, profileModel)
      .Subscribe(OnModelEdit);
  }

  private void OnModelEdit(IModel value)
  {
    Model = value;
    OnOtherPropertyChanged(nameof(Title));
  }

  public string Title => ProfileModel.Name;

  private ProfileModel ProfileModel => (ProfileModel)Model;

  public void Dispose()
  {
    _subscription.Dispose();
  }
}
