using System;
using Autofac;
using Events.Provider.Interfaces;

namespace Events.Provider
{
    public class EventModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataProvider>().As<IDataProvider>();
            builder.RegisterType<EventService>().As<IEventService>();
            base.Load(builder);
        }
    }
}
