using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using EngraveMemory.Common;
using EngraveMemory.Engine;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace EngraveMemory.MemorialList
{
    public class ProgressVm : ReactiveObject
    {
        private Memorial _memorial;
        private CompositeDisposable _pointsLifetime;
        private List<TimePointVm> _timePoints;

        public ProgressVm(Memorial memorial)
        {
            _memorial = memorial;
        }

        [Reactive]
        public TimePointVm Point0 { get; private set; }

        [Reactive]
        public TimePointVm Point1 { get; private set; }

        [Reactive]
        public TimePointVm Point2 { get; private set; }

        [Reactive]
        public TimePointVm Point3 { get; private set; }

        [Reactive]
        public TimePointVm Point4 { get; private set; }
        
        [Reactive]
        public TimePointVm NextRepeatPoint { get; private set; }
        
        private void InvalidateNextRepeatPoint()
        {
            NextRepeatPoint = _timePoints.FirstOrDefault(p => !p.Repeated);
        }

        public void Invalidate(Memorial memorial)
        {
            _memorial = memorial;
            _timePoints = GetTimePoints().ToList();
            InvalidateNextRepeatPoint();
            Point0 = _timePoints[0];
            Point1 = _timePoints[1];
            Point2 = _timePoints[2];
            Point3 = _timePoints[3];
            Point4 = _timePoints[4];
        }

        private IEnumerable<TimePointVm> GetTimePoints()
        {
            _pointsLifetime?.Dispose();
            _pointsLifetime = new CompositeDisposable();
            var prev = new TimePointVm(-1, _memorial.Timestamp, null, _memorial.LastRepeatDate);
            yield return prev;
            var repeatDateTimes = _memorial.Timestamp.GetRepeatDateTimes().OrderBy(p => p).ToArray();
            for (var i = 0; i < repeatDateTimes.Length; i++)
            {
                var current = new TimePointVm(i, repeatDateTimes[i], prev, _memorial.LastRepeatDate);
                prev = current;
                _pointsLifetime.Add(current);
                _pointsLifetime.Add(current.WhenAnyValue(p => p.NeedRepeat).Where(t => _timePoints != null).Subscribe(async p =>
                {
                    await Task.Delay(300);
                    InvalidateNextRepeatPoint();
                }));
                yield return current;
            }
        }
    }
}