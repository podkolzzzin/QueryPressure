using System.Reflection;
using System.Windows.Input;
using Autofac;
using Autofac.Features.AttributeFilters;
using QueryPressure.App;
using QueryPressure.App.Console;
using QueryPressure.Core.Interfaces;
using QueryPressure.WinUI.Common.Converters;
using QueryPressure.WinUI.Extensions;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services;
using QueryPressure.WinUI.Services.Execute;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Services.Metric;
using QueryPressure.WinUI.Services.Project;
using QueryPressure.WinUI.Services.Selection;
using QueryPressure.WinUI.Services.Settings;
using QueryPressure.WinUI.Services.Subscriptions;
using QueryPressure.WinUI.Services.Theme;
using QueryPressure.WinUI.Services.WindowPosition;
using QueryPressure.WinUI.ViewModels;
using QueryPressure.WinUI.ViewModels.Helpers;
using QueryPressure.WinUI.ViewModels.Helpers.Status;
using QueryPressure.WinUI.ViewModels.ProjectTree;
using QueryPressure.WinUI.ViewModels.Properties;
using QueryPressure.WinUI.Views;

namespace QueryPressure.WinUI;
public class WinApplicationLoader : ApplicationLoader
{
  private readonly App _application;
  private readonly IList<IDisposable> _subjects;

  public WinApplicationLoader(App application, IList<IDisposable> subjects)
  {
    _application = application;
    _subjects = subjects;
  }

  public override ContainerBuilder Load(ContainerBuilder builder)
  {
    //register dependencies
    builder.RegisterInstance(_application).AsSelf().ExternallyOwned();

    builder.RegisterType<DefaultSettingsProvider>().As<IDefaultSettingsProvider>();
    builder.RegisterType<LanguageService>().As<ILanguageService>().SingleInstance();
    builder.RegisterType<WindowPositionService>().As<IWindowPositionService>().SingleInstance();
    builder.RegisterType<ThemeService>().As<IThemeService>().SingleInstance();
    builder.RegisterType<SettingsService>().As<ISettingsService>().SingleInstance();
    builder.RegisterType<ProjectService>().As<IProjectService>().SingleInstance().WithAttributeFiltering();
    builder.RegisterType<SubscriptionManager>().As<ISubscriptionManager>().SingleInstance();
    builder.RegisterType<SelectionService>().As<ISelectionService>().SingleInstance();
    builder.RegisterType<DispatcherService>().As<IDispatcherService>().SingleInstance();
    builder.RegisterType<ExecutionService>().As<IExecutionService>().SingleInstance();
    builder.RegisterType<TestConnectionStringService>().As<ITestConnectionStringService>();

    builder.RegisterType<LocaleViewModel>().SingleInstance();
    builder.RegisterType<LocaleStringValueConverter>().SingleInstance();
    builder.RegisterType<ThemeViewModel>().SingleInstance();
    builder.RegisterType<NodeCreator>().As<INodeCreator>();
    builder.RegisterType<ProjectTreeViewModel>().SingleInstance();
    builder.RegisterType<PropertiesViewModel>().SingleInstance();
    builder.RegisterType<DockToolsViewModel>().SingleInstance();
    builder.RegisterType<MenuViewModel>().SingleInstance();
    builder.RegisterType<ShellViewModel>().SingleInstance();
    builder.RegisterType<Shell>().SingleInstance();
    builder.RegisterType<ExecutionStatusProvider>().As<IExecutionStatusProvider>().SingleInstance();

    builder.RegisterType<ExecutionVisualizer>()
      .Keyed<IMetricsVisualizer>(ExecutionVisualizer.Key)
      .SingleInstance();

    builder.RegisterType<Execution>()
    .AsSelf()
    .InstancePerLifetimeScope()
    .WithAttributeFiltering();

    builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
      .Where(t => typeof(ICommand).IsAssignableFrom(t));


    _subjects.Add(builder.RegisterSubject<LanguageItem>());
    _subjects.Add(builder.RegisterSubject<ApplicationTheme>());
    _subjects.Add(builder.RegisterSubject<ProjectModel?>());
    _subjects.Add(builder.RegisterSubject<Selection>());

    builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
      .Where(t => typeof(IMetricValueViewModelCreator).IsAssignableFrom(t))
      .As<IMetricValueViewModelCreator>();

    builder.RegisterType<MetricValueViewModelFactory>().As<IMetricViewModelFactory>();
    builder.RegisterType<DefaultMetricValueViewModelCreator>();

    return base.Load(builder);
  }
}
