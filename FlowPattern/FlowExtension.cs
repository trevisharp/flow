/* Author:  Leonardo Trevisan Silio
 * Date:    03/04/2023
 */
using System;
using System.Collections.Generic;

namespace FlowPattern;

using Flows;

/// <summary>
/// Extension Methods to improve the flow pattern.
/// </summary>
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
        
    public static InnerFlow<T, T, S> If<T, S>(this Flow<T> flow, Predicate<T> predicate)
        where S : Flow<T> => new ConditionalFlow<T, S>(flow as S, predicate);
        
    public static InnerFlow<T, T, ConstantFlow<T>> If<T>(this T seed, Predicate<T> predicate)
        where T : unmanaged => new ConditionalFlow<T, ConstantFlow<T>>(ConstantFlow<T>.Create(seed), predicate);
    
    public static S Act<T, S>(this Flow<T> flow, Action<T> action)
        where S : Flow<T> => new ActionFlow<T, S>(flow as S, action).Ret;
    
    public static ConstantFlow<T> Act<T>(this T seed, Action<T> action)
        where T : unmanaged => new ActionFlow<T, ConstantFlow<T>>(ConstantFlow<T>.Create(seed) as ConstantFlow<T>, action).Ret;
    
    public static InnerFlow<T, R, P> Take<T, R, P>(this Flow<T> flow, Func<T, R> map)
        where P : Flow<T> => new MapFlow<T, R, P>(flow as P, map);
    
    public static InnerFlow<T, P, ConstantFlow<T>> Take<T, P>(this T seed, Func<T, P> map)
        where T : unmanaged => new MapFlow<T, P, ConstantFlow<T>>(ConstantFlow<T>.Create(seed), map);
    
    public static InnerFlow<T, IEnumerable<T>, P> Window<T, P>(this Flow<T> flow, int size)
        where P : Flow<T> => new WindowFlow<T, P>(flow as P, size);
    
    public static InnerFlow<T, IEnumerable<T>, ConstantFlow<T>> Window<T>(this T seed, int size)
        where T : unmanaged => new WindowFlow<T, ConstantFlow<T>>(ConstantFlow<T>.Create(seed), size);
    
    public static InnerFlow<T, IEnumerable<T>, P> Chunk<T, P>(this Flow<T> flow, int size)
        where P : Flow<T> => new WindowFlow<T, P>(flow as P, size, true);
    
    public static InnerFlow<T, IEnumerable<T>, ConstantFlow<T>> Chunk<T>(this T seed, int size)
        where T : unmanaged => new WindowFlow<T, ConstantFlow<T>>(ConstantFlow<T>.Create(seed), size, true);
    
    public static SubFlow<T, R, P> Join<T, R, P>(this Flow<T> flow, Func<T, Flow<R>> creator)
        where P : Flow<T> => new SubFlow<T, R, P>(flow as P, creator);
    
    public static ZipFlow<T, R, Flow<T>> Zip<T, R>(this Flow<T> flow, Flow<R> sflow)
        => new ZipFlow<T, R, Flow<T>>(flow, sflow);
        
    public static ZipFlow<T, R, ConstantFlow<T>> Zip<T, R>(this T seed, Flow<R> sflow)
        where T : unmanaged => seed.ConstFlow().Zip(sflow) as ZipFlow<T, R, ConstantFlow<T>>;
        
    public static ZipFlow<T, R, Flow<T>> Zip<T, R>(this Flow<T> flow, R seed)
        where R : unmanaged => flow.Zip(seed.ConstFlow());
        
    public static ZipFlow<T, R, ConstantFlow<T>> Zip<T, R>(this T seed, R otherSeed)
        where T : unmanaged
        where R : unmanaged
        => seed.ConstFlow().Zip(otherSeed.ConstFlow()) as ZipFlow<T, R, ConstantFlow<T>>;       
}