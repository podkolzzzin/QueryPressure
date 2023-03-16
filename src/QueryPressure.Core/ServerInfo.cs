using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core;

public record ServerInfo(string ServerVersion) : IServerInfo;
