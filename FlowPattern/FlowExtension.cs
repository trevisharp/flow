using System;

namespace FlowPattern;

public static class FlowExtension
{
    public static Flow<T> If<T>(this Flow<T> main, Predicate<T> predicate)
        => new ConditionalFlow<T>(main, predicate);
    
    public static Flow<T> Act<T>(this Flow<T> main, Action<T> action)
    {
        var flow = new ActionFlow<T>(main, action);
        return flow.Return;
    }
}