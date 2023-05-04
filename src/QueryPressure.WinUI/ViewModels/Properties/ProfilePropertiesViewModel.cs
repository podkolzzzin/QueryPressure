using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.ViewModels.Properties;

public class ProfilePropertiesViewModel : ViewModelBase, IDisposable
{
  public ProfilePropertiesViewModel(ISubscriptionManager subscriptionManager, ProfileModel profileModel)
  {
  }

  public void Dispose()
  {
  }
}
