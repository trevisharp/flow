using System;

namespace FlowPattern;

public class ActionFlow<T, P> : SubFlow<T, P>
    where P : IFlow
{
    private Action<T> act;
    public ActionFlow(P main, Action<T> act)
        : base(main) => this.act = act;

    protected override void onMainFlowing(T x)
        => this.act(x);
}