using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.Interfaces;

public interface ICreator<out T>
{
    string Type { get; }

    T Create(ArgumentsSection argumentsSection);
}