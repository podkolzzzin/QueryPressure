using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.Requirements;

public class ConnectionRequirement : IRequirement
{
  public int ConnectionCount { get; }

  public ConnectionRequirement() : this(1)
  {

  }

  public ConnectionRequirement(int connectionCount)
  {
    ConnectionCount = connectionCount;

  }
  public int CompareTo(IRequirement? other)
  {
    if (ReferenceEquals(this, other))
      return 0;
    if (ReferenceEquals(null, other))
      return 1;

    if (other is not ConnectionRequirement otherC)
      throw new ArgumentException("Cannot compare ConnectionRequirement with other type.");

    return ConnectionCount.CompareTo(otherC.ConnectionCount);
  }
}
