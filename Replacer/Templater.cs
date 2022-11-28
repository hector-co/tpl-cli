namespace Replacer;

public class Templater
{
    private readonly Dictionary<string, (string KeyGuid, string ReplaceValue)> _keyValues;

    public IReadOnlyDictionary<string, string> KeyValues
        => _keyValues.ToDictionary(kv => kv.Key, kv => kv.Value.KeyGuid);

    public Templater(IEnumerable<string> keys)
    {
        _keyValues = new Dictionary<string, (string, string)>();
        foreach (var key in keys)
        {
            var keyGuid = Guid.NewGuid().ToString();
            _keyValues.Add(key, (keyGuid, $"${{{keyGuid}}}"));
        }
    }

    public string Apply(string content)
    {
        var template = content;
        foreach (var tuple in _keyValues)
        {
            template = template.Replace(tuple.Key, tuple.Value.ReplaceValue);
        }
        return template;
    }
}
