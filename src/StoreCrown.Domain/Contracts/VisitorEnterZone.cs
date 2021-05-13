using System;

namespace StoreCrown.Domain.Contracts
{
    public class VisitorEnterZone
    {
        public DateTimeOffset Timestamp { get; set; }

        public string ZoneId { get; set; }
    }
}
