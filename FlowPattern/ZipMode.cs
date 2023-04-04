/* Author:  Leonardo Trevisan Silio
 * Date:    03/04/2023
 */
namespace FlowPattern;

/// <summary>
/// Define how the zip flow will work.
/// </summary>
public enum ZipMode
{
    /// <summary>
    /// Flowing for every new pair of data.
    /// Save a queue of all data recived.
    /// </summary>
    EveryPair,
    /// <summary>
    /// Flowing for every new pair of data.
    /// Always use the latest data recived and use the data once.
    /// </summary>
    Latest,
    /// <summary>
    /// Flowing for every data.
    /// Always use the latest data recived and repeat use of same data.
    /// </summary>
    LatestRepeat
}