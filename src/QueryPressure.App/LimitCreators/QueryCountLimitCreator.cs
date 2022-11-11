using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Limits;

namespace QueryPressure.App.LimitCreators;

public class QueryCountLimitCreator : ICreator<ILimit>
{
  public string Type => "queryCount";
  public ILimit Create(ArgumentsSection argumentsSection)
  {
    return new QueryCountLimit(argumentsSection.ExtractIntArgumentOrThrow("limit"));
  }
}