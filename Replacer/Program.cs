using CommandLine;
using Replacer;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var parserResult = Parser.Default.ParseArguments<ArgOptions>(args);
if (parserResult.Errors.Any())
    return;

var parsedArgs = parserResult.Value;

var deserializer = new DeserializerBuilder()
    .WithNamingConvention(UnderscoredNamingConvention.Instance)
    .Build();

var definition = deserializer.Deserialize<Definition>(File.ReadAllText(parsedArgs.KeysFile));

_ = new Processor(parsedArgs.TemplateFolder, definition.Mapping, parsedArgs.OutputFolder);
