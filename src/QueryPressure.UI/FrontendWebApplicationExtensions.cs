using Microsoft.Extensions.FileProviders;

namespace QueryPressure.UI;

public static class FrontendWebApplicationExtensions
{
  public static void UseFrontendStaticFiles(this WebApplication app)
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

  public static void OpenBrowserWhenReady(this WebApplication app)
  {
    app.Lifetime.ApplicationStarted.Register(() =>
    {
      var _ = app.Services.GetRequiredService<Launcher>().Start(app.Lifetime.ApplicationStopped);
    });
  }
}
