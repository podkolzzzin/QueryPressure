using QueryPressure.Core.Interfaces;

namespace QueryPressure.Metrics.Core;

public record SimpleMetric(string Name, object Value) : IMetric;
