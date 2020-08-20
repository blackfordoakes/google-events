using System;
using System.Collections.Generic;
using Events.Provider.Models;

namespace Events.Provider.Interfaces
{
    internal interface IDataProvider
    {
        List<SubscriptionChangeEvent> GetEvents();
    }
}
