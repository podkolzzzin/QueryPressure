using QueryPressure.Core.Interfaces;
using QueryPressure.WinUI.Services.Execute;
using System.Text.Json.Serialization;

namespace QueryPressure.WinUI.Models;

public class ExecutionModel : IModel
{
  public ExecutionModel()
  {
  }

  public ExecutionModel(Guid guid)
  {
    Id = guid;
    Status = ExecutionStatus.None;
  }

  public Guid Id { get; set; }

  public ExecutionStatus Status { get; set; }

  public DateTime StartTime { get; set; }

  public DateTime EndTime { get; set; }

  [JsonIgnore]
  public string Title => StartTime.ToLocalTime().ToString("G") ?? Id.ToString();

  [JsonIgnore]
  public ExecutionVisualization? RealtimeMetrics { get; set; }

  [JsonIgnore]
  public ExecutionVisualization? ResultMetrics { get; set; }

  public IEnumerable<ExecutionResult>? RowResults { get; set; }
}
