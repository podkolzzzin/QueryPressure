namespace QueryPressure.App.Interfaces;

public enum ResourceFormat { Plain, Html }

public interface IResourceManager
{
  IDictionary<string, string> GetResources(string locale, ResourceFormat format);
}

public record ResourceFile(string FileName, Stream Content);

public interface IResourceDiscovery
{
  IEnumerable<ResourceFile> GetAllResources();
}

