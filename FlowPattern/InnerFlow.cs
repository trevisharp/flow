using System.Threading.Tasks;

namespace FlowPattern;

using Exceptions;

public abstract class InnerFlow<T, R, P> : Flow<R, InnerFlow<T, R, P>>
    where P : IFlow
{
    public P Ret { get; private set; }

    protected InnerFlow(P main)
    {
        main.Attach(x =>
        {
            if (x is T data)
                onMainFlowing(data);
            else throw InvalidDataFlowException.Default;
        });
        this.Ret = main;
    }

    protected abstract void onMainFlowing(T x);

    public override void Start()
    {
        if (Ret is IFlow flow)
            flow.Start();
        else throw InvalidFlowParentException.Default;
    }

    public override async Task StartAsync()
    {
        if (Ret is IFlow flow)
            await flow.StartAsync();
        else throw InvalidFlowParentException.Default;
    }

    public override void ParallelStart()
    {
        if (Ret is IFlow flow)
            flow.ParallelStart();
        else throw InvalidFlowParentException.Default;
    }

    public override async Task ParallelStartAsync()
    {
        if (Ret is IFlow flow)
            await flow.ParallelStartAsync();
        else throw InvalidFlowParentException.Default;
    }
}