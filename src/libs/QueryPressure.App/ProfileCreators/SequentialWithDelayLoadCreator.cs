using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;

namespace QueryPressure.App.ProfileCreators;

public class SequentialWithDelayLoadCreator : IProfileCreator
{
  public string Type => "sequentialWithDelay";

  public ArgumentDescriptor[] Arguments => new[] {
    new ArgumentDescriptor("delay", ArgumentType.TIME_SPAN)
  };

  public IProfile Create(ArgumentsSection profile) => new SequentialWithDelayLoadProfile(
      profile.ExtractTimeSpanArgumentOrThrow("delay")
  );
}
