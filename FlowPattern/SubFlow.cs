/* Author:  Leonardo Trevisan Silio
 * Date:    03/04/2023
 */
using System;
using System.Threading.Tasks;
using FlowPattern.Exceptions;

namespace FlowPattern;

/// <summary>
/// A Flow that run with other main flow.
/// </summary>
/// <typeparam name="T">Input data of the main flow.</typeparam>
/// <typeparam name="R">Input data of the sub flow.</typeparam>
/// <typeparam name="P">Type of parent main flow.</typeparam>
public class SubFlow<T, R, P> : ParentFlow<(T i0, R i1), P>
    where P : IFlow
{
    public SubFlow(P main, Func<T, Flow<R>> creator)
    {
        Ret = main;
        (main as Flow<T>)?.Attach((T x) =>
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