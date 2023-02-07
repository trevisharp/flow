using System.Collections.Generic;
using System.Threading.Tasks;
using FlowPattern.Exceptions;

namespace FlowPattern;

public class ZipFlow<T, R, P, S> : Flow<(T, R), ZipFlow<T, R, P, S>>
    where P : Flow<T, P>
    where S : Flow<R, S>
{
    public P Ret => primary;

    private Queue<T> primaryQueue = new Queue<T>();
    private Queue<R> secundaryQueue = new Queue<R>();
    private P primary;
    private S secondary;
    public ZipFlow(P primary, S secondary)
    {
        this.primary = primary;
        this.secondary = secondary;
        
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