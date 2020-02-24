using Xamarin.Forms;

namespace EngraveMemory.MemorialList
{
    public partial class MemorialView : ContentView
    {
        public MemorialView() => InitializeComponent();

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var b = BindingContext;
        }
    }
}