using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using QueryPressure.Interfaces;

namespace QueryPressure.ProfileCreators;

public class SequentialWithDelayLoadCreator : ICreator<IProfile>
{
    public string Type => "sequentialWithDelay";
    
    public IProfile Create(ArgumentsSection profile) => new SequentialWithDelayLoadProfile(
        profile.ExtractTimeSpanArgumentOrThrow("delay")
    );
}