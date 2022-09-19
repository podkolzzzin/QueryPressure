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
            new SequentialLoadProfileWithDelayCreator(),
            new LimitedConcurrencyLoadProfileCreator(),
            new SequentialLoadProfileWithDelayCreator(),
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
}