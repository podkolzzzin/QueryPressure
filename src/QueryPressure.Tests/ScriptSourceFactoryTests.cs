using QueryPressure.App.Arguments;
using QueryPressure.App.Factories;
using QueryPressure.App.Interfaces;
using QueryPressure.App.ScriptSourceCreators;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.ScriptSources;
using Xunit;

namespace QueryPressure.Tests;

public class ScriptSourceFactoryTests
{
  private readonly SettingsFactory<IScriptSource> _factory;

  public ScriptSourceFactoryTests()
  {
    _factory = new SettingsFactory<IScriptSource>("script", new ICreator<IScriptSource>[]
    {
      new FileScriptSourceCreator()
    });
  }

  [Fact]
  public void Create_FileScriptSource_IsCreated()
  {
    var section = new ArgumentsSection() {
      Type = "file",
      Arguments = new() {
        ["path"] = "file.sql"
      }
    };

    Assert.IsType<FileScriptSource>(_factory.Create(new ApplicationArguments() {
      ["script"] = section
    }));
  }
}