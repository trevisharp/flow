namespace FlowPattern;

using Flows;

public static class FlowExtension
{
    public static DirectoryFlow OpenDirectoryFlow(this string path)
        => DirectoryFlow.Create(path); 
        
    public static TextFileFlow OpenTextFileFlow(this string path)
        => TextFileFlow.Create(path); 
}