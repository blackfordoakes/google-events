using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<List<SubscriptionChangeEvent>> GetAllEvents()
        {
            return await _dataProvider.GetEvents();
        }

        public async Task AddEvent(SubscriptionChangeEvent changeEvent)
        {
            await _dataProvider.WriteEvent(changeEvent);
        }
    }
}
