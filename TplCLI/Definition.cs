namespace TplCLI;

public class Definition
{
    public List<string> Keys { get; set; } = new();
    public List<string> ExcludedFiles { get; set; } = new();
    public List<string> ExcludedFolders { get; set; } = new();
}
