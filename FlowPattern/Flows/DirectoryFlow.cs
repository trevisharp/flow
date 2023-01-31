using System.IO;

namespace FlowPattern.Flows;

public class DirectoryFlow : Flow<FileSystemInfo>
{
    private DirectoryFlow(string path)
    {

    }

    public override void StartFlow()
    {
        
    }

    public static DirectoryFlow Create(string path)
        => new DirectoryFlow(path);
}