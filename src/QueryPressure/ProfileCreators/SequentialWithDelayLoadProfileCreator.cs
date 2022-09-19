using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using QueryPressure.Interfaces;

namespace QueryPressure.ProfileCreators;

public class SequentialWithDelayLoadProfileCreator : IProfileCreator
{
    public string ProfileTypeName => "sequentialWithDelay";
    
    public IProfile Create(ProfileArguments profile) => new SequentialWithDelayLoadProfile(
        profile.ExtractTimeSpanArgumentOrThrow("delay")
    );
}