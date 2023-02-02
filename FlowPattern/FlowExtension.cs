using System;

namespace FlowPattern;

using Flows;

public static class FlowExtension
{
    public static DirectoryFlow OpenDirectoryFlow(this string path)
        => DirectoryFlow.Create(path); 
        
    public static TextFileFlow OpenTextFileFlow(this string path)
        => TextFileFlow.Create(path); 
        
    public static InnerFlow<T, T, S> If<T, S>(this Flow<T, S> flow, Predicate<T> predicate)
        where S : Flow<T, S> => new ConditionalFlow<T, S>(flow as S, predicate);
    
    public static S Act<T, S>(this Flow<T, S> flow, Action<T> action)
        where S : Flow<T, S> => new ActionFlow<T, S>(flow as S, action).Ret;
    
    public static InnerFlow<T, R, S> Take<T, R, S>(this Flow<T, S> flow, Func<T, R> map)
        where S : Flow<T, S> => new MapFlow<T, R, S>(flow as S, map);
    
    public static InnerFlow<T, T, S> Set<T, S>(this Flow<T, S> flow, Func<T, T> map)
        where S : Flow<T, S> => new MapFlow<T, T, S>(flow as S, map);
    
    public static SubFlow<T, tS, R, rS> Join<T, R, tS, rS>(this Flow<T, tS> flow, Func<T, Flow<R, rS>> creator)
        where tS : Flow<T, tS>
        where rS : Flow<R, rS>
        => new SubFlow<T, tS, R, rS>(flow, creator);
    
    public static InnerFlow<T, (T i0, R i1), S> Zip<T, R, S>(this Flow<T, S> flow, Func<T, R> selector)
        where S : Flow<T, S> => new MapFlow<T, (T, R), S>(flow as S, x => (x, selector(x)));
    
    public static InnerFlow<(T1, T2), (T1 i0, T2 i1, R i2), S> Zip<T1, T2, R, S>(
        this Flow<(T1, T2), S> flow, Func<(T1, T2), R> selector)
        where S : Flow<(T1, T2), S> => 
        new MapFlow<(T1, T2), (T1, T2, R), S>(flow as S, x => (x.Item1, x.Item2, selector(x)));
    
    public static InnerFlow<(T1, T2, T3), (T1 i0, T2 i1, T3 i2, R i3), S> Zip<T1, T2, T3, R, S>(
        this Flow<(T1, T2, T3), S> flow, Func<(T1, T2, T3), R> selector)
        where S : Flow<(T1, T2, T3), S> => 
        new MapFlow<(T1, T2, T3), (T1, T2, T3, R), S>(flow as S, x => (x.Item1, x.Item2, x.Item3, selector(x)));
}