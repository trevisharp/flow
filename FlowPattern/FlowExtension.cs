using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace FlowPattern;

using Flows;

public static class FlowExtension
{
    public static DirectoryFlow OpenDirectoryFlow(this string path)
        => DirectoryFlow.Create(path); 
        
    public static TextFileFlow OpenTextFileFlow(this string path)
        => TextFileFlow.Create(path);
    
    private static ConstantFlow<T> ConstFlow<T>(this T seed)
        => ConstantFlow<T>.Create(seed); 

    public static RandomFlow Noise(this int seed)
        => RandomFlow.Create(seed); 

    public static RandomFlow Noise(this DateTime dt)
        => RandomFlow.Create((int)(dt.Ticks % int.MaxValue)); 
        
    public static InnerFlow<T, T, S> If<T, S>(this Flow<T, S> flow, Predicate<T> predicate)
        where S : Flow<T, S> => new ConditionalFlow<T, S>(flow as S, predicate);
        
    public static InnerFlow<T, T, ConstantFlow<T>> If<T>(this T seed, Predicate<T> predicate)
        where T : unmanaged => new ConditionalFlow<T, ConstantFlow<T>>(ConstantFlow<T>.Create(seed), predicate);
    
    public static S Act<T, S>(this Flow<T, S> flow, Action<T> action)
        where S : Flow<T, S> => new ActionFlow<T, S>(flow as S, action).Ret;
    
    public static ConstantFlow<T> Act<T>(this T seed, Action<T> action)
        where T : unmanaged => new ActionFlow<T, ConstantFlow<T>>(ConstantFlow<T>.Create(seed) as ConstantFlow<T>, action).Ret;
    
    public static InnerFlow<T, R, S> Take<T, R, S>(this Flow<T, S> flow, Func<T, R> map)
        where S : Flow<T, S> => new MapFlow<T, R, S>(flow as S, map);
    
    public static InnerFlow<T, R, ConstantFlow<T>> Take<T, R>(this T seed, Func<T, R> map)
        where T : unmanaged => new MapFlow<T, R, ConstantFlow<T>>(ConstantFlow<T>.Create(seed), map);
    
    public static InnerFlow<T, IEnumerable<T>, S> Window<T, S>(this Flow<T, S> flow, int size)
        where S : Flow<T, S> => new WindowFlow<T, S>(flow as S, size);
    
    public static InnerFlow<T, IEnumerable<T>, ConstantFlow<T>> Window<T>(this T seed, int size)
        where T : unmanaged => new WindowFlow<T, ConstantFlow<T>>(ConstantFlow<T>.Create(seed), size);
    
    public static InnerFlow<T, IEnumerable<T>, S> Chunk<T, S>(this Flow<T, S> flow, int size)
        where S : Flow<T, S> => new WindowFlow<T, S>(flow as S, size, true);
    
    public static InnerFlow<T, IEnumerable<T>, ConstantFlow<T>> Chunk<T>(this T seed, int size)
        where T : unmanaged => new WindowFlow<T, ConstantFlow<T>>(ConstantFlow<T>.Create(seed), size, true);
    
    public static SubFlow<T, tS, R, rS> Join<T, R, tS, rS>(this Flow<T, tS> flow, Func<T, Flow<R, rS>> creator)
        where tS : Flow<T, tS>
        where rS : Flow<R, rS>
        => new SubFlow<T, tS, R, rS>(flow, creator);
    
    public static ZipFlow<T, R, P, S> Zip<T, R, P, S>(this Flow<T, P> flow, Flow<R, S> sflow)
        where P : Flow<T, P>
        where S : Flow<R, S>
        => new ZipFlow<T, R, P, S>(flow as P, sflow as S);
        
    public static ZipFlow<T, R, ConstantFlow<T>, S> Zip<T, R, S>(this T seed, Flow<R, S> sflow)
        where T : unmanaged
        where S : Flow<R, S>
        => seed.ConstFlow().Zip(sflow);
        
    public static ZipFlow<T, R, P, ConstantFlow<R>> Zip<T, R, P>(this Flow<T, P> flow, R seed)
        where P : Flow<T, P>
        where R : unmanaged
        => flow.Zip(seed.ConstFlow());
        
    public static ZipFlow<T, R, ConstantFlow<T>, ConstantFlow<R>> Zip<T, R>(this T seed, R otherSeed)
        where T : unmanaged
        where R : unmanaged
        => seed.ConstFlow().Zip(otherSeed.ConstFlow());
        
}