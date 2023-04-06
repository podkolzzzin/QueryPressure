using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.ScriptSources;

namespace QueryPressure.App.ScriptSourceCreators;

public class TextScriptSourceCreator : ICreator<IScriptSource>
{
  public string Type => "text";
  public IScriptSource Create(ArgumentsSection argumentsSection)
  {
    return new TextScriptSource(argumentsSection.ExtractStringArgumentOrThrow("text"));
  }
}
