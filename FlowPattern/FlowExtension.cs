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
    /// <summary>
    /// Create a Directory Flow in folder.
    /// </summary>
    /// <param name="path">The path of the folder.</param>
    public static DirectoryFlow OpenDirectoryFlow(this string path)
        => DirectoryFlow.Create(path); 
    
    /// <summary>
    /// Create a Text File Flow in a text file.
    /// </summary>
    /// <param name="path">The path of text file.</param>
    public static TextFileFlow OpenTextFileFlow(this string path)
        => TextFileFlow.Create(path);
    
    /// <summary>
    /// Create a constant flow of type T.
    /// </summary>
    /// <param name="seed">The constant value.</param>
    private static ConstantFlow<T> ConstFlow<T>(this T seed)
        => ConstantFlow<T>.Create(seed); 

    /// <summary>
    /// Create a random noise with seed for random parameter.
    /// </summary>
    /// <param name="seed">The integer value to be a seed of random algorithm.</param>
    public static RandomFlow Noise(this int seed)
        => RandomFlow.Create(seed); 

    /// <summary>
    /// Create a random noise with seed for random parameter.
    /// </summary>
    /// <param name="dt">Time to generate the seed of random algorithm.</param>
    /// <returns></returns>
    public static RandomFlow Noise(this DateTime dt)
        => RandomFlow.Create((int)(dt.Ticks % int.MaxValue)); 
    
    /// <summary>
    /// Create a conditional flow to filter data.
    /// </summary>
    public static InnerFlow<T, T, Flow<T>> If<T>(this Flow<T> flow, Predicate<T> predicate)
        => new ConditionalFlow<T, Flow<T>>(flow, predicate);
    
    /// <summary>
    /// Create a conditional flow to filter data.
    /// </summary>
    public static InnerFlow<T, T, ParentFlow<T, P>> If<T, P>(this ParentFlow<T, P> flow, Predicate<T> predicate)
        where P : IFlow => new ConditionalFlow<T, ParentFlow<T, P>>(flow, predicate);
    
    /// <summary>
    /// Create a flow to act over data.
    /// </summary>
    public static Flow<T> Act<T>(this Flow<T> flow, Action<T> action)
        => new ActionFlow<T, Flow<T>>(flow, action).Ret;

    /// <summary>
    /// Create a flow to act over data.
    /// </summary>
    public static ParentFlow<T, P> Act<T, P>(this ParentFlow<T, P> flow, Action<T> action)
        where P : IFlow => new ActionFlow<T, ParentFlow<T, P>>(flow, action).Ret;    
    
    /// <summary>
    /// Create a Map Flow to tranform data.
    /// </summary>
    public static InnerFlow<T, R, Flow<T>> Take<T, R>(this Flow<T> flow, Func<T, R> selector)
        => new MapFlow<T, R, Flow<T>>(flow, selector);
    
    /// <summary>
    /// Create a Map Flow to tranform data.
    /// </summary>
    public static InnerFlow<T, R, ParentFlow<T, P>> Take<T, R, P>(this ParentFlow<T, P> flow, Func<T, R> selector)
        where P : IFlow => new MapFlow<T, R, ParentFlow<T, P>>(flow, selector);
    
    /// <summary>
    /// Create a Map Flow to tranform data.
    /// </summary>
    public static InnerFlow<T, P, ConstantFlow<T>> Take<T, P>(this T seed, Func<T, P> map)
        => new MapFlow<T, P, ConstantFlow<T>>(ConstantFlow<T>.Create(seed), map);
    
    /// <summary>
    /// Join with a sub flow to start for every flowing data.
    /// </summary>
    public static SubFlow<T, R, Flow<T>> Join<T, R>(this Flow<T> flow, Func<T, Flow<R>> creator)
        => new SubFlow<T, R, Flow<T>>(flow, creator);
    
    /// <summary>
    /// Join with a sub flow to start for every flowing data.
    /// </summary>
    public static SubFlow<T, R, ParentFlow<T, P>> Join<T, R, P>(this ParentFlow<T, P> flow, Func<T, Flow<R>> creator)
        where P : IFlow => new SubFlow<T, R, ParentFlow<T, P>>(flow, creator);
    
    /// <summary>
    /// Start a window effect to collect every sequence of size N in a unique flowing data.
    /// </summary>
    public static InnerFlow<T, IEnumerable<T>, Flow<T>> Window<T>(this Flow<T> flow, int N)
        => new WindowFlow<T, Flow<T>>(flow, N);
    
    /// <summary>
    /// Start a window effect to collect every sequence of size N in a unique flowing data.
    /// </summary>
    public static InnerFlow<T, IEnumerable<T>, ParentFlow<T, P>> Window<T, P>(this ParentFlow<T, P> flow, int size)
       where P : IFlow => new WindowFlow<T, ParentFlow<T, P>>(flow, size);
    
    /// <summary>
    /// Start a window effect to collect every sequence of size N in a unique flowing data.
    /// </summary>
    public static InnerFlow<T, IEnumerable<T>, ConstantFlow<T>> Window<T>(this T seed, int size)
        where T : unmanaged => new WindowFlow<T, ConstantFlow<T>>(ConstantFlow<T>.Create(seed), size);
    
    /// <summary>
    /// Start a chumk effect to collect sequences each N elements in a unique flowing data.
    /// </summary>
    public static InnerFlow<T, IEnumerable<T>, Flow<T>> Chunk<T>(this Flow<T> flow, int size)
        => new WindowFlow<T, Flow<T>>(flow, size, true);
        
    /// <summary>
    /// Start a chumk effect to collect sequences each N elements in a unique flowing data.
    /// </summary>
    public static InnerFlow<T, IEnumerable<T>, ParentFlow<T, P>> Chunk<T, P>(this ParentFlow<T, P> flow, int size)
        where P : IFlow => new WindowFlow<T, ParentFlow<T, P>>(flow, size, true);
    
    /// <summary>
    /// Start a chumk effect to collect sequences each N elements in a unique flowing data.
    /// </summary>
    public static InnerFlow<T, IEnumerable<T>, ConstantFlow<T>> Chunk<T>(this T seed, int size)
        where T : unmanaged => new WindowFlow<T, ConstantFlow<T>>(ConstantFlow<T>.Create(seed), size, true);
    
    /// <summary>
    /// Unify two flow in a unique tuple data flow. 
    /// </summary>
    public static ZipFlow<T, R, Flow<T>> Zip<T, R>(
        this Flow<T> flow, Flow<R> sflow, ZipMode mode = ZipMode.EveryPair)
        => new ZipFlow<T, R, Flow<T>>(flow, sflow, mode);
    
    /// <summary>
    /// Unify two flow in a unique tuple data flow. 
    /// </summary>
    public static ZipFlow<T, R, ParentFlow<T, P>> Zip<T, R, P>(
        this ParentFlow<T, P> flow, Flow<R> sflow, ZipMode mode = ZipMode.EveryPair)
        where P : IFlow => new ZipFlow<T, R, ParentFlow<T, P>>(flow, sflow, mode);
        
    /// <summary>
    /// Unify two flow in a unique tuple data flow. 
    /// </summary>
    public static ZipFlow<T, R, ConstantFlow<T>> Zip<T, R>(
        this T seed, Flow<R> sflow, ZipMode mode = ZipMode.EveryPair)
        where T : unmanaged => seed.ConstFlow().Zip(sflow, mode) as ZipFlow<T, R, ConstantFlow<T>>;
        
    /// <summary>
    /// Unify two flow in a unique tuple data flow. 
    /// </summary>
    public static ZipFlow<T, R, Flow<T>> Zip<T, R>(
        this Flow<T> flow, R seed, ZipMode mode = ZipMode.EveryPair)
        where R : unmanaged => flow.Zip(seed.ConstFlow(), mode);
    
    /// <summary>
    /// Unify two flow in a unique tuple data flow. 
    /// </summary>
    public static ZipFlow<T, R, ParentFlow<T, P>> Zip<T, R, P>(
        this ParentFlow<T, P> flow, R seed, ZipMode mode = ZipMode.EveryPair)
        where R : unmanaged
        where P : IFlow
        => flow.Zip(seed.ConstFlow(), mode);
        
    /// <summary>
    /// Unify two flow in a unique tuple data flow. 
    /// </summary>
    public static ZipFlow<T, R, ConstantFlow<T>> Zip<T, R>(
        this T seed, R otherSeed, ZipMode mode = ZipMode.EveryPair)
        where T : unmanaged
        where R : unmanaged
        => new ZipFlow<T, R, ConstantFlow<T>>(seed.ConstFlow(), otherSeed.ConstFlow(), mode);
}