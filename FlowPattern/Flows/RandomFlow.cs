/* Author:  Leonardo Trevisan Silio
 * Date:    03/04/2023
 */
using System;

namespace FlowPattern.Flows;

/// <summary>
/// Random flow of float values
/// </summary>
public class RandomFlow : Flow<float>
{
    private Random rnd;
    private RandomFlow(int seed)
        => rnd = new Random(seed);

    public override void Start()
    {
        while (true)
            Flowing(rnd.NextSingle());
    }

    /// <summary>
    /// Create a random flow based in a seed to random algorithm.
    /// </summary>
    public static RandomFlow Create(int seed)
        => new RandomFlow(seed);
}