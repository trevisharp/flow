namespace FlowPattern.Flows;

public class ConstantFlow<T> : Flow<T, ConstantFlow<T>>
{
    private T constant;
    private ConstantFlow(T seed)
        => this.constant = seed;

    public override void Start()
    {
        while (true)
            Flowing(constant);
    }

    public static ConstantFlow<T> Create(T seed)
        => new ConstantFlow<T>(seed);
}