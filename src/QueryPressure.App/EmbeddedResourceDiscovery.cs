using QueryPressure.App.Interfaces;

namespace QueryPressure.App;

public class EmbeddedResourceDiscovery : IResourceDiscovery
{
  private const string ResourceName = "Resources.yml";

  public IEnumerable<ResourceFile> GetAllResources()
  {
    var allResources = AppDomain.CurrentDomain.GetAssemblies()
      .Where(x => x.FullName.Contains(nameof(QueryPressure)))
      .SelectMany(Asm => Asm.GetManifestResourceNames()
        .Select(Resource => new { Asm, Resource }));

    return allResources.Where(x => x.Resource.EndsWith(ResourceName))
      .Select(x => new ResourceFile(x.Resource, x.Asm.GetManifestResourceStream(x.Resource)!));
  }
}
