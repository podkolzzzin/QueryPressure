using Autofac;
using QueryPressure.App;
using QueryPressure.App.Arguments;
using QueryPressure.Postgres.App;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

internal class Loader
{
  public Loader()
  {
  }

  public IContainer Load(string[] args)
  {
    var builder = new ContainerBuilder();
    var appArgs = Merge(args);

    builder.RegisterInstance(appArgs).AsSelf();
    builder.RegisterModule<AppModule>();
    builder.RegisterModule<PostgresAppModule>();
        
    return builder.Build();
  }

  private ApplicationArguments Merge(string[] args)
  {
    var configExtenstions = new[] { ".yml", ".yaml" };
    var configFiles = args.Where(x => configExtenstions.Contains(Path.GetExtension(x)));
    var scriptFile = args.Single(x => Path.GetExtension(x) == ".sql");

    var result = new ApplicationArguments();
    foreach (var configFile in configFiles)
    {
      var appArgs = Deserialize(File.ReadAllText(configFile));
      foreach (var applicationArgument in appArgs)
      {
        result.Add(applicationArgument.Key, applicationArgument.Value);
      }
    }
    
    result.Add("script", new ArgumentsSection() {
      Type = "file",
      Arguments = new() {
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