namespace QueryPressure.WinUI.Services.Language;

public readonly record struct LanguageItem(string Locale, IDictionary<string, string> Strings);
