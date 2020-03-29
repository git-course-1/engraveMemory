using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace EngraveMemory.MemorialList
{
    public class MemorialRow : ReactiveObject
    {
        public MemorialRow(MemorialVm left, MemorialVm right)
        {
            Left = left;
            Right = right;
        }

        [Reactive]
        public MemorialVm Left { get; private set; }
        
        [Reactive]
        public MemorialVm Right { get; private set; }

        public void ChangeItems(MemorialVm left, MemorialVm right)
        {
            Left = left;
            Right = right;
        }
    }
}