using CommandLine;
using Replacer;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var parserResult = Parser.Default.ParseArguments<ArgOptions>(args);
if (parserResult.Errors.Any())
{
    Console.ReadKey();
    return;
}

var parsedArgs = parserResult.Value;

var deserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .Build();

var definition = deserializer.Deserialize<Definition>(File.ReadAllText(parsedArgs.KeysFile));

var mapping = definition.Keys.ToDictionary(k => k, k => k);
do
{
    mapping = ReadKeyValues(definition.Keys, mapping);

} while (!ConfirmValues(mapping));

Console.Clear();
Console.WriteLine("Processing files...");

_ = new Processor(parsedArgs.TemplateFolder, mapping, parsedArgs.OutputFolder, definition.ExcludedFiles, definition.ExcludedFolders);

Console.WriteLine("Completed.");
static Dictionary<string, string> ReadKeyValues(List<string> keys, Dictionary<string, string> defaultValues)
{
    Console.Clear();
    var mapping = new Dictionary<string, string>();
    foreach (var key in keys)
    {
        Console.Write($"Introduce value for key '{key}'");
        if (defaultValues.ContainsKey(key))
            Console.Write($" (empty for default '{defaultValues[key]}')");
        Console.Write(": ");
        Console.WriteLine();
        Console.Write("> ");
        var keyValue = Console.ReadLine() ?? string.Empty;
        if (keyValue == string.Empty)
            keyValue = defaultValues[key];
        mapping.TryAdd(key, keyValue);
    }
    return mapping;
}

static bool ConfirmValues(Dictionary<string, string> selectedValues)
{
    Console.Clear();
    Console.WriteLine("Confirm following values:");
    Console.WriteLine();
    foreach (var value in selectedValues)
    {
        Console.WriteLine($"> {value.Key} : {value.Value}");
    }
    Console.WriteLine();
    Console.WriteLine("[C]ontinue (default) - [R]eset values");
    var response = Console.ReadLine() ?? string.Empty;
    response = response.Trim();

    if (response != string.Empty && response.Trim().ToUpper() != "C" && response.Trim().ToUpper() != "CONTINUE")
        return false;

    return true;
}