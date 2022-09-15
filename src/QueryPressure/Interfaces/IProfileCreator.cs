using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.Interfaces;

public interface IProfileCreator
{
    string ProfileTypeName { get; }

    IProfile Create(ProfileArguments arguments);
}