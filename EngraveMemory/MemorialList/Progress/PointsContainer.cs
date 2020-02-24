using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace EngraveMemory.MemorialList
{
    public class PointsContainer : ReactiveObject
    {
        [Reactive]
        public TimePointVm Point0 { get; set; }

        [Reactive]
        public TimePointVm Point1 { get; set; }

        [Reactive]
        public TimePointVm Point2 { get; set; }

        [Reactive]
        public TimePointVm Point3 { get; set; }

        [Reactive]
        public TimePointVm Point4 { get; set; }
    }
}