using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Limits;

namespace QueryPressure.App.LimitCreators;

public class TimeLimitCreator : ICreator<ILimit>
{
  public string Type => "time";
  public ILimit Create(ArgumentsSection argumentsSection)
  {
    return new TimeLimit(argumentsSection.ExtractTimeSpanArgumentOrThrow("limit"));
  }
}