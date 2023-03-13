using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Limits;

namespace QueryPressure.App.LimitCreators;

public sealed class TillFirstErrorLimitCreator : ILimitCreator
{
  public ILimit Create(ArgumentsSection argumentsSection)
  {
    return new TillNErrorsLimit(1);
  }

  public string Type => "tillFirstError";
  public ArgumentDescriptor[] Arguments { get; } = Array.Empty<ArgumentDescriptor>();
}
