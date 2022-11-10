using System.Collections.Generic;
using QueryPressure.App.Factories;
using QueryPressure.App.Interfaces;
using QueryPressure.App.LimitCreators;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Limits;
using Xunit;

namespace QueryPressure.Tests;

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