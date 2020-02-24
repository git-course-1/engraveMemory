using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Xamarin.Forms;

namespace EngraveMemory.TabPageSample
{
    public class AppVm : ReactiveObject
    {
        public TabPageVm TabPageVm { get; }

        public AppVm(TabPageVm tabPageVm, FirstPageVm firstPageVm, SecondPageVm secondPageVm, ThirdPageVm thirdPageVm)
        {
            TabPageVm = tabPageVm;
            TabPageVm.AddPages(firstPageVm, secondPageVm, thirdPageVm);
        }

        [Reactive]
        public INavigation Navigation { get; set; }
    }
}