using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

if (args.Length == 0)
    return;

var basePath = Directory.GetParent(args[0]);

var processingName = Path.GetFileNameWithoutExtension(args[0]);

var valuesFile = processingName + ".yaml";

var templateFile = "template.tpl";
if (args.Length > 1)
    templateFile = args[1];

var outputFile = $"{basePath}\\{processingName}.output";

var deserializer = new DeserializerBuilder()
    .WithNamingConvention(UnderscoredNamingConvention.Instance)
    .Build();

var values = deserializer.Deserialize<Dictionary<string, string>>(File.ReadAllText($"{basePath}\\{valuesFile}"));

var result = ReplaceInTemplate(File.ReadAllText($"{basePath}\\{templateFile}"), values);

File.WriteAllText(outputFile, result);

string ReplaceInTemplate(string template, Dictionary<string, string> mappings)
{
    var result = template;
    foreach (var pair in mappings)
    {
        var key = $"${{{pair.Key}}}";
        result = result.Replace(key, pair.Value);
    }
    return result;
}