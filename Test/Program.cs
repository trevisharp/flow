﻿#pragma warning disable CS8321
#pragma warning disable CS4014

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

using FlowPattern;

workWithFiles();

void workWithFiles()
{
    "/".OpenDirectoryFlow()
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

void multIfActDirectoryRead()
{
    "/".OpenDirectoryFlow()
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
        await @"C:\Users\SII5CT\Desktop\flow-main\FlowPattern\Flows\TextFileFlow.cs"
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
