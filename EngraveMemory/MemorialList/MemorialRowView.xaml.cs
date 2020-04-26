using Xamarin.Forms;

namespace EngraveMemory.MemorialList
{
    public partial class MemorialRowView : ContentView
    {
        public MemorialRowView()
        {
            InitializeComponent();

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var b = BindingContext;
        }
    }
}