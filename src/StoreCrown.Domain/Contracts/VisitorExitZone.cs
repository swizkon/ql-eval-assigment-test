using System;

namespace StoreCrown.Domain.Contracts
{
    public class VisitorExitZone
    {
        public DateTimeOffset Timestamp { get; set; }

        public string ZoneId { get; set; }
    }
}