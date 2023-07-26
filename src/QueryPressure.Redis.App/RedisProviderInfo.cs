using QueryPressure.Core;

namespace QueryPressure.Redis.App;

public record RedisProviderInfo()
  : ProviderInfo("Redis", new[] { "redis", "lua" }, Script)
{
  // source: https://github.com/microsoft/monaco-editor/blob/main/website/src/website/data/home-samples/sample.redis.txt
  private const string Script = """
EXISTS mykey
APPEND mykey "Hello"
APPEND mykey " World"
GET mykey
""";
}
