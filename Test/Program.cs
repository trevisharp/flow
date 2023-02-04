#pragma warning disable CS8321
#pragma warning disable CS4014

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

using FlowPattern;

joinFlowsDirectory();

void multIfActDirectoryRead()
{
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..")
    .OpenDirectoryFlow()
        .If(x => x is FileInfo)
            .Act(x => WriteLine($"Arquivo: {x.Name}"))
        .Ret
        .If(x => x is DirectoryInfo)
            .If(x => x.Name.Length < 8)
                .Act(x => WriteLine($"Pasta com nome pequeno: {x.Name}"))
            .Ret
            .If(x => x.Name.Length > 8)
                .Act(x => WriteLine($"Pasta com nome grande: {x.Name}"))
            .Ret
        .Ret
    .Start();
}

void asyncParallelFileRead()
{
    run();

    WriteLine("Esperando dados...");
    ReadKey(true);

    async Task run()
    {
        await Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..")
        .OpenTextFileFlow()
            .Act(line => 
            {
                int sum = 0;
                for (int k = 0; k < 50000; k++)
                {
                    int nums = line.Count(x => x >= '0' && x <= '9');
                    int lowLett = line.Count(x => x >= 'a' && x <= 'z');
                    int higLett = line.Count(x => x >= 'A' && x <= 'Z');
                    int others = line.Length - nums - lowLett - higLett;
                    sum += others;
                }
                WriteLine(sum);
            })
        .ParallelStartAsync();
    }
}

void workWithTake()
{
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..")
    .OpenDirectoryFlow()
        .If(x => x is FileInfo)
            .Take(x => x.Name)
                .Take(x => x.Length)
                    .Act(x => WriteLine(x))
                .Ret
                .Take(x => x[0])
                    .Act(x => WriteLine(x))
                .Ret
            .Ret
            .Act(x => WriteLine(x.Extension))
        .Ret
    .Start();
}

void zipSetExample()
{
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..")
    .OpenDirectoryFlow()
        .Zip(x => 0)
        .Zip(x => 0)
        .Set(x => x.i0 is FileInfo ? (x.i0, x.i1, x.i2) : (x.i0, x.i1 + 1, x.i2))
        .Set(x => x.i0 is DirectoryInfo ? (x.i0, x.i1, x.i2) : (x.i0, x.i1, x.i2 + 1))
        .Set(x => x.i0.Name.Length >= 8 ? (x.i0, x.i1, x.i2) : (x.i0, x.i1 + 1, x.i2))
        .Set(x => x.i0.Name.Length < 8? (x.i0, x.i1, x.i2) : (x.i0, x.i1, x.i2 + 1))
        .Act(x => WriteLine(x))
    .Start();
}

void joinFlowsDirectory()
{
    int count = 0;
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..")
    .OpenDirectoryFlow()
        .If(x => x is FileInfo)
            .Join(x => x.FullName.OpenTextFileFlow())
                .Act(x => count++)
    .Start();
    Console.WriteLine(count);
}