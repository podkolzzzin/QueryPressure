using QueryPressure.App.Arguments;

namespace QueryPressure.UI;

public record ConnectionRequest(string ConnectionString, string Provider);

public record ExecutionRequest(string ConnectionString, string Provider, string Script, FlatArgumentsSection Profile, FlatArgumentsSection Limit);
