using QueryPressure.WinUI.Services.Execute;
using System.Text.Json.Serialization;

namespace QueryPressure.WinUI.Models;

public class ExecutionModel : IModel
{
  public ExecutionModel(Guid guid)
  {
    Id = guid;
    Status = ExecutionStatus.None;
  }

  public Guid Id { get; set; }

  public ExecutionStatus Status { get; set; }

  [JsonIgnore]
  public ExecutionVisualization? LiveMetrics { get; set; }

  [JsonIgnore]
  public string Title => StartTime.ToLocalTime().ToString("G") ?? Id.ToString();

  public ExecutionVisualization? ResultMetrics { get; set; }

  public DateTime StartTime { get; set; }

  public DateTime EndTime { get; set; }
}
