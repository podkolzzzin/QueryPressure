using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;
using QueryPressure.Redis.Core;

[assembly: QueryPressurePlugin]
namespace QueryPressure.Redis.App;

public sealed class RedisConnectionProviderCreator: ICreator<IConnectionProvider>
{
  public string Type => "redis";

  public IConnectionProvider Create(ArgumentsSection argumentsSection)
  {
    var connectionString = argumentsSection.ExtractStringArgumentOrThrow("connectionString");
    return new RedisConnectionProvider(connectionString);
  }
}
