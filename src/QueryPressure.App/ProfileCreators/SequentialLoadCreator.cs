using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;

namespace QueryPressure.App.ProfileCreators;

public class SequentialLoadCreator : IProfileCreator
{
  public string Type => "sequential";
  
  

  public IProfile Create(ArgumentsSection profile) => new SequentialLoadProfile();
  public ArgumentDescriptor[] Arguments => Array.Empty<ArgumentDescriptor>();
}
