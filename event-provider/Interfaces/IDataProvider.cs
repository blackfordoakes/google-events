using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Events.Provider.Models;

namespace Events.Provider.Interfaces
{
    internal interface IDataProvider
    {
        Task<List<SubscriptionChangeEvent>> GetEvents();

        Task WriteEvent(SubscriptionChangeEvent change);
    }
}
