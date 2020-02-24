using System;
using DevsTeam.Framework.Autofac;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace EngraveMemory.MemorialDetails
{
    [Xamarin.Forms.Xaml.XamlCompilation(Xamarin.Forms.Xaml.XamlCompilationOptions.Compile)]
    public partial class MemorialDetailsPage
    {
        private readonly IDisposable _lifetime;
        private readonly MemorialDetailsVm _memorialDetailsVm;

        public MemorialDetailsPage(Disposable<MemorialDetailsVm> lifetime)
        {
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            BindingContext = lifetime.Value;
            _memorialDetailsVm = lifetime.Value;
            _lifetime = lifetime;
            InitializeComponent();
        }

        protected override async void OnParentSet()
        {
            base.OnParentSet();
            if (Parent != null) return;
            await _memorialDetailsVm.Save(false);
            _lifetime.Dispose();
        }
    }
}
