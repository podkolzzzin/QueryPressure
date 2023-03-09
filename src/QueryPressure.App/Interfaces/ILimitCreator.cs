using QueryPressure.App.Arguments;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.App.Interfaces
{
  public interface ILimitCreator : ICreator<ILimit>
  {
    ArgumentDescriptor[] Arguments { get; }
  }
}
