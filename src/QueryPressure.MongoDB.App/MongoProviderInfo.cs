using QueryPressure.Core;

namespace QueryPressure.MongoDB.App;

public record MongoProviderInfo()
  : ProviderInfo("MongoDB", new[] { "mongo", "javascript" }, "")
{
}
