using System;

namespace StoreCrown.Domain.Contracts
{
    public class ResetZone
    {
        public DateTimeOffset Timestamp { get; set; }

        public string ZoneId { get; set; }
    }
}