using System;
using System.Threading.Tasks;

namespace FlowPattern;

public abstract class Flow<T>
{
    public abstract void StartFlow();
    public virtual async Task StartFlowAsync()
        => await Task.Run(StartFlow);

    private Flow<T> parent = null;
    public Flow<T> Return
    {
        get => parent ?? this;
        protected set => parent = value;
    }

    protected void onFlowing(T data)
    {
        if (OnFlowing != null)
            OnFlowing(data);
    }

    public event Action<T> OnFlowing;
}