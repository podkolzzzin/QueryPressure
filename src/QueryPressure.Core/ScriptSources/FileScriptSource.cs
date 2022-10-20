using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.ScriptSources;

public class FileScriptSource : IScriptSource
{
  private readonly string _path;

  public FileScriptSource(string path)
  {
    _path = path;

  }

  public IScript GetScript()
  {
    return new TextScript(File.ReadAllText(_path));
  }
}