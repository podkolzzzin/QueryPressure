using System.Collections.Generic;
using QueryPressure.App.Arguments;
using QueryPressure.App.Factories;
using QueryPressure.App.Interfaces;
using QueryPressure.App.LimitCreators;
using QueryPressure.App.ScriptSourceCreators;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Limits;
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

public class LimitFactoryTests
{
  private readonly SettingsFactory<ILimit> _factory;
    
  public LimitFactoryTests()
  {
    _factory = new SettingsFactory<ILimit>("limit", new ICreator<ILimit>[]
    {
      new QueryCountLimitCreator(),
      new TimeLimitCreator()
    });
  }
    
  [Fact]
  public void Create_QueryCountLimit_IsCreated()
  {
    var yml = @"
limit:
    type: queryCount
    arguments:
        limit: 10";

    Assert.IsType<QueryCountLimit>(TestUtils.Create(_factory, yml));
  }
    
  [Fact]
  public void Create_TimeLimit_IsCreated()
  {
    var yml = @"
limit:
    type: time
    arguments:
        limit: 10:00:00";

    Assert.IsType<TimeLimit>(TestUtils.Create(_factory, yml));
  }
}