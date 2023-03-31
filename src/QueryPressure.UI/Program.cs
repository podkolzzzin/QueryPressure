using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.UI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
  .ConfigureContainer<ContainerBuilder>(diBuilder => new ApiApplicationLoader().Load(diBuilder));

builder.UseBuiltAssemblyPlugins();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseFrontendStaticFiles();

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

