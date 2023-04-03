using System;

namespace FlowPattern.Flows;

public class RandomFlow : Flow<float, RandomFlow>
{
    private Random rnd;
    private RandomFlow(int seed)
        => rnd = new Random(seed);

    public override void Start()
    {
        while (true)
            Flowing(rnd.NextSingle());
    }

    public static RandomFlow Create(int seed)
        => new RandomFlow(seed);
}