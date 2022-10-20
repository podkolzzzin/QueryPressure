using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using QueryPressure.Interfaces;

namespace QueryPressure.ProfileCreators;

public class LimitedConcurrencyLoadCreator : ICreator<IProfile>
{
    public string Type => "limitedConcurrency";
    
    public IProfile Create(ArgumentsSection profile)
    {
        return new LimitedConcurrencyLoadProfile(profile.ExtractIntArgumentOrThrow("limit"));
    }
}