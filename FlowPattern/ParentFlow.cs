/* Author:  Leonardo Trevisan Silio
 * Date:    03/04/2023
 */
 namespace FlowPattern;

/// <summary>
/// A Flow with a parent flow.
/// </summary>
/// <typeparam name="T">The type of data flow.</typeparam>
/// <typeparam name="P">The type of parent flow.</typeparam>
public abstract class ParentFlow<T, P> : Flow<T>
    where P : IFlow
{
    /// <summary>
    /// Return to parent flow.
    /// </summary>
    public P Ret { get; protected set; }
}