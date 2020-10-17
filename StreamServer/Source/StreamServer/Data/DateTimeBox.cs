using System;

namespace StreamServer.Data
{
    public class DateTimeBox
    {
        public readonly DateTime LastUpdated;

        public DateTimeBox(DateTime lastUpdated)
        {
            LastUpdated = lastUpdated;
        }
    }
}
