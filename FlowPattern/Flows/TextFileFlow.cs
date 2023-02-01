using System;
using System.IO;
using System.Threading.Tasks;

namespace FlowPattern.Flows;

public class TextFileFlow : Flow<string, TextFileFlow>
{
    private FileInfo file;
    private TextFileFlow(string path)
    {
        if (!File.Exists(path))
            throw new Exception(
                $"O valor de 'path'({path}) deve ser o caminho de um arquivo"
            );
        
        this.file = new FileInfo(path);
    }

    public override void Start()
    {
        using var stream = file.Open(FileMode.Open);
        using var reader = new StreamReader(stream);
        
        while (!reader.EndOfStream)
            Flowing(reader.ReadLine());

        reader.Close();
        stream.Close();
    }

    public override void ParallelStart()
    {
        using var stream = file.Open(FileMode.Open);
        using var reader = new StreamReader(stream);
        
        var coreCount = Environment.ProcessorCount;
        bool end = false;

        Parallel.For(0, coreCount, k =>
        {
            while (!end)
            {
                string line = string.Empty;
                lock(reader)
                {
                    if (!reader.EndOfStream)
                        line = reader.ReadLine();
                    else
                    {
                        end = true;
                        break;
                    }
                }
                Flowing(line);
            }
        });

        reader.Close();
        stream.Close();
    }

    public override async Task StartAsync()
    {
        using var stream = file.Open(FileMode.Open);
        using var reader = new StreamReader(stream);
        
        while (!reader.EndOfStream)
            Flowing(await reader.ReadLineAsync());

        reader.Close();
        stream.Close();
    }

    public static TextFileFlow Create(string path)
        => new TextFileFlow(path);
}