using ICSharpCode.AvalonEdit.Highlighting;

namespace QueryPressure.WinUI.Services.Theme;


public interface IThemedHighlightingManager : IHighlightingDefinitionReferenceResolver
{
  public void InitializeThemeDefinitions();

  public IHighlightingDefinition GetThemedDefinition(ApplicationTheme theme, string name);
}

public class ThemedHighlightingManager : IThemedHighlightingManager
{

  private readonly Dictionary<ApplicationTheme, Dictionary<string, IHighlightingDefinition>> _themedDefinitions;

  public ThemedHighlightingManager()
  {
    _themedDefinitions = new Dictionary<ApplicationTheme, Dictionary<string, IHighlightingDefinition>>();
  }

  public void InitializeThemeDefinitions()
  {
    _themedDefinitions.Add(ApplicationTheme.Dark, new Dictionary<string, IHighlightingDefinition>
    {
      {"TSQL", LoadDefinition(@"Themes\Dark\DarkSyntax.xshtd")}
    });
    _themedDefinitions.Add(ApplicationTheme.Light, new Dictionary<string, IHighlightingDefinition>
    {
      {"TSQL", LoadDefinition(@"Themes\Light\LightSyntax.xshtd")}
    });
  }

  private IHighlightingDefinition LoadDefinition(string pathToDefinition)
  {
    throw new NotImplementedException();
  }

  public IHighlightingDefinition GetDefinition(string name)
  {
    throw new NotImplementedException();
  }

  public IHighlightingDefinition GetThemedDefinition(ApplicationTheme theme, string name)
  {
    throw new NotImplementedException();
  }
}
