using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;

namespace QueryPressure.App.ProfileCreators;

public class SequentialLoadCreator : ICreator<IProfile>
{
  public string Type => "sequential";

  public IProfile Create(ArgumentsSection profile) => new SequentialLoadProfile();
}
