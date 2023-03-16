using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;

namespace QueryPressure.App.ProfileCreators;

public class LimitedConcurrencyWithDelayLoadCreator : IProfileCreator
{
  public string Type => "limitedConcurrencyWithDelay";

  public ArgumentDescriptor[] Arguments => new[] {
    new ArgumentDescriptor("limit", ArgumentType.INT),
    new ArgumentDescriptor("delay", ArgumentType.TIME_SPAN)
  };

  public IProfile Create(ArgumentsSection profile) => new LimitedConcurrencyWithDelayLoadProfile(
          profile.ExtractIntArgumentOrThrow("limit"),
          profile.ExtractTimeSpanArgumentOrThrow("delay")
      );
}
