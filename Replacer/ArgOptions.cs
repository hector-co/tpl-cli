using CommandLine;

namespace Replacer
{
    internal class ArgOptions
    {
        [Option('f', "file", Default = "./keys.yaml")]
        public string KeysFile { get; set; } = "./keys.yaml";

        //[Option('e', "output-file-extension", Default = "output")]
        //public string OutputFileExtension { get; set; } = "output";

        [Option('t', "template-folder", Required = true)]
        public string TemplateFolder { get; set; } = string.Empty;

        [Option('o', "output-folder", Default = ".")]
        public string OutputFolder { get; set; } = ".";

        //public void Adjust()
        //{
        //    var processingName = Path.GetFileNameWithoutExtension(KeysFile);

        //    if (string.IsNullOrEmpty(OutputFolder))
        //        OutputFolder = $@"{processingName}.{OutputFileExtension}";
        //}
    }
}
