using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Limits;

namespace QueryPressure.App.LimitCreators;

public sealed class TillFirstErrorLimitCreator : ICreator<ILimit>
{
  public string Type => "tillFirstError";

  public ILimit Create(ArgumentsSection argumentsSection)
  {
    return new TillNErrorsLimit(1);
  }
}
