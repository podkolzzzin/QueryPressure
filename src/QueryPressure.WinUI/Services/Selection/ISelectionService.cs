using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.Services.Selection;

public interface ISelectionService
{
  void Set(IModel? selection);

  IModel? Get();
}
