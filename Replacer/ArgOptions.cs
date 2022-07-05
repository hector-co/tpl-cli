using CommandLine;

namespace Replacer
{
    internal class ArgOptions
    {
        [Option('f', "file", Required = true)]
        public string ValuesFile { get; set; } = string.Empty;

        [Option('o', "output-file")]
        public string OutputFile { get; set; } = string.Empty;

        [Option('e', "output-file-extension", Default = "output")]
        public string OutputFileExtension { get; set; } = "output";

        [Option('t', "template-file", Default = "template.tpl")]
        public string TemplateFile { get; set; } = "template.tpl";

        public void Adjust()
        {
            var processingName = Path.GetFileNameWithoutExtension(ValuesFile);

            if (string.IsNullOrEmpty(OutputFile))
                OutputFile = $@"{processingName}.{OutputFileExtension}";
        }
    }
}
