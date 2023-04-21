using Autofac;
using QueryPressure.App;
using QueryPressure.WinUI.ViewModels;
using QueryPressure.WinUI.Views;

namespace QueryPressure.WinUI;
public class WinApplicationLoader : ApplicationLoader
{
  public override ContainerBuilder Load(ContainerBuilder builder)
  {
    //register dependencies

    builder.RegisterType<ShellViewModel>().SingleInstance();
    builder.RegisterType<Shell>().SingleInstance();

    return base.Load(builder);
  }
}
