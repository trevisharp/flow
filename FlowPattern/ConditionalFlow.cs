using System;

namespace FlowPattern;

public class ConditionalFlow<T, P> : SubFlow<T, T, P>
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