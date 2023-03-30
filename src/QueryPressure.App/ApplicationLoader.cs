using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using Autofac;
using QueryPressure.Core;

namespace QueryPressure.App;

public class ApplicationLoader
{
  public virtual ContainerBuilder Load(ContainerBuilder builder)
  {
    LoadPlugins(builder);
    builder.RegisterModule<AppModule>();
    return builder;
  }

  private bool IsSuitable(string path)
  {
    try
    {
      var type = typeof(QueryPressurePluginAttribute);
      var asm = Mono.Cecil.AssemblyDefinition.ReadAssembly(path); // (!!!) Important
      return asm
        .CustomAttributes
        .Any(attribute => attribute.AttributeType.Name == type.Name && attribute.AttributeType.Namespace == type.Namespace);
    }
    catch
    {
      return false;
    }
  }

  private void LoadPlugins(ContainerBuilder builder)
  {
    var dir = new FileInfo(AppContext.BaseDirectory).Directory;
    var dlls = dir.GetFiles("*.dll");
    var asms = AppDomain.CurrentDomain.GetAssemblies();
    foreach (var dll in dlls.Where(x => IsSuitable(x.FullName)))
    {
      var defaultContext = System.Runtime.Loader.AssemblyLoadContext.Default; // (!!!) Important
      var loaded = asms.FirstOrDefault(x => x.Location.ToLowerInvariant() == dll.FullName.ToLowerInvariant());
      if (loaded == null)
      {
        defaultContext.LoadFromAssemblyPath(dll.FullName);
      }
    }

    asms = AppDomain.CurrentDomain.GetAssemblies();
    foreach (var asm in asms.Where(x => x.GetCustomAttribute<QueryPressurePluginAttribute>() != null))
    {
      Console.WriteLine(asm.FullName);
      builder.RegisterAssemblyModules(asm);
    }
  }
}
