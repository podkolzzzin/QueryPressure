using System.Reflection;
using System.Windows.Input;
using Autofac;
using QueryPressure.App;
using QueryPressure.WinUI.Extensions;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.ViewModels;
using QueryPressure.WinUI.Views;

namespace QueryPressure.WinUI;
public class WinApplicationLoader : ApplicationLoader
{
  private readonly IList<IDisposable> _subjects;

  public WinApplicationLoader(IList<IDisposable> subjects)
  {
    _subjects = subjects;
  }

  public override ContainerBuilder Load(ContainerBuilder builder)
  {
    //register dependencies
    builder.RegisterType<LanguageService>().As<ILanguageService>().SingleInstance();

    builder.RegisterType<ShellViewModel>().SingleInstance();
    builder.RegisterType<Shell>().SingleInstance();

    builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
      .Where(t => typeof(ICommand).IsAssignableFrom(t));


    _subjects.Add(builder.RegisterSubject<LanguageItem>());

    return base.Load(builder);
  }
}
