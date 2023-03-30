using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.UI;

// dotnet publish .\QueryPressure.UI.csproj
//        -c Release
//        -o .out
//        -p:PublishSingleFile=true
//        -p:PublishTrimmed=true
//        -p:PublishReadyToRun=true
//        -p:PublishTrimmed=true
//        --self-contained true

Console.WriteLine("1");
var types = new[] {
  typeof(QueryPressure.MongoDB.App.MongoDBAppModule),
  typeof(QueryPressure.Postgres.App.PostgresAppModule),
  typeof(QueryPressure.Redis.App.RedisAppModule),
  typeof(QueryPressure.MySql.App.MySqlAppModule),
  typeof(QueryPressure.SqlServer.App.SqlServerAppModule),
  typeof(QueryPressure.Metrics.App.MetricsModule),
};
Console.WriteLine("========");
foreach (var asm in typeof(Program).Assembly.GetReferencedAssemblies().Where(x => x.Name.Contains("QueryPressure")))
{
  Console.WriteLine(asm.Name);
}

Console.WriteLine("========");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
  .ConfigureContainer<ContainerBuilder>(diBuilder => new ApiApplicationLoader().Load(diBuilder));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseFileServer(new FileServerOptions()
{
  RequestPath = "/ui",
  FileProvider = new ManifestEmbeddedFileProvider(typeof(Program).Assembly, "dist/"),
  EnableDefaultFiles = true,
});

app.UseFileServer(new FileServerOptions()
{
  RequestPath = "/img",
  FileProvider = new ManifestEmbeddedFileProvider(typeof(Program).Assembly, "dist/img/"),
  EnableDefaultFiles = true,
});

app.UseFileServer(new FileServerOptions()
{
  RequestPath = "/assets",
  FileProvider = new ManifestEmbeddedFileProvider(typeof(Program).Assembly, "dist/assets/"),
  EnableDefaultFiles = true,
});

app.MapGet("/api/providers", (IProviderInfo[] providers) => providers.Select(x => x.Name));

app.MapPost("/api/connection/test", async (ConnectionRequest request, ProviderManager manager) =>
  await manager.GetProvider(request.Provider).TestConnectionAsync(request.ConnectionString));

app.MapGet("/api/profiles", GetCreatorMetadata<IProfileCreator, IProfile>);

app.MapGet("/api/limits", GetCreatorMetadata<ILimitCreator, ILimit>);

app.MapPost("/api/execution", (ExecutionRequest request, ProviderManager manager) =>
  manager.GetProvider(request.Provider).StartExecutionAsync(request));

app.MapGet("/api/resources/{locale}", (IResourceManager manager, string locale) =>
  manager.GetResources(locale, ResourceFormat.Html));

app.MapGet("/api/locales", () => new[] { "en-US", "uk-UA" });

app.Lifetime.ApplicationStarted.Register(() =>
{
  var _ = app.Services.GetRequiredService<Launcher>().Start(app.Lifetime.ApplicationStopped);
});
app.Run();

static IEnumerable<CreatorMetadataResponse> GetCreatorMetadata<TCreator, TCreated>(IEnumerable<TCreator> creators)
  where TCreator : IArgumentProvider, ICreator<TCreated>
  => creators.Select(x => new CreatorMetadataResponse(x.Arguments, x.Type));

