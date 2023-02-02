using System;

namespace FlowPattern;

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