using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;

namespace QueryPressure.App.ProfileCreators;

public class SequentialWithDelayLoadCreator : ICreator<IProfile>
{
  public string Type => "sequentialWithDelay";

  public IProfile Create(ArgumentsSection profile) => new SequentialWithDelayLoadProfile(
      profile.ExtractTimeSpanArgumentOrThrow("delay")
  );
}
