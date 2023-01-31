using System.Threading.Tasks;

namespace FlowPattern;

public abstract class SubFlow<T> : Flow<T>
{
    protected SubFlow(Flow<T> main)
    {
        this.Return = main;
        main.OnFlowing += x =>
        {
            onMainFlowing(x);
        };
    }

    protected abstract void onMainFlowing(T x);

    public override void StartFlow()
        => Return.StartFlow();

    public override Task StartFlowAsync()
        => Return.StartFlowAsync();
}