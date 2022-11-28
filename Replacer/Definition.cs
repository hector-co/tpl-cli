namespace Replacer;

public class Definition
{
    public List<string> Keys { get; set; } = new();
    public Dictionary<string, string> Mapping { get; set; } = new();
    public List<string> ExcludedFiles { get; set; } = new();
    public List<string> ExcludedFolders { get; set; } = new();
}
