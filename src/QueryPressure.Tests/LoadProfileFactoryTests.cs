using System;
using System.Threading.Tasks;
using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.LoadProfiles;
using QueryPressure.Factories;
using QueryPressure.Interfaces;
using QueryPressure.ProfileCreators;
using Xunit;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace QueryPressure.Tests;

public class LoadProfileFactoryTests
{
    private readonly LoadProfilesFactory _factory;
    
    public LoadProfileFactoryTests()
    {
        _factory = new LoadProfilesFactory(new IProfileCreator[]
        {
            new SequentialLoadProfileCreator(),
            new SequentialWithDelayLoadProfileCreator(),
            new LimitedConcurrencyLoadProfileCreator(),
            new LimitedConcurrencyWithDelayLoadProfileCreator(),
            new TargetThroughputLoadProfileCreator()
        });
    }

    private IProfile CreateProfile(string yml)
    {
        var args = Deserialize(yml);
        return _factory.CreateProfile(args);
    }
    
    private static ApplicationArguments Deserialize(string fileContent)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        return deserializer.Deserialize<ApplicationArguments>(fileContent);
    }

    [Fact]
    public void Create_SequentialLoadProfileIsCreated()
    {
        var yml = @"
profile:
    type: sequential";

        Assert.IsType<SequentialLoadProfile>(CreateProfile(yml));
    }
    
    [Fact]
    public void Create_LimitedConcurrencyLoadProfile_IsCreated()
    {
        var yml = @"
profile:
    type: limitedConcurrency
    arguments: 
        limit: 2";

        Assert.IsType<LimitedConcurrencyLoadProfile>(CreateProfile(yml));
    }
    
    [Fact]
    public void Create_LimitedConcurrencyLoadProfile_ThrowsOnMissedArgument()
    {
        var yml = @"
profile:
    type: limitedConcurrency";

        Assert.Throws<ArgumentException>(() => CreateProfile(yml));
    }
    
    [Fact]
    public void Create_LimitedConcurrencyLoadProfile_ThrowsOnInvalidArgument()
    {
        var yml = @"
profile:
    type: limitedConcurrency
    arguments: 
        limit: ololo";

        Assert.Throws<ArgumentException>(() => CreateProfile(yml));
    }
    
    [Fact]
    public void Create_LimitedConcurrencyWithDelayLoadProfile_IsCreated()
    {
        var yml = @"
profile:
    type: limitedConcurrency
    arguments: 
        limit: 2
        delay: 00:00:01";

        Assert.IsType<LimitedConcurrencyLoadProfile>(CreateProfile(yml));
    }
    
    [Fact]
    public void Create_SequentialWithDelayLoadProfile_IsCreated()
    {
        var yml = @"
profile:
    type: sequentialWithDelay
    arguments:
        delay: 00:00:01";

        Assert.IsType<SequentialWithDelayLoadProfile>(CreateProfile(yml));
    }
    
    [Fact]
    public void Create_TargetThroughputLoadProfile_IsCreated()
    {
        var yml = @"
profile:
    type: targetThroughput
    arguments:
        rps: 50";

        Assert.IsType<TargetThroughputLoadProfile>(CreateProfile(yml));
    }
    
    [Fact]
    public void Create_LimitedConcurrencyWithDelayLoadProfile_ThrowsOnMissedArgument()
    {
        var yml = @"
profile:
    type: limitedConcurrencyWithDelay
    arguments: 
        limit: ololo";

        Assert.Throws<ArgumentException>(() => CreateProfile(yml));
    }
}