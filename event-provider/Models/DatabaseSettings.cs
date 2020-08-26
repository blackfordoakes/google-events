using System;

namespace Events.Provider.Models
{
    public class DatabaseSettings
    {
        public string HostUrl { get; set; }

        public string DynamoTableName { get; set; }
    }
}
