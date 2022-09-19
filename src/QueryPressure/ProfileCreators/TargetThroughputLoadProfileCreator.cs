using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using QueryPressure.Interfaces;

namespace QueryPressure.ProfileCreators;

public class TargetThroughputLoadProfileCreator : IProfileCreator
{
    public string ProfileTypeName => "targetThroughput";
    
    public IProfile Create(ProfileArguments profile) => new TargetThroughputLoadProfile(
        profile.ExtractIntArgumentOrThrow("rps")
    );
}