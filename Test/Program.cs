#pragma warning disable CS8321

using static System.Console;
using System.IO;

using FlowPattern;
using FlowPattern.Flows;

ex3();

void ex1()
{
    var flow = DirectoryFlow.Create("/");

    flow.OnFlowing += fsi =>
    {
        if (fsi is FileInfo)
            WriteLine($"File: {fsi.Name}");
        else if (fsi is DirectoryInfo)
            WriteLine($"Directory: {fsi.Name}");
    };

    flow.StartFlow();
}

void ex2()
{
    var flow = DirectoryFlow.Create("/");

    flow
        .If(x => x is FileInfo)
        .OnFlowing += fsi =>
        {
            WriteLine($"File: {fsi.Name}");
        };
    
    flow
        .If(x => x is DirectoryInfo)
        .OnFlowing += fsi =>
        {
            WriteLine($"Directory: {fsi.Name}");
        };
    
    flow.StartFlow();
}

void ex3()
{
    DirectoryFlow.Create("/")
        .If(x => x is FileInfo)
            .Act(x => WriteLine($"File: {x.Name}"))
        .Return
        .If(x => x is DirectoryInfo)
            .Act(x => WriteLine($"Directory: {x.Name}"))
        .Return
    .StartFlow();
}