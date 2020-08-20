using System;
using System.Collections.Generic;
using Events.Provider.Interfaces;
using Events.Provider.Models;

namespace Events.Provider
{
    internal class DataProvider : IDataProvider
    {
        public List<SubscriptionChangeEvent> GetEvents()
        {
            return new List<SubscriptionChangeEvent>();
        }
    }
}
