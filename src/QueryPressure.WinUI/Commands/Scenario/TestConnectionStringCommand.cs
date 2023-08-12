using Microsoft.Extensions.Logging;
using QueryPressure.Core.Interfaces;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Dtos;
using QueryPressure.WinUI.Services;
using QueryPressure.WinUI.Services.Language;
using System.Windows;

namespace QueryPressure.WinUI.Commands.Scenario;

public class TestConnectionStringCommand : CommandBase<TestConnectionStringDto>
{
  private readonly ITestConnectionStringService _testConnectionStringService;
  private readonly ILanguageService _languageService;

  public TestConnectionStringCommand(ILogger<TestConnectionStringCommand> logger,
    ITestConnectionStringService testConnectionStringService,
    ILanguageService languageService) : base(logger)
  {
    _testConnectionStringService = testConnectionStringService;
    _languageService = languageService;
  }

  protected override void ExecuteInternal(TestConnectionStringDto parameter)
  {
    _testConnectionStringService.TestConnectionAsync(parameter.Provider, parameter.ConnectionString, default).ContinueWith(CheckTestResult);
  }

  private void CheckTestResult(Task<IServerInfo> task)
  {
    var strings = _languageService.GetStrings();
    try
    {

      var result = task.Result;

      if (result is null)
      {
        ShowError(strings, "NULL Result");
      }
      else
      {
        MessageBox.Show(
        string.Format(strings["labels.dialogs.succeed-test-connectionString.message"], result.ServerVersion),
        strings["labels.dialogs.succeed-test-connectionString.title"],
        MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }
    catch (Exception ex)
    {
      var message = ex.InnerException?.Message ?? ex.Message;
      ShowError(strings, message);
    }
  }

  private static void ShowError(IDictionary<string, string> strings, string error)
  {
    MessageBox.Show(
      string.Format(strings["labels.dialogs.failed-test-connectionString.message"], error),
      strings["labels.dialogs.failed-test-connectionString.title"],
      MessageBoxButton.OK, MessageBoxImage.Error);
  }
}
