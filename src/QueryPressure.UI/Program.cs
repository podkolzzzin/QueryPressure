using Autofac;
using Autofac.Extensions.DependencyInjection;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.UI;
using QueryPressure.UI.Extensions;
using QueryPressure.UI.Hubs;
using QueryPressure.UI.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
  .ConfigureContainer<ContainerBuilder>(diBuilder => new ApiApplicationLoader().Load(diBuilder));

builder.UseBuiltAssemblyPlugins();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseDeveloperExceptionPage();
app.UseFrontendStaticFiles();
app.OpenBrowserWhenReady();

app.MapHub<DashboardHub>("/ws/dashboard");

app.MapGet("/api/providers", (IProviderInfo[] providers) => providers);

app.MapPost("/api/connection/test", async (ConnectionRequest request, ProviderManager manager) =>
  await manager.GetProvider(request.Provider).TestConnectionAsync(request.ConnectionString));

app.MapGet("/api/profiles", GetCreatorMetadata<IProfileCreator, IProfile>);

app.MapGet("/api/limits", GetCreatorMetadata<ILimitCreator, ILimit>);

app.MapPost("/api/execution", (ExecutionRequest request, ProviderManager manager, CancellationToken cancellationToken) =>
  manager.GetProvider(request.Provider).StartExecutionAsync(request, cancellationToken));

app.MapPost("/api/execution/{executionId:guid}/cancel", (Guid executionId, ProviderManager manager) => manager.CancelExecution(executionId));

app.MapGet("/api/resources/{locale}", (IResourceManager manager, string locale) =>
  manager.GetResources(locale, ResourceFormat.Html));

app.MapGet("/api/locales", () => new[] { "en-US", "uk-UA" });

app.Run();

static IEnumerable<CreatorMetadataResponse> GetCreatorMetadata<TCreator, TCreated>(IEnumerable<TCreator> creators)
  where TCreator : IArgumentProvider, ICreator<TCreated>
  => creators.Select(x => new CreatorMetadataResponse(x.Arguments, x.Type));
