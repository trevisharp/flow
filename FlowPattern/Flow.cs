/* Author:  Leonardo Trevisan Silio
 * Date:    03/04/2023
 */
using System;
using System.Threading.Tasks;

namespace FlowPattern;

using Exceptions;

/// <summary>
/// A generic Data Flow that work with a T type.
/// </summary>
/// <typeparam name="T">The type of data flow.</typeparam>
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

    /// <summary>
    /// Add a object of type T in data flow.
    /// </summary>
    /// <param name="data">Data added to flow.</param>
    public void Flowing(T data)
    {
        if (onFlowing is not null)
            onFlowing(data);
    }

    public void Flowing(object obj)
    {
        if (obj is T data)
            Flowing(data);
        else throw InvalidDataFlowException.Default;
    }

    /// <summary>
    /// Attach a event to execute every time that a object is added in this flow.
    /// </summary>
    /// <param name="action">The function that recive a object of type T.</param>
    public void Attach(Action<T> action)
        => this.onFlowing += delegate(T x)
        {
            action(x);
        };
    
    public void Attach(Action<object> action)
    {
        if (action is Action<T> genericAction)
            Attach(genericAction);
        else Attach((T x) => action(x));
    }
    
    private event Action<T> onFlowing;
}