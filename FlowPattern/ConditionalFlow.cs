using System;

namespace FlowPattern;

public class ConditionalFlow<T> : SubFlow<T>
{
    private Predicate<T> predicate;
    public ConditionalFlow(Flow<T> main, Predicate<T> predicate) 
        : base(main) => this.predicate = predicate;

    protected override void onMainFlowing(T x)
    {
        if (predicate(x))
            onFlowing(x);
    }
}