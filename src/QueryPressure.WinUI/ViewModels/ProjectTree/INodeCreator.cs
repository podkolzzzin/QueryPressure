using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.ViewModels.ProjectTree;

public interface INodeCreator
{
  BaseNodeViewModel Create<T>(T model) where T : IModel;
}
