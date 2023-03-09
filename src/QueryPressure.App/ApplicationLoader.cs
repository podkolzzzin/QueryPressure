using Autofac;
using QueryPressure.Core;

namespace QueryPressure.App;

public class ApplicationLoader
{
  public virtual ContainerBuilder Load(ContainerBuilder builder)
  {
    LoadPlugins(builder);
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
    var dir = new FileInfo(GetType().Assembly.Location).Directory;
    var dlls = dir.GetFiles("*.dll");
    var asms = AppDomain.CurrentDomain.GetAssemblies();
    foreach (var dll in dlls.Where(x => IsSuitable(x.FullName)))
    {
      var defaultContext = System.Runtime.Loader.AssemblyLoadContext.Default; // (!!!) Important
      var loaded = asms.FirstOrDefault(x => x.Location.ToLowerInvariant() == dll.FullName.ToLowerInvariant());
      if (loaded == null)
      {
        loaded = defaultContext.LoadFromAssemblyPath(dll.FullName);
      }
      builder.RegisterAssemblyModules(loaded);
    }
  }
}
