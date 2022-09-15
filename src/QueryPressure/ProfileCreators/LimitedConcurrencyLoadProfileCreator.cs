using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using QueryPressure.Interfaces;

namespace QueryPressure.ProfileCreators;

internal class LimitedConcurrencyLoadProfileCreator : IProfileCreator
{
    public string ProfileTypeName => "limitedConcurrency";
    
    public IProfile Create(ProfileArguments profile)
    {
        return new LimitedConcurrencyLoadProfile(int.Parse(profile.Arguments["limit"]));
    }
}