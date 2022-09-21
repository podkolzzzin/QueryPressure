using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using QueryPressure.Interfaces;

namespace QueryPressure.ProfileCreators;

public class LimitedConcurrencyWithDelayLoadProfileCreator : IProfileCreator
{
    public string ProfileTypeName => "limitedConcurrencyWithDelay";
    
    public IProfile Create(ProfileArguments profile) => new LimitedConcurrencyWithDelayLoadProfile(
            profile.ExtractIntArgumentOrThrow("limit"),
            profile.ExtractTimeSpanArgumentOrThrow("delay")
        );
}