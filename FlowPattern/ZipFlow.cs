/* Author:  Leonardo Trevisan Silio
 * Date:    03/04/2023
 */
using FlowPattern.Exceptions;
using System.Collections.Generic;

namespace FlowPattern;

/// <summary>
/// A flow to Zip/Join two flows in a unique flow.
/// </summary>
/// <typeparam name="T">Input data of first flow.</typeparam>
/// <typeparam name="R">Input data of second flow.</typeparam>
/// <typeparam name="P">Type of parent flow.</typeparam>
/// <typeparam name="F">Type of first flow.</typeparam>
/// <typeparam name="G">Type of second flow.</typeparam>
public class ZipFlow<T, R, P> : ParentFlow<(T, R), P>
    where P : IFlow
{
    private Queue<T> primaryQueue = null;
    private Queue<R> secundaryQueue = null;
    private bool hasT = false;
    private T lastT;
    private bool hasR = false;
    private R lastR;
    private Flow<T> primary;
    private Flow<R> secondary;
    private bool latest;
    private bool repeat;

    public ZipFlow(Flow<T> primary, Flow<R> secondary, bool latest = false, bool repeat = false)
    {
        this.Ret = (P)(IFlow)primary;
        this.primary = primary;
        this.secondary = secondary;
        this.latest = latest;
        this.repeat = repeat;

        if (this.latest)
        {
            primaryQueue = new Queue<T>();
            secundaryQueue = new Queue<R>();

            primary.Attach((T x) =>
            {
                primaryQueue.Enqueue(x);
                tryFlowing();
            });
            
            secondary.Attach((R x) =>
            {
                secundaryQueue.Enqueue(x);
                tryFlowing();
            });
        }
        else
        {
            primary.Attach((T x) =>
            {
                lastT = x;
                hasT = true;
                tryFlowingLatest();
            });
            
            secondary.Attach((R x) =>
            {
                lastR = x;
                hasR = true;
                tryFlowingLatest();
            });
        }
        
    }

    public ZipFlow(Flow<T> primary, Flow<R> secondary, ZipMode mode)
        : this(primary, secondary, mode != ZipMode.EveryPair, mode == ZipMode.LatestRepeat)
    { }

    private void tryFlowingLatest()
    {
        if (!hasT || !hasR)
            return;
        
        hasT = hasR = repeat;
        Flowing((lastT, lastR));
    }

    private void tryFlowing()
    {
        lock (primaryQueue)
        {
            if (primaryQueue.Count == 0)
                return;
            
            if (secundaryQueue.Count == 0)
                return;
            
            var primaryValue = primaryQueue.Dequeue();
            var secondaryValue = secundaryQueue.Dequeue();
            var data = (primaryValue, secondaryValue);
            Flowing(data);
        }
    }

    public override void Start()
    {
        if (primary is IFlow flow)
            flow.StartAsync();
        else throw InvalidFlowParentException.Default;
        
        if (secondary is IFlow sflow)
            sflow.StartAsync();
        else throw InvalidFlowParentException.Default;
    }
}