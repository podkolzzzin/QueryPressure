using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using QueryPressure.Interfaces;

namespace QueryPressure.ProfileCreators;

public class SequentialLoadProfileCreator : IProfileCreator
{
    public string ProfileTypeName => "sequential";
    
    public IProfile Create(ProfileArguments profile) => new SequentialLoadProfile();
}