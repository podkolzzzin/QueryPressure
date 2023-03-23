using QueryPressure.App.Arguments;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.App.Interfaces;

public interface IProfileCreator : ICreator<IProfile>, IArgumentProvider
{
}
