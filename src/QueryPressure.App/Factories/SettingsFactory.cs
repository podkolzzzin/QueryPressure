using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.App.Factories;

public class SettingsFactory<T> : ISettingsFactory<T> where T : ISetting
{
    private readonly string _settingType;
    private readonly IDictionary<string, ICreator<T>> _creators;

    public SettingsFactory(string settingType, IEnumerable<ICreator<T>> creators)
    {
        _settingType = settingType;
        _creators = creators.ToDictionary(x => x.Type);
    }
    
    public T Create(ApplicationArguments arguments)
    {
        if (!arguments.TryGetValue(_settingType, out var section))
        {
            throw new ApplicationException($"No section {_settingType} was found.");
        }
        
        if (!_creators.TryGetValue(section.Type, out var creator))
            throw new ApplicationException($"No setting {typeof(T).Name} with the name of {section.Type}");

        return creator.Create(section);
    }
}