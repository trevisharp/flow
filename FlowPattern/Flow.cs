using System;
using System.Threading.Tasks;

namespace FlowPattern;

using Exceptions;

public abstract class Flow<T> : IFlow
{
    public abstract void Start();
    public virtual async Task StartAsync()
        => await Task.Run(Start);
    public virtual void ParallelStart()
    {
        throw new NotImplementedException(
            "A execução paralela não está disponível para esse objeto"
        );
    }
    public virtual async Task ParallelStartAsync()
        => await Task.Run(ParallelStart);

    public void Flowing(T data)
    {
        if (onFlowing != null)
            onFlowing(data);
    }
    public void Flowing(object obj)
    {
        if (obj is T data)
            Flowing(data);
        else throw InvalidDataFlowException.Default;
    }

    public void Attach(Action<T> action)
        => this.onFlowing += action;
    public void Attach(Action<object> action)
    {
        if (action is Action<T> genericAction)
            Attach(genericAction);
        else Attach((T x) => action(x));
    }
    
    private event Action<T> onFlowing;

    public SubFlow<T, Flow<T>> If(Predicate<T> predicate)
        => new ConditionalFlow<T, Flow<T>>(this, predicate);
    
    public Flow<T> Act(Action<T> action)
        => new ActionFlow<T, Flow<T>>(this, action).Ret;
}