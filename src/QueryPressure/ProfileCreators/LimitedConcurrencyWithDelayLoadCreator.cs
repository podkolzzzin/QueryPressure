using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using QueryPressure.Interfaces;

namespace QueryPressure.ProfileCreators;

public class LimitedConcurrencyWithDelayLoadCreator  : ICreator<IProfile>
{
    public string Type => "limitedConcurrencyWithDelay";
    
    public IProfile Create(ArgumentsSection profile) => new LimitedConcurrencyWithDelayLoadProfile(
            profile.ExtractIntArgumentOrThrow("limit"),
            profile.ExtractTimeSpanArgumentOrThrow("delay")
        );
}