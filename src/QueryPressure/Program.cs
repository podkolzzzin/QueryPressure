// See https://aka.ms/new-console-template for more information

using QueryPressure.Arguments;
using QueryPressure.Core.Factories;
using QueryPressure.ProfileCreators;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

Console.WriteLine("Hello, World!");
var file = @"
profile:
    type: limitedConcurrency
    arguments:
        limit: 10
limit:
    type: queryCount
    arguments:
        limit: 100
connection:
    type: Postgres
    connectionString: ${POSTGRES_STRING}
execution:
    type: query
    arguments:
        sql: 'SELECT * FROM sys.allobjects'
reports:
    type: csv
    arguments:
        output: file.csv
";
file = @"
profile:
    type: limitedConcurrency
    arguments:
        limit: 10";

//var shell = "querystress benchmark.yml";

var @params = Deserialize(file);

Console.WriteLine(@params);

var factory = new LoadProfilesFactory(new[]
{
    new LimitedConcurrencyLoadProfileCreator()
});

var model = factory.CreateProfile(@params);

Console.ReadLine();

Arguments Deserialize(string fileContent)
{
    var deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    return deserializer.Deserialize<Arguments>(fileContent);
}