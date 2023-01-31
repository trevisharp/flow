using System;
using System.Threading.Tasks;

namespace FlowPattern;

public interface IFlow
{
    void Start();
    Task StartAsync();
    void ParallelStart();
    Task ParallelStartAsync();
    void Flowing(object obj);
    void Attach(Action<object> action);
}