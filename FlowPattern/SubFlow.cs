using System;
using System.Threading.Tasks;
using FlowPattern.Exceptions;

namespace FlowPattern;

public class SubFlow<T, S1, R, S2> : Flow<(T i0, R i1), SubFlow<T, S1, R, S2>>
    where S1 : Flow<T, S1>
    where S2 : Flow<R, S2>
{
    private S2 subFlow;
    public S1 Ret { get; private set; }

    public SubFlow(Flow<T, S1> main, Func<T, Flow<R, S2>> creator)
    {
        Ret = (S1)main;
        main.Attach((T x) =>
        {
            var newFlow = creator(x);
            newFlow.Attach((R y) => Flowing((i0: x, i1: y)));
            newFlow.Start();
        });
    }

    public override void Start()
    {
        if (Ret is IFlow flow)
            flow.Start();
        else throw InvalidFlowParentException.Default;
    }

    public override async Task StartAsync()
    {
        if (Ret is IFlow flow)
            await flow.StartAsync();
        else throw InvalidFlowParentException.Default;
    }

    public override void ParallelStart()
    {
        if (Ret is IFlow flow)
            flow.ParallelStart();
        else throw InvalidFlowParentException.Default;
    }

    public override async Task ParallelStartAsync()
    {
        if (Ret is IFlow flow)
            await flow.ParallelStartAsync();
        else throw InvalidFlowParentException.Default;
    }
}