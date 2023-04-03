/* Author:  Leonardo Trevisan Silio
 * Date:    03/04/2023
 */
namespace FlowPattern.Flows;

/// <summary>
/// A flow to return contantly constant.
/// </summary>
/// <typeparam name="T">Type of constant value.</typeparam>
public class ConstantFlow<T> : Flow<T>
{
    private T constant;
    private ConstantFlow(T seed)
        => this.constant = seed;

    public override void Start()
    {
        while (true)
            Flowing(constant);
    }

    /// <summary>
    /// Create a new constant flow based in a seed.
    /// </summary>
    public static ConstantFlow<T> Create(T seed)
        => new ConstantFlow<T>(seed);
}