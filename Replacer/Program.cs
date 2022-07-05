using CommandLine;
using Replacer;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var parserResult = Parser.Default.ParseArguments<ArgOptions>(args);
if (parserResult.Errors.Any())
    return;

var parsedArgs = parserResult.Value;
parsedArgs.Adjust();

var deserializer = new DeserializerBuilder()
    .WithNamingConvention(UnderscoredNamingConvention.Instance)
    .Build();

var values = deserializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(parsedArgs.ValuesFile));

var result = ReplaceInTemplate(File.ReadAllText(parsedArgs.TemplateFile), values);

File.WriteAllText(parsedArgs.OutputFile, result);

static string ReplaceInTemplate(string template, Dictionary<string, string> mappings)
{
    var result = template;
    foreach (var pair in mappings)
    {
        var key = $"${{{pair.Key}}}";
        result = result.Replace(key, pair.Value);
    }
    return result;
}