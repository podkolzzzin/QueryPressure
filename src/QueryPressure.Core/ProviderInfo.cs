using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core;

public record ProviderInfo(string Name, string[] SyntaxAliases, string? InitialScript = null) : IProviderInfo;
