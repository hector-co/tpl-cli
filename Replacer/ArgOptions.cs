using CommandLine;

namespace Replacer
{
    internal class ArgOptions
    {
        [Option('f', "file", Default = "./tpl-def.yaml")]
        public string KeysFile { get; set; } = "./tpl-def.yaml";

        [Option('o', "output-folder", Default = ".")]
        public string OutputFolder { get; set; } = ".";
    }
}
