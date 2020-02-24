using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Xamarin.Forms;

namespace EngraveMemory.TabPageSample
{
    public class FirstPageVm : ReactiveObject, IPage
    {
        private readonly TabPageVm _tabPageVm;
        
        [Reactive] public INavigation Navigation { get; set; }

        public FirstPageVm(TabPageVm tabPageVm)
        {
            _tabPageVm = tabPageVm;
        }
    }
}