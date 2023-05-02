using System.Reflection;
using AvalonDock.Layout;

namespace QueryPressure.WinUI.ViewModels.DockElements;

class LayoutInitializer : ILayoutUpdateStrategy
{

  private static bool BeforeInsertContent(LayoutRoot layout, LayoutContent anchorableToShow)
  {
    var viewModel = (PaneViewModel)anchorableToShow.Content;
    var layoutContent = layout.Descendents().OfType<LayoutContent>().FirstOrDefault(x => x.ContentId == viewModel.ContentId);

    if (layoutContent == null)
      return false;

    layoutContent.Content = anchorableToShow.Content;

    // Add layoutContent to it's previous container
    var layoutContainer = layoutContent.GetType()
      .GetProperty("PreviousContainer", BindingFlags.NonPublic | BindingFlags.Instance)?
      .GetValue(layoutContent, null) as ILayoutContainer;

    switch (layoutContainer)
    {
      case LayoutAnchorablePane pane:
        pane.Children.Add(layoutContent as LayoutAnchorable);
        break;
      case LayoutDocumentPane documentPane:
        documentPane.Children.Add(layoutContent);
        break;
      default:
        throw new NotSupportedException();
    }
    return true;
  }

  public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
  {
    return BeforeInsertContent(layout, anchorableToShow);
  }


  public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
  {
  }


  public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
  {
    return BeforeInsertContent(layout, anchorableToShow);
  }

  public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
  {
  }
}
