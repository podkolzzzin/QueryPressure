using QueryPressure.App.Arguments;

namespace QueryPressure.App.Interfaces;

public interface ISettingsFactory<out T>
{
  T Create(ApplicationArguments arguments);
}