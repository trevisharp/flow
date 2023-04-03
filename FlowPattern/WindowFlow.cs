/* Author:  Leonardo Trevisan Silio
 * Date:    03/04/2023
 */
using System.Collections.Generic;

namespace FlowPattern;

/// <summary>
/// A window flow that get a collection of N sequencial values.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="P"></typeparam>
public class WindowFlow<T, P> : InnerFlow<T, IEnumerable<T>, P>
    where P : IFlow
{
    private bool clearBehavior = false;
    private int size = 0;
    private Queue<T> queue = new Queue<T>();
    public WindowFlow(P main, int size, bool clearBehavior = false) : base(main)
    {
        this.size = size;
        this.clearBehavior = clearBehavior;
    }

    protected override void onMainFlowing(T x)
    {
        if (queue.Count == size)
        {
            if (clearBehavior)
                queue.Clear();
            else queue.Dequeue();
        }
        
        queue.Enqueue(x);
        
        if (queue.Count == size)
            Flowing(queue);
    }
}