using QueryPressure.Core.Interfaces;

namespace QueryPressure.WinUI.Services.Execute;

public record ExecutionVisualization(IMetric[] Metrics) : IVisualization;
