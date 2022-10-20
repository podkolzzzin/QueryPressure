namespace QueryPressure.Core.Interfaces;

public interface IScriptSource : ISetting
{
  IScript GetScript();
}