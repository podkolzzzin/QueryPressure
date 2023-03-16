using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Limits;

namespace QueryPressure.App.LimitCreators;

public class QueryCountLimitCreator : ILimitCreator
{
  private const string LIMIT_ARGUMENT_NAME = "limit";

  public ILimit Create(ArgumentsSection argumentsSection)
  {
    return new QueryCountLimit(argumentsSection.ExtractIntArgumentOrThrow(LIMIT_ARGUMENT_NAME));
  }

  public string Type => "queryCount";
  public ArgumentDescriptor[] Arguments { get; } = { new(LIMIT_ARGUMENT_NAME, ArgumentType.INT) };
}
