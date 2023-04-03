/* Author:  Leonardo Trevisan Silio
 * Date:    03/04/2023
 */
using System;

namespace FlowPattern;

/// <summary>
/// A flow to act over data without update any value.
/// </summary>
/// <typeparam name="T">Input type of data.</typeparam>
/// <typeparam name="P">Type of parent flow.</typeparam>
public class ActionFlow<T, P> : InnerFlow<T, T, P>
    where P : IFlow
{
    private Action<T> act;
    public ActionFlow(P main, Action<T> act)
        : base(main) => this.act = act;

    protected override void onMainFlowing(T x)
        => this.act(x);
}