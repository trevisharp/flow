using static System.Console;
using System.IO;

using FlowPattern;

DirectoryFlow.Create("/")
    .If(x => x is FileInfo)
        .Act(x => WriteLine($"Arquivo: {x.Name}"))
    .Ret
    .If(x => x is DirectoryInfo)
        .If(x => x.Name.Length < 8)
            .Act(x => WriteLine($"Pasta com nome pequeno: {x.Name}"))
        .Ret
    .Ret
.Start();