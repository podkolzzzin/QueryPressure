using QueryPressure.Arguments;
using QueryPressure.Core.Interfaces;
using QueryPressure.Factories;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace QueryPressure.Tests;

public static class TestUtils
{
  public static T Create<T>(SettingsFactory<T> factory, string yml) where T : ISetting
  {
    var args = Deserialize(yml);
    return factory.CreateProfile(args);
  }
    
  private static ApplicationArguments Deserialize(string fileContent)
  {
    var deserializer = new DeserializerBuilder()
      .WithNamingConvention(CamelCaseNamingConvention.Instance)
      .Build();

    return deserializer.Deserialize<ApplicationArguments>(fileContent);
  }
}