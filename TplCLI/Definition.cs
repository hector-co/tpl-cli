namespace TplCLI;

public class Definition
{
    public Dictionary<string, string> Keys { get; set; } = new();
    public List<string> ExcludedFiles { get; set; } = new();
    public List<string> ExcludedFolders { get; set; } = new();
    public string OutputFolder { get; set; } = string.Empty;

    public string EvalOutputFolder(Dictionary<string, string> mappings)
    {
        if (OutputFolder.StartsWith("$"))
            return mappings[OutputFolder[1..]];

        return OutputFolder;
    }
}
