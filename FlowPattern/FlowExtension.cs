namespace FlowPattern;

using Flows;

public static class FlowExtension
{
    public static DirectoryFlow OpenDirectoryFlow(this string path)
        => DirectoryFlow.Create(path); 
}