using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Events.Provider.Models;

namespace Events.Provider.Interfaces
{
    public interface IEventService
    {
        Task<List<SubscriptionChangeEvent>> GetAllEvents();

        Task AddEvent(SubscriptionChangeEvent changeEvent);
    }
}
