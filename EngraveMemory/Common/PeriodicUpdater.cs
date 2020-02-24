using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;

namespace EngraveMemory.Common
{
    public class PeriodicUpdater
    {
        private List<Action> _eachSecondUpdateList = new List<Action>();
        private List<Action> _eachMinuteUpdateList = new List<Action>();
        private IObservable<long> _eachSecondObservable;
        
        public IDisposable UpdateEachSecond(Action action)
        {
            action();
            if (_eachSecondObservable == null)
            {
                _eachSecondObservable = Observable.Interval(TimeSpan.FromSeconds(1)).ObserveOn(RxApp.MainThreadScheduler);
                _eachSecondObservable.Subscribe(t => _eachSecondUpdateList.ToList().ForEach(a => a()));
            }

            _eachSecondUpdateList.Add(action);
            return Disposable.Create(()=>
            {
                _eachSecondUpdateList.Remove(action);
            });
        }

        public void UpdateEachMinute()
        {
        }
    }
}