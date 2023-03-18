using System.IO;
using System.Linq;
using System.Text;
using NSubstitute;
using QueryPressure.App;
using QueryPressure.App.Interfaces;
using Xunit;

namespace QueryPressure.Tests;

public class ResourceManagerTests
{
  [Fact]
  public void GetResources_SimpleSingleResource_Success()
  {
    var resourceManager = CreateManager("""
en-US:
  simple: String 1
""");
    var result = resourceManager.GetResources("en-US", ResourceFormat.Plain);
    Assert.Collection(result, x => Assert.Equal(("simple", "String 1"), (x.Key, x.Value)));
  }

  [Fact]
  public void GetResources_SimpleSingleResource_PlainIsReturnedIfHtmlNotProvided()
  {
    var resourceManager = CreateManager("""
en-US:
  simple: String 1
""");
    var result = resourceManager.GetResources("en-US", ResourceFormat.Html);
    Assert.Collection(result, x => Assert.Equal(("simple", "String 1"), (x.Key, x.Value)));
  }

  [Fact]
  public void GetResources_MultiFormatResource_HtmlIsReturnedIfProvided()
  {
    var resourceManager = CreateManager("""
en-US:
  simple: 
    plain: String 1
    html: <b>String</b> 1
""");
    var result = resourceManager.GetResources("en-US", ResourceFormat.Html);
    Assert.Collection(result, x => Assert.Equal(("simple", "<b>String</b> 1"), (x.Key, x.Value)));
  }

  [Fact]
  public void GetResources_ComplexValue_IsRepresentedAsPlain()
  {
    var resourceManager = CreateManager("""
en-US:
  root: Root Value
  parent: 
    child1: String 1
    child2:
      nestedChild: nestedValue
""");
    var result = resourceManager.GetResources("en-US", ResourceFormat.Plain);
    Assert.Collection(result,
      x => Assert.Equal(("root", "Root Value"), (x.Key, x.Value)),
      x => Assert.Equal(("parent.child1", "String 1"), (x.Key, x.Value)),
      x => Assert.Equal(("parent.child2.nestedChild", "nestedValue"), (x.Key, x.Value))
    );
  }

  [Fact]
  public void GetResources_TwoResourceFiles_MergedSuccessfully()
  {
    var resourceManager = CreateManager("""
en-US:
  simple: String 1
""",
"""
en-US:
  another: Value 2
""");
    var result = resourceManager.GetResources("en-US", ResourceFormat.Plain);
    Assert.Collection(result,
      x => Assert.Equal(("simple", "String 1"), (x.Key, x.Value)),
      x => Assert.Equal(("another", "Value 2"), (x.Key, x.Value))
    );
  }

  [Fact]
  public void Ctor_HasConflict_ThrowsAmbiguousException()
  {
    Assert.Throws<AmbiguousResourceException>(() =>
    {
      CreateManager("""
en-US:
  key1: String 1
""",
        """
en-US:
  key1: Value 2
""");
    });
  }

  private IResourceManager CreateManager(params string[] resources)
  {
    return new ResourceManager(CreateResources(resources));
  }

  private IResourceDiscovery CreateResources(params string[] resources)
  {
    var result = Substitute.For<IResourceDiscovery>();
    result.GetAllResources().Returns(
      resources.Select((x, i) => new ResourceFile($"Resource {i}", new MemoryStream(Encoding.Default.GetBytes(x)))));
    return result;
  }
}
