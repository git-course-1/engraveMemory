using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Xamarin.Forms;

namespace EngraveMemory.TabPageSample
{
    public class TabPageVm : ReactiveObject, IPage
    {
        public ObservableCollection<IPage> Pages { get; } = new ObservableCollection<IPage>();

        [Reactive]
        public IPage SelectedPage { get; set; }

        [Reactive]
        public INavigation Navigation { get; set; }

        public TPage SelectPage<TPage>()
            where TPage : class, IPage
        {
            var page = Pages.FirstOrDefault(p => p.GetType() == typeof(TPage));
            return (TPage) page;
        }

        public void AddPages(params IPage[] pages)
        {
            foreach (var page in pages) Pages.Add(page);
        }

        public bool IsSelected<TPage>()
            where TPage : IPage
        {
            return SelectedPage.GetType() == typeof(TPage);
        }
        
        public bool IsSelected(IPage page)
        {
            return SelectedPage == page;
        }
        
        public void SelectPage(IPage page)
        {
            SelectedPage = page;
        }
    }
}