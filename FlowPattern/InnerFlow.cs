/* Author:  Leonardo Trevisan Silio
 * Date:    03/04/2023
 */
using System.Threading.Tasks;

namespace FlowPattern;

using Exceptions;

/// <summary>
/// A Inner Flow, a intermediary flow that can transform and operate with data.
/// </summary>
/// <typeparam name="T">Type of input data.</typeparam>
/// <typeparam name="R">Type of output data.</typeparam>
/// <typeparam name="P">The type of parent flow.</typeparam>
public abstract class InnerFlow<T, R, P> : ParentFlow<R, P>
    where P : IFlow
{
    protected InnerFlow(P main)
    {
        main.Attach(x =>
        {
            if (x is T data)
                onMainFlowing(data);
            else throw InvalidDataFlowException.Default;
        });
        this.Ret = main;
    }

    /// <summary>
    /// This function will be called every time that the parent flow flowing data.
    /// </summary>
    /// <param name="x">Data that flowed.</param>
    protected abstract void onMainFlowing(T x);

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