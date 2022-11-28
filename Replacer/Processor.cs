namespace Replacer;

public class Processor
{
    private readonly string _path;
    private readonly Dictionary<string, string> _mappings;
    private string _outputPath;
    private readonly List<string> _excludedFiles;
    private readonly List<string> _excludedFolders;

    public Processor(string path, Dictionary<string, string> mappings, string outputPath = ".", List<string>? excludedFiles = null,
        List<string>? excludedFolders = null)
    {
        _path = path;
        _mappings = mappings;
        _outputPath = outputPath;
        _excludedFiles = excludedFiles ?? new();
        _excludedFolders = excludedFolders ?? new();

        Process();
    }

    private void Process()
    {
        var folderName = Path.GetFileName(_path);
        var parentFolder = Path.GetDirectoryName(_path);
        var replacedFolderName = Replace(folderName, _mappings);

        if (string.IsNullOrEmpty(_outputPath))
        {
            _outputPath = Path.Combine(parentFolder!, replacedFolderName);
        }

        var files = Directory.EnumerateFiles(_path, "*.*", SearchOption.AllDirectories)
            .Where(f => !_excludedFiles.Contains(Path.GetFileName(f)!, StringComparer.InvariantCultureIgnoreCase))
            .Where(f => !_excludedFolders.Contains(Directory.GetParent(f)!.Name, StringComparer.InvariantCultureIgnoreCase))
            .Where(f => _excludedFolders.All(folder => !f.Contains($"\\{folder}\\")));

        foreach (var file in files)
        {
            var fileContent = File.ReadAllText(file);
            fileContent = Replace(fileContent, _mappings);

            var fileRelPath = Path.GetRelativePath(_path, file);
            fileRelPath = Replace(fileRelPath, _mappings);

            var fileNewPath = Path.Combine(_outputPath, fileRelPath);
            var fileParentFolder = Path.GetDirectoryName(fileNewPath);
            Directory.CreateDirectory(fileParentFolder!);

            File.WriteAllText(fileNewPath, fileContent);
        }
    }

    private static string Replace(string template, Dictionary<string, string> mappings)
    {
        var result = template;
        foreach (var pair in mappings)
        {
            result = result.Replace(pair.Key, pair.Value);
        }
        return result;
    }
}
