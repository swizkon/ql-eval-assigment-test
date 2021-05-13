using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using StoreCrown.Domain.Projections;

namespace StoreCrown.Domain.Services
{
    public class VisitorCounter
    {
        private static readonly ConcurrentDictionary<string, int> Counters = new ConcurrentDictionary<string, int>();
        
        public void Initiate(IEnumerable<KeyValuePair<string, int>> initialState)
        {
            foreach (var keyValuePair in initialState)
                Counters.AddOrUpdate(keyValuePair.Key, keyValuePair.Value, (s, i) => keyValuePair.Value);
        }

        public int Increase(string zoneId) 
            => Counters.AddOrUpdate(zoneId, 1, (s, i) => ++i);

        public int Decrease(string zoneId) 
            => Counters.AddOrUpdate(zoneId, 0, (s, i) => Math.Max(i-1, 0));

        public ZoneStateProjection GetZone(string zoneId) =>
            new ZoneStateProjection
            {
                ZoneId = zoneId,
                Visitors = Counters.GetOrAdd(zoneId, 0)
            };

        public IEnumerable<ZoneStateProjection> GetZones()
            => Counters.Keys.Select(GetZone);

        public int Reset(string zoneId) 
            => Counters.AddOrUpdate(zoneId, 0, (s, i) => 0);
    }
}
