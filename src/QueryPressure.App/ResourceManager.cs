using QueryPressure.App.Interfaces;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace QueryPressure.App;

public class AmbiguousResourceException : Exception
{
  private const string Template = "Resource {0}/{1} is ambiguous to Resource {2}/{3}.";

  public string Resource1 { get; }
  public string File1 { get; }
  public string Resource2 { get; }
  public string File2 { get; }
  public AmbiguousResourceException(string File1, string Resource1, string File2, string Resource2)
    : base(string.Format(Template, File1, Resource1, File2, Resource2))
  {
    this.Resource1 = Resource1;
    this.Resource2 = Resource2;
    this.File1 = File1;
    this.File2 = File2;
  }
}

public class ResourceManager : IResourceManager
{
  private class Resource
  {
    public string ResourceName { get; }
    public string FileName { get; }

    private readonly Dictionary<ResourceFormat, string> _resources;
    public Resource(string resource, string resourceName, string fileName)
      : this(new Dictionary<ResourceFormat, string> { [ResourceFormat.Plain] = resource }, resourceName, fileName)
    {
    }

    public Resource(Dictionary<ResourceFormat, string> resources, string resourceName, string fileName)
    {
      _resources = resources;
      ResourceName = resourceName;
    }

    public string GetValue(ResourceFormat format)
    {
      if (_resources.TryGetValue(format, out var result))
        return result;
      return _resources[ResourceFormat.Plain];
    }
  }

  private readonly Dictionary<string, Resource> _resources;

  public ResourceManager(IResourceDiscovery discovery)
  {
    _resources = GetAllResources(discovery);
  }

  public IDictionary<string, string> GetResources(string locale, ResourceFormat format)
  {
    return _resources.Where(x => x.Key.StartsWith(locale))
      .ToDictionary(x => x.Key.Substring(locale.Length + 1), x => x.Value.GetValue(format));
  }

  private static Dictionary<string, Resource> GetAllResources(IResourceDiscovery discovery)
  {
    var deserializer = new DeserializerBuilder()
      .WithNamingConvention(CamelCaseNamingConvention.Instance)
      .Build();

    var allResources = discovery.GetAllResources();
    var resources = allResources.SelectMany(x => Load(deserializer, x))
      .ToArray();

    return Merge(resources);
  }
  private static Dictionary<string, Resource> Merge(Resource[] resources)
  {
    Dictionary<string, Resource> result = new();
    foreach (var item in resources)
    {
      if (result.TryGetValue(item.ResourceName, out var resource))
      {
        throw new AmbiguousResourceException(item.FileName, item.ResourceName, resource.FileName, resource.ResourceName);
      }
      result[item.ResourceName] = item;
    }
    return result;
  }

  private static List<Resource> Load(IDeserializer deserializer, ResourceFile file)
  {
    using (file.Content)
    {
      using var reader = new StreamReader(file.Content);
      var dictionary = deserializer.Deserialize<Dictionary<object, object?>>(reader);
      return ConvertToResources(new List<Resource>(), dictionary, file.FileName, string.Empty);
    }
  }
  private static List<Resource> ConvertToResources(List<Resource> resources, Dictionary<object, object?> dictionary, string fileName, string resourceName)
  {
    foreach (var (key, value) in dictionary)
    {
      if (value is null)
        continue;

      var name = resourceName is "" ? key.ToString()! : resourceName + "." + key;
      var resource = GetResource(value, name, fileName);
      if (resource != null)
      {
        resources.Add(resource);
      }
      else
      {
        ConvertToResources(resources, (Dictionary<Object, object>)value, fileName, name);
      }
    }
    return resources;
  }

  private static Resource? GetResource(object source, string resourceName, string fileName)
  {
    if (source is string str)
      return new Resource(str, resourceName, fileName);

    var sourceDic = (Dictionary<object, object>)source;
    if (sourceDic.Count > Enum.GetValues(typeof(ResourceFormat)).Length)
      return null;

    var keys = Enum.GetNames(typeof(ResourceFormat))
      .Select(x => x.ToLower())
      .ToArray();

    if (sourceDic.Keys.Cast<string>().Any(x => !keys.Contains(x.ToLowerInvariant())))
      return null;

    return new Resource(sourceDic.ToDictionary(x => Enum.Parse<ResourceFormat>(x.Key.ToString()!, true), x => x.Value.ToString())!, resourceName, fileName);
  }
}
