using Autofac;
using QueryPressure.App;
using QueryPressure.App.Arguments;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

internal class ConsoleApplicationLoader : ApplicationLoader
{
  private readonly string[] _args;
  public ConsoleApplicationLoader(string[] args)
  {
    _args = args;
  }

  public override ContainerBuilder Load(ContainerBuilder builder)
  {
    base.Load(builder);

    var appArgs = Merge(_args);

    builder.RegisterInstance(appArgs).AsSelf();
    return builder;
  }

  private ApplicationArguments Merge(string[] args)
  {
    var configExtensions = new[] { ".yml", ".yaml" };

    var configFiles = args.Where(x => configExtensions.Contains(Path.GetExtension(x)));
    var scriptFile = args.Single(x => !configExtensions.Contains(Path.GetExtension(x)));

    var result = new ApplicationArguments();
    foreach (var configFile in configFiles)
    {
      var appArgs = Deserialize(File.ReadAllText(configFile));
      foreach (var applicationArgument in appArgs)
      {
        result.Add(applicationArgument.Key, applicationArgument.Value);
      }
    }

    result.Add("script", new ArgumentsSection()
    {
      Type = "file",
      Arguments = new()
      {
        ["path"] = scriptFile
      }
    });

    return result;
  }

  private ApplicationArguments Deserialize(string fileContent)
  {
    var deserializer = new DeserializerBuilder()
      .WithNamingConvention(CamelCaseNamingConvention.Instance)
      .Build();

    return deserializer.Deserialize<ApplicationArguments>(fileContent);
  }
}
