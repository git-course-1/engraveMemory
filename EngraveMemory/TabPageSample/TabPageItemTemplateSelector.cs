using Xamarin.Forms;

namespace EngraveMemory.TabPageSample
{
    public class TabPageItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FirstPage { get; set; }
        public DataTemplate SecondPage { get; set; }
        public DataTemplate ThirdPage { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is FirstPageVm) return FirstPage;
            if (item is SecondPageVm) return SecondPage;
            if (item is ThirdPageVm) return ThirdPage;
            throw new System.NotImplementedException();
        }
    }
}