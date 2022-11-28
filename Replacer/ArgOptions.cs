using CommandLine;

namespace Replacer
{
    internal class ArgOptions
    {
        [Option('f', "file", Default = "./definition.yaml")]
        public string KeysFile { get; set; } = "./definition.yaml";

        [Option('t', "template-folder", Required = true)]
        public string TemplateFolder { get; set; } = string.Empty;

        [Option('o', "output-folder", Default = ".")]
        public string OutputFolder { get; set; } = ".";
    }
}
