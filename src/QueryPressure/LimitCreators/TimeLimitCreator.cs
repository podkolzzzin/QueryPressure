using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Limits;
using QueryPressure.Interfaces;

namespace QueryPressure.LimitCreators;

public class TimeLimitCreator : ICreator<ILimit>
{
  public string Type => "time";
  public ILimit Create(ArgumentsSection argumentsSection)
  {
    return new TimeLimit(argumentsSection.ExtractTimeSpanArgumentOrThrow("limit"));
  }
}