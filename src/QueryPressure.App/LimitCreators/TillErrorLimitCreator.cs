using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Limits;

namespace QueryPressure.App.LimitCreators;

public sealed class TillNErrorLimitCreator : ICreator<ILimit>
{
  public string Type => "tillNErrors";

  public ILimit Create(ArgumentsSection argumentsSection)
  {
    return new TillNErrorsLimit(argumentsSection.ExtractIntArgumentOrThrow("errorCount"));
  }
}
