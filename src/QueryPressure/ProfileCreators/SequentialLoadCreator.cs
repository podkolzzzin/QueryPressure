using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using QueryPressure.Interfaces;

namespace QueryPressure.ProfileCreators;

public class SequentialLoadCreator : ICreator<IProfile>
{
    public string Type => "sequential";
    
    public IProfile Create(ArgumentsSection profile) => new SequentialLoadProfile();
}