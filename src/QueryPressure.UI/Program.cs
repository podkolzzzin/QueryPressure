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

var api = app.MapGroup("/api");

api.MapGet("/providers", (IProviderInfo[] providers) => providers);

api.MapPost("/connection/test", async (ConnectionRequest request, ProviderManager manager) =>
  await manager.GetProvider(request.Provider).TestConnectionAsync(request.ConnectionString));

api.MapGet("/profiles", GetCreatorMetadata<IProfileCreator, IProfile>);

api.MapGet("/limits", GetCreatorMetadata<ILimitCreator, ILimit>);

api.MapPost("/execution", (ExecutionRequest request, ProviderManager manager, CancellationToken cancellationToken) =>
  manager.GetProvider(request.Provider).StartExecutionAsync(request, cancellationToken));

api.MapGet("/execution", (ProviderManager manager, CancellationToken cancellationToken) => manager.GetExecutions());

api.MapPost("/execution/{executionId:guid}/cancel", (Guid executionId, ProviderManager manager) => manager.CancelExecution(executionId));

api.MapGet("/resources/{locale}", (IResourceManager manager, string locale) =>
  manager.GetResources(locale, ResourceFormat.Html));

api.MapGet("/locales", () => new[] { "en-US", "uk-UA" });

app.Run();

static IEnumerable<CreatorMetadataResponse> GetCreatorMetadata<TCreator, TCreated>(IEnumerable<TCreator> creators)
  where TCreator : IArgumentProvider, ICreator<TCreated>
  => creators.Select(x => new CreatorMetadataResponse(x.Arguments, x.Type));
