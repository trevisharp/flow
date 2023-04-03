/* Author:  Leonardo Trevisan Silio
 * Date:    03/04/2023
 */
using System;
using System.Threading.Tasks;

namespace FlowPattern;

/// <summary>
/// Represents Flow of data.
/// </summary>
public interface IFlow
{
    /// <summary>
    /// Start the flow of data.
    /// </summary>
    void Start();
    /// <summary>
    /// Start the data flow asynchronously.
    /// </summary>
    Task StartAsync();
    /// <summary>
    /// Start the data flow in parallel events.
    /// </summary>
    void ParallelStart();
    /// <summary>
    /// Start the data flow in parallel events asynchronously.
    /// </summary>
    Task ParallelStartAsync();
    /// <summary>
    /// Add a object in data flow.
    /// </summary>
    /// <param name="obj">Data added to flow.</param>
    void Flowing(object obj);
    /// <summary>
    /// Attach a event to execute every time that a object is added in this flow.
    /// </summary>
    /// <param name="action">The function that recive a object.</param>
    void Attach(Action<object> action);
}