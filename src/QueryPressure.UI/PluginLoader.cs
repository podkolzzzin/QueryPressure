namespace QueryPressure.UI;

public static class PluginLoader
{
  /// <summary>
  /// Method is used to make real reference to plugins asms to make them loaded into app correctly.
  /// </summary>
  public static WebApplicationBuilder UseBuiltAssemblyPlugins(this WebApplicationBuilder app)
  {
    MentionPluginAutofacModules();
    return app;
  }

  private static Type[] MentionPluginAutofacModules() => new[] {
    typeof(QueryPressure.MongoDB.App.MongoDBAppModule),
    typeof(QueryPressure.Postgres.App.PostgresAppModule),
    typeof(QueryPressure.Redis.App.RedisAppModule),
    typeof(QueryPressure.MySql.App.MySqlAppModule),
    typeof(QueryPressure.SqlServer.App.SqlServerAppModule),
    typeof(QueryPressure.Metrics.App.MetricsModule),
  };
}
