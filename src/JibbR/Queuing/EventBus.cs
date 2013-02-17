using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace JibbR.Queuing
{
    public class EventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, object> _subjects = new ConcurrentDictionary<Type, object>();

        public Task Push<TEvent>(TEvent pushEvent)
        {
            if (pushEvent == null)
            {
                throw new ArgumentNullException("pushEvent");
            }

            return Task.Factory.StartNew(() =>
            {
                var operationType = pushEvent.GetType();

                InvokeCompatible(operationType, pushEvent);
            });
        }

        private void InvokeCompatible(Type operationtType, object operation)
        {
            var compatible = from key in _subjects.Keys
                             where key.IsAssignableFrom(operationtType)
                             select _subjects[key];

            foreach (dynamic subject in compatible)
            {
                subject.OnNext((dynamic) operation);
            }
        }

        public IObservable<TEvent> Pull<TEvent>()
        {
            return (IObservable<TEvent>)_subjects.GetOrAdd(typeof(TEvent), t => new Subject<TEvent>());
        }
    }
}