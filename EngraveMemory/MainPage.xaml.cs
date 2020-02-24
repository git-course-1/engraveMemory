using EngraveMemory.MemorialList;

namespace EngraveMemory
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = CompositonRoot.Resolve<MemorialListVm>();
        }
    }
}