using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using QueryPressure.Interfaces;

namespace QueryPressure.ProfileCreators;

public class LimitedConcurrencyLoadProfileCreator : IProfileCreator
{
    public string ProfileTypeName => "limitedConcurrency";
    
    public IProfile Create(ProfileArguments profile)
    {
        return new LimitedConcurrencyLoadProfile(profile.ExtractIntArgumentOrThrow("limit"));
    }
}