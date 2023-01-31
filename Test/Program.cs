#pragma warning disable CS8321

using static System.Console;
using System.IO;

using FlowPattern;
using FlowPattern.Flows;

DirectoryFlow.Create("/")
    .If(x => x is FileInfo)
        .Act(x => WriteLine($"Arquivo: {x.Name}"))
    .Return
    .If(x => x is DirectoryInfo)
        .If(x => x.Name.Length < 8)
            .Act(x => WriteLine($"Pasta com nome pequeno: {x.Name}"))
        .Return
    .Return
.Start();