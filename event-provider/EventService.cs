using System;
using System.Collections.Generic;
using Events.Provider.Interfaces;
using Events.Provider.Models;

namespace Events.Provider
{
    internal class EventService : IEventService
    {
        private readonly IDataProvider _dataProvider;

        public EventService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public List<SubscriptionChangeEvent> GetAllEvents()
        {
            return _dataProvider.GetEvents();
        }
    }
}
