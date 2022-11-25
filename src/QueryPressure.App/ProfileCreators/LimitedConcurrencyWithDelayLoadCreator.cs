using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;

namespace QueryPressure.App.ProfileCreators;

public class LimitedConcurrencyWithDelayLoadCreator : ICreator<IProfile>
{
  public string Type => "limitedConcurrencyWithDelay";

  public IProfile Create(ArgumentsSection profile) => new LimitedConcurrencyWithDelayLoadProfile(
          profile.ExtractIntArgumentOrThrow("limit"),
          profile.ExtractTimeSpanArgumentOrThrow("delay")
      );
}
