using Microsoft.Extensions.FileProviders;

namespace QueryPressure.UI;

public static class FrontendWebApplicationExtensions
{
  public static void UseFrontendStaticFiles(this WebApplication app)
  {
    try
    {
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
    }
    catch (InvalidOperationException)
    {
      if (app.Environment.EnvironmentName == "Development")
      {
        app.Logger.LogWarning("""
There is no folder dist in the project. Run 'npm run build' in frontend folder and copy the results to it.
You can also use start configuration 'Start API + Front' to start developer instance of frontend.
For the release build everything would be copied automatically by CI build script.
""");
      }
      else throw;
    }
  }

  public static void OpenBrowserWhenReady(this WebApplication app)
  {
    app.Lifetime.ApplicationStarted.Register(() =>
    {
      var _ = app.Services.GetRequiredService<Launcher>().Start(app.Lifetime.ApplicationStopped);
    });
  }
}
