using Autofac;
using Autofac.Extensions.DependencyInjection;
using QueryPressure.App.Arguments;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapGet("/api/providers", (IProviderInfo[] providers) => providers.Select(x => x.Name));

app.MapPost("/api/connection/test", async (ConnectionRequest request, ProviderManager manager) =>
  await manager.GetProvider(request.Provider).TestConnectionAsync(request.ConnectionString));

app.MapGet("/api/profiles", (IProfileCreator[] creators) => creators.Select(x => new
{
  x.Arguments,
  x.Type
}));

app.MapGet("/api/limits", (ILimitCreator[] creators) => creators.Select(x => new
{
  x.Arguments,
  x.Type
}));

app.MapPost("/api/execution", (ExecutionRequest request, ProviderManager manager) =>
  manager.GetProvider(request.Provider).StartExecutionAsync(request));

app.Run();

public record ConnectionRequest(string ConnectionString, string Provider);

public record ExecutionRequest(string ConnectionString, string Provider, string Script, FlatArgumentsSection Profile, FlatArgumentsSection Limit);
