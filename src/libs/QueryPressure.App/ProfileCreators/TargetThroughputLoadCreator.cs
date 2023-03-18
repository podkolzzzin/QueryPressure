using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;

namespace QueryPressure.App.ProfileCreators;

public class TargetThroughputLoadCreator : IProfileCreator
{
  public string Type => "targetThroughput";

  public ArgumentDescriptor[] Arguments => new[] {
    new ArgumentDescriptor("rps", ArgumentType.INT)
  };

  public IProfile Create(ArgumentsSection profile) => new TargetThroughputLoadProfile(
      profile.ExtractIntArgumentOrThrow("rps")
  );
}
