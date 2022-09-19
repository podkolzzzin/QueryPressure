using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using QueryPressure.Interfaces;

namespace QueryPressure.ProfileCreators;

public class LimitedConcurrencyLoadProfileWithDelayCreator : IProfileCreator
{
    public string ProfileTypeName => "limitedConcurrencyWithDelay";
    
    public IProfile Create(ProfileArguments profile) => new LimitedConcurrencyLoadProfileWithDelay(
            profile.ExtractIntArgumentOrThrow("limit"),
            profile.ExtractTimeSpanArgumentOrThrow("delay")
        );
}