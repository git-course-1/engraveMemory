using ActimaUAT.Common;
using Autofac;
using EngraveMemory.Common;
using EngraveMemory.Engine;
using EngraveMemory.MemorialDetails;
using EngraveMemory.MemorialList;
using EngraveMemory.TabPageSample;
using Xamarin.Forms;

namespace EngraveMemory
{
    public class CompositonRoot
    {
        private static IContainer _container;

        public static void Init()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => DependencyService.Get<ICompressImage>()).SingleInstance();
            builder.RegisterType<MemorialListVm>().ExternallyOwned();
            builder.RegisterType<MemorialVm>().ExternallyOwned();
            builder.RegisterType<ProgressVm>().ExternallyOwned();
            builder.RegisterType<MemorialRepository>().ExternallyOwned();            
            builder.RegisterType<RootPageNavigation>().ExternallyOwned();
            builder.RegisterType<MemorialFilterVm>().InstancePerDependency();
            builder.RegisterType<MemorialFilterSettings>().ExternallyOwned();
            builder.RegisterType<PeriodicUpdater>().ExternallyOwned();
            builder.RegisterEditMemorial();
            builder.RegisterType<TabPageVm>().SingleInstance();
            builder.RegisterType<FirstPageVm>().SingleInstance();
            builder.RegisterType<SecondPageVm>().SingleInstance();
            builder.RegisterType<ThirdPageVm>().SingleInstance();
            
            _container = builder.Build();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}