using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;

namespace QueryPressure.App.ProfileCreators;

public class LimitedConcurrencyLoadCreator : ICreator<IProfile>
{
  public string Type => "limitedConcurrency";

  public IProfile Create(ArgumentsSection profile)
  {
    return new LimitedConcurrencyLoadProfile(profile.ExtractIntArgumentOrThrow("limit"));
  }
}
