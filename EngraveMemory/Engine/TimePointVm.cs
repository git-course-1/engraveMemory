using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using EngraveMemory.Common;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Xamarin.Forms;

namespace EngraveMemory.Engine
{
    public class TimePointVm : ReactiveObject, IDisposable
    {
        private readonly TimePointVm _prevPointVm;
        private readonly DateTime _lastRepeateDate;
        

        private readonly Dictionary<int, Color> _progressColorMap = new Dictionary<int, Color>
        {
            {-1, Color.FromHex("ff7f00")},
            {0, Color.FromHex("ffba00")},
            {1, Color.FromHex("e8f027")},
            {2, Color.FromHex("b8ed28")},
            {3, Color.FromHex("6ecc29")}
        };

        private readonly CompositeDisposable _lifetime = new CompositeDisposable();

        public TimePointVm(int orderNumber, DateTime timestamp, TimePointVm prevPointVm, DateTime lastRepeateDate)
        {
            _prevPointVm = prevPointVm;
            _lastRepeateDate = lastRepeateDate;
            OrderNumber = orderNumber;
            Timestamp = timestamp;
            ProgressColor = _progressColorMap[OrderNumber];
            InvalidateProgress();
            Repeated = _lastRepeateDate >= Timestamp;
            InvalidateColor();
        }
        
        [Reactive] public string TimeToRepeat { get; private set; }
        
        private double GetProgress(out TimeSpan left)
        {
            var total = Timestamp - _prevPointVm.Timestamp;
            var current = DateTime.Now - _prevPointVm.Timestamp;
            left = total < current ? TimeSpan.Zero : total - current;
            var progress = current.TotalMilliseconds / total.TotalMilliseconds;
            if (progress < 0) return 0;
            return progress;
        }
        
        [Reactive] public double Progress { get; private set; }

        private void InvalidateProgress()
        {
            try
            {
                if (_prevPointVm == null)
                {
                    Progress = 0;
                    Color = _progressColorMap[OrderNumber];                
                    return;
                }

                if (DateTime.Now > Timestamp)
                {
                    Progress = 1;
                    return;
                }

                _lifetime.Add(CompositonRoot.Resolve<PeriodicUpdater>().UpdateEachSecond(()=>
                {
                    Progress = GetProgress(out var timeToRepeat);
                    TimeToRepeat = GetTimeToRepeat(timeToRepeat);
                    InvalidateNeedRepeat();  
                }));
            }
            finally
            {
                InvalidateNeedRepeat();  
            }
        }

        private string GetTimeToRepeat(TimeSpan left)
        {
            if (left.TotalHours > 24) return $"{(int)left.TotalDays} (дней)";

            if (left.TotalMinutes > 60) return $"{(int)left.TotalHours} (часов)";
            
            if (left.TotalSeconds > 60) return $"{(int)left.TotalMinutes} (минут)";
            
            return $"{(int)left.TotalSeconds} (секунд)";
        }

        public Color ProgressColor { get; }
        
        [Reactive]
        public Color Color { get; private set; }

        public DateTime Timestamp { get; }

        public int OrderNumber { get; }

        public void Dispose() => _lifetime.Dispose();

        private void InvalidateNeedRepeat()
        {
            NeedRepeat = !Repeated && _prevPointVm != null && DateTime.Now > Timestamp && _lastRepeateDate < Timestamp;
        }

        private void InvalidateColor()
        {
            Color = Repeated ? _progressColorMap[OrderNumber] : App.ProgressGray;
        }

        [Reactive]
        public bool NeedRepeat { get; private set; }

        public bool Repeated { get;}

    }
}