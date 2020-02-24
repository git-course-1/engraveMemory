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
            builder.RegisterType<MemorialListVm>().SingleInstance();
            builder.RegisterType<MemorialVm>().InstancePerDependency();
            builder.RegisterType<ProgressVm>().InstancePerDependency();
            builder.RegisterType<MemorialRepository>().SingleInstance();            
            builder.RegisterType<RootPageNavigation>().SingleInstance();
            builder.RegisterType<MemorialFilterVm>().SingleInstance();
            builder.RegisterType<MemorialFilterSettings>().SingleInstance();
            builder.RegisterType<PeriodicUpdater>().SingleInstance();
            builder.RegisterEditMemorial();

            builder.RegisterType<AppVm>().SingleInstance();
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