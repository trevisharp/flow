using System;
using System.IO;

namespace FlowPattern.Flows;

public class DirectoryFlow : Flow<FileSystemInfo>
{
    private DirectoryInfo dir;
    private DirectoryFlow(string path)
    {
        if (!Directory.Exists(path))
            throw new Exception(
                $"O valor de 'path'({path}) deve ser o caminho de um diretório"
            );
        
        this.dir = new DirectoryInfo(path);
    }

    public override void StartFlow()
    {
        foreach (var fileSysInfo in dir.GetFileSystemInfos())
            onFlowing(fileSysInfo);
    }

    public static DirectoryFlow Create(string path)
        => new DirectoryFlow(path);
}