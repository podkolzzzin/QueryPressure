using Autofac;
using QueryPressure.WinUI.Common.Observer;

namespace QueryPressure.WinUI.Extensions
{
  public static class ContainerBuilderExtensions
  {
    public static IDisposable RegisterSubject<TPayload>(this ContainerBuilder builder)
    {
      var instance = new Subject<TPayload>();
      builder.RegisterInstance(instance).As<ISubject<TPayload>>().ExternallyOwned();
      builder.RegisterInstance(instance).As<IObservableItem<TPayload>>().ExternallyOwned();
      return instance;
    }
  }
}
