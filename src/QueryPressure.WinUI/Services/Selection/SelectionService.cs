using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.Services.Selection;

public class SelectionService : ISelectionService
{
  
  private readonly ISubject<Selection> _subject;
  private Selection _selection;

  public SelectionService(ISubject<Selection> subject)
  {
    _subject = subject;
    _selection = new Selection();
  }

  public void Set(IModel? model)
  {
    _selection.Model = model;
    _subject.Notify(_selection);
  }

  public IModel? Get()
  {
    return _selection.Model;
  }
}
