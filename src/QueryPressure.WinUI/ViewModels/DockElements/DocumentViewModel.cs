using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.ViewModels.DockElements;

public class DocumentViewModel : PaneViewModel, IDisposable
{
  public DocumentViewModel(IModel model)
  {
    ContentId = model.Id.ToString();
  }

  public string FilePath => Title;

  public virtual void Dispose()
  {
  }
}
