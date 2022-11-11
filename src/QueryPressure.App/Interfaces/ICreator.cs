using QueryPressure.App.Arguments;

namespace QueryPressure.App.Interfaces;

public interface ICreator<out T>
{
    string Type { get; }

    T Create(ArgumentsSection argumentsSection);
}