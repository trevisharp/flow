using System;

namespace FlowPattern;

public class ActionFlow<T> : SubFlow<T>
{
    private Action<T> act;
    public ActionFlow(Flow<T> main, Action<T> act)
        : base(main) => this.act = act;

    protected override void onMainFlowing(T x)
        => this.act(x);
}