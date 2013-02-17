using System;
using System.Threading.Tasks;

namespace JibbR.Queuing
{
    public interface IEventBus
    {
        Task Push<TEvent>(TEvent pushEvent);
        IObservable<TEvent> Pull<TEvent>();
    }
}