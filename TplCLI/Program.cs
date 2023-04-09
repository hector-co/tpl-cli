using CommandLine;
using TplCLI;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

const string TplDefFile = "tpl-def.yaml";

var parserResult = Parser.Default.ParseArguments<ArgOptions>(args);
if (parserResult.Errors.Any())
{
    return;
}

var deserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .Build();

var parsedArgs = parserResult.Value;

if (string.IsNullOrEmpty(parsedArgs.TemplateFolder))
{   
    var settings = deserializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(
        $"{AppDomain.CurrentDomain.BaseDirectory}appsettings.yaml"));

    parsedArgs.TemplatesParentFolder = settings["templatesFolder"];
}

if (string.IsNullOrEmpty(parsedArgs.TemplateFolder))
{
    parsedArgs.TemplateFolder = SelectTemplate(parsedArgs.TemplatesParentFolder);
}

var definition = deserializer.Deserialize<Definition>(File.ReadAllText($"{parsedArgs.TemplateFolder}/{TplDefFile}"));

var mapping = definition.Keys.ToDictionary(k => k, k => k);
do
{
    mapping = ReadKeyValues(definition.Keys, mapping);

} while (!ConfirmValues(mapping));

Console.Clear();
Console.WriteLine("Processing files...");

_ = new Processor(parsedArgs.TemplateFolder, mapping, parsedArgs.OutputFolder, definition.ExcludedFiles, definition.ExcludedFolders);

Console.WriteLine("Completed.");

static string SelectTemplate(string templatesFolderContainer, bool clear = true)
{
    if (clear) Console.Clear();
    Console.WriteLine($"Select template:");
    var dirs = Directory.EnumerateDirectories(templatesFolderContainer);
    var templateFolders = new List<string>();
    var index = 1;
    foreach (var dir in dirs)
    {
        if (File.Exists(Path.Combine(dir, TplDefFile)))
        {
            templateFolders.Add(dir);
            Console.WriteLine($"  {index}) {Path.GetFileName(dir)}");
            index++;
        }
    }
    Console.Write("> ");
    var selectedInput = Console.ReadLine() ?? string.Empty;
    if (!int.TryParse(selectedInput, out var selected) || selected < 1 || selected > templateFolders.Count)
    {
        Console.Clear();
        Console.WriteLine("Invalid selection.");
        return SelectTemplate(templatesFolderContainer, false);
    }

    return templateFolders[selected - 1];
}

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