using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;

namespace QueryPressure.App.ProfileCreators;

public class TargetThroughputLoadCreator : ICreator<IProfile>
{
    public string Type => "targetThroughput";
    
    public IProfile Create(ArgumentsSection profile) => new TargetThroughputLoadProfile(
        profile.ExtractIntArgumentOrThrow("rps")
    );
}