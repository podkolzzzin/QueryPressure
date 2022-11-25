using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.ScriptSources;

namespace QueryPressure.App.ScriptSourceCreators;

public class FileScriptSourceCreator : ICreator<IScriptSource>
{
  public string Type => "file";
  public IScriptSource Create(ArgumentsSection argumentsSection)
  {
    return new FileScriptSource(argumentsSection.ExtractStringArgumentOrThrow("path"));
  }
}
