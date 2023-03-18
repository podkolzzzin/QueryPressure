using QueryPressure.App.Arguments;

namespace QueryPressure.App.Interfaces;

public interface IArgumentProvider
{
  ArgumentDescriptor[] Arguments { get; }
}
