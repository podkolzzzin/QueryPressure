using QueryPressure.WinUI.Common.Commands;
using Microsoft.Extensions.Logging;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.Commands.App;

public record EditModelCommandParameter(object Sender, IModel Model);

public class EditModelCommand : CommandBase<EditModelCommandParameter>
{
  private Timer? _timer;

  private readonly ISubscriptionManager _subscriptionManager;
  private readonly IDispatcherService _dispatcherService;

  public EditModelCommand(ILogger<EditModelCommand> logger, ISubscriptionManager subscriptionManager, IDispatcherService dispatcherService) : base(logger)
  {
    _subscriptionManager = subscriptionManager;
    _dispatcherService = dispatcherService;
  }

  protected override void ExecuteInternal(EditModelCommandParameter parameter)
  {
    _dispatcherService.Invoke(() => _subscriptionManager.Notify(parameter.Sender, ModelAction.Edit, parameter.Model));
  }

  public void DeBounce(EditModelCommandParameter parameter, TimeSpan delay = default)
  {
    if (delay == default)
    {
      delay = TimeSpan.FromSeconds(1);
    }

    _timer?.Dispose();
    _timer = new Timer(Execute, parameter, delay, TimeSpan.Zero);
  }

}
