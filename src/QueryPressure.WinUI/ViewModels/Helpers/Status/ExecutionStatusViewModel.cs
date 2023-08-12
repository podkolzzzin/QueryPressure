using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.ViewModels.Helpers.Status;

public record ExecutionStatusViewModel(ExecutionStatus Value, string LabelLink, StatusIcon Icon);
