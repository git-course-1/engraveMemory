using System;
using Autofac;
using EngraveMemory.Common;
using EngraveMemory.Engine;

namespace EngraveMemory.MemorialDetails
{
    public static class EditMemorialRegistration
    {
        public static void RegisterEditMemorial(this ContainerBuilder builder)
        {
            DevsTeam.Framework.Autofac.FactoryRegistration.RegisterFactory<MemorialDetailsVm, Memorial>(builder,
                (childBuilder, m) =>
                {
                    childBuilder.RegisterType<MemorialDetailsVm>().SingleInstance();
                    childBuilder.Register<Memorial>(c => m);
                });
            
//           
        }
    }
}