using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;

namespace QueryPressure.App.ProfileCreators;

public class LimitedConcurrencyLoadCreator : IProfileCreator
{
  public string Type => "limitedConcurrency";

  public ArgumentDescriptor[] Arguments => new[] { new ArgumentDescriptor("limit", ArgumentType.INT) };

  public IProfile Create(ArgumentsSection profile)
  {
    return new LimitedConcurrencyLoadProfile(profile.ExtractIntArgumentOrThrow("limit"));
  }
}
