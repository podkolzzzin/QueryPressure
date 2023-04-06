using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.ScriptSources;

public class TextScriptSource : IScriptSource
{
  private readonly string _text;

  public TextScriptSource(string text)
  {
    _text = text;
  }

  public Task<IScript> GetScriptAsync(CancellationToken cancellationToken)
  {
    return Task.FromResult<IScript>(new TextScript(_text));
  }
}
