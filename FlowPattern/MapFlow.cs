/* Author:  Leonardo Trevisan Silio
 * Date:    03/04/2023
 */
using System;

namespace FlowPattern;

/// <summary>
/// A flow of transformation data from type T to type R.
/// </summary>
/// <typeparam name="T">Input type.</typeparam>
/// <typeparam name="R">Output type.</typeparam>
/// <typeparam name="P">Parent type.</typeparam>
public class MapFlow<T, R, P> : InnerFlow<T, R, P>
    where P : IFlow
{
    private Func<T, R> func;
    public MapFlow(P main, Func<T, R> func)
        : base(main) => this.func = func;

    protected override void onMainFlowing(T x)
    {
        var data = func(x);
        Flowing(data);
    }
}