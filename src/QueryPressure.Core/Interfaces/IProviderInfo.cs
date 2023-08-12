namespace QueryPressure.Core.Interfaces;

public interface IProviderInfo
{
  string Name { get; }
  string[] SyntaxAliases { get; }
  string? InitialScript { get; }
}
