using QueryPressure.App.Arguments;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.App.Interfaces;

public interface ICreator<out T>
{
  string Type { get; }

  T Create(ArgumentsSection argumentsSection);
}


public record ArgumentDescriptor(string Name, string Type);
public interface IProfileCreator : ICreator<IProfile>
{
  ArgumentDescriptor[] Arguments { get; }
}
