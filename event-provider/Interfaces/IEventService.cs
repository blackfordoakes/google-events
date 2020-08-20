using System;
using System.Collections.Generic;
using Events.Provider.Models;

namespace Events.Provider.Interfaces
{
    public interface IEventService
    {
        List<SubscriptionChangeEvent> GetAllEvents();
    }
}
