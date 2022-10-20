using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using QueryPressure.Interfaces;

namespace QueryPressure.ProfileCreators;

public class TargetThroughputLoadCreator : ICreator<IProfile>
{
    public string Type => "targetThroughput";
    
    public IProfile Create(ArgumentsSection profile) => new TargetThroughputLoadProfile(
        profile.ExtractIntArgumentOrThrow("rps")
    );
}