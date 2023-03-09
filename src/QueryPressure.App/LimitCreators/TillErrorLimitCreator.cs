using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Limits;

namespace QueryPressure.App.LimitCreators;

public sealed class TillNErrorLimitCreator : ILimitCreator
{
  private const string ERROR_COUNT_ARGUMENT_NAME = "errorCount";

  public ILimit Create(ArgumentsSection argumentsSection)
  {
    return new TillNErrorsLimit(argumentsSection.ExtractIntArgumentOrThrow(ERROR_COUNT_ARGUMENT_NAME));
  }

  public string Type => "tillNErrors";
  public ArgumentDescriptor[] Arguments { get; } = { new(ERROR_COUNT_ARGUMENT_NAME, ArgumentType.INT) };
}
