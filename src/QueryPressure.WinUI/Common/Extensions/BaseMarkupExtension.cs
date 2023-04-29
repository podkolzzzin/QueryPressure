using System.Windows;
using System.Windows.Markup;

namespace QueryPressure.WinUI.Common.Extensions;

public abstract class BaseMarkupExtension : MarkupExtension
{
  static BaseMarkupExtension()
  {
    var currentApplication = Application.Current;

    if (currentApplication is App app)
    {
      ServiceProvider = app.ServiceProvider;
    }
  }

  protected static IServiceProvider? ServiceProvider { get; }
}
