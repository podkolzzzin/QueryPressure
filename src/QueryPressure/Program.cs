// See https://aka.ms/new-console-template for more information

using QueryPressure.Core;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Limits;
using QueryPressure.Core.LoadProfiles;




Console.WriteLine("Hello, World!");
// var file = @"
// profile:
//     type: limitedConcurrency
//     arguments:
//         limit: 10
// limit:
//     type: queryCount
//     arguments:
//         limit: 100
// connection:
//     type: Postgres
//     connectionString: ${POSTGRES_STRING}
// execution:
//     type: query
//     arguments:
//         sql: 'SELECT * FROM sys.allobjects'
// reports:
//     type: csv
//     arguments:
//         output: file.csv
// ";
var file = @"
profile:
    type: limitedConcurrency
    arguments:
        limit: 10
limit:
    type: queryCount
    arguments:
        limit: 100";

var shell = "querystress benchmark.yml";


var executor = new QueryExecutor(
    new Executable(), 
    new LimitedConcurrencyWithDelayLoadProfile(2, TimeSpan.FromMilliseconds(1_000)),
    new QueryCountLimit(10));

await executor.ExecuteAsync(CancellationToken.None);

class Executable : IExecutable
{
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        return Task.Delay(500, cancellationToken);
    }
}