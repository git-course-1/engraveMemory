using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Xamarin.Forms;

namespace EngraveMemory.TabPageSample
{
    public class SecondPageVm : ReactiveObject, IPage
    {
        [Reactive] public INavigation Navigation { get; set; }
    }
}