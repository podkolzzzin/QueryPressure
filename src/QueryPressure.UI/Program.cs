using Autofac;
using Autofac.Extensions.DependencyInjection;
using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;

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

app.MapGet("/providers", (IProviderInfo[] providers) => providers.Select(x => x.Name));
app.MapPost("/connection/test", async (ConnectionRequest request, ICreator<IConnectionProvider>[] creators) =>
{
  var creator = creators.Single(x => x.Type == request.Provider);
  var provider = creator.Create(new ArgumentsSection() {
    Type = request.Provider,
    Arguments = new Dictionary<string, string>() {
      ["connectionString"] = request.ConnectionString // TODO: put constant connectionString somewhere
    }
  });
  return await provider.GetServerInfoAsync(default);
});

app.Run();

public record ConnectionRequest(string ConnectionString, string Provider);

