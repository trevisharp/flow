/* Author:  Leonardo Trevisan Silio
 * Date:    03/04/2023
 */
using System;

namespace FlowPattern;

/// <summary>
/// A conditional flow that Flowing considerating a condition.
/// </summary>
/// <typeparam name="T">Input type of flow.</typeparam>
/// <typeparam name="P">Parent type of flow.</typeparam>
public class ConditionalFlow<T, P> : InnerFlow<T, T, P>
    where P : IFlow
{
    private Predicate<T> predicate;
    public ConditionalFlow(P main, Predicate<T> predicate) 
        : base(main) => this.predicate = predicate;

    protected override void onMainFlowing(T x)
    {
        if (predicate(x))
            Flowing(x);
    }
}