using CommandLine;

namespace Replacer
{
    internal class ArgOptions
    {
        [Option('p', "templates-parent-folder", Default = "")]
        public string TemplatesParentFolder { get; set; } = string.Empty;

        [Option('f', "template-folder", Default = "")]
        public string TemplateFolder { get; set; } = string.Empty;

        [Option('o', "output-folder", Default = ".")]
        public string OutputFolder { get; set; } = ".";
    }
}
