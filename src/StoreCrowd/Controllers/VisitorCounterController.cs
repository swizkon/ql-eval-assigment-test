using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StoreCrown.Domain.Contracts;
using StoreCrown.Domain.Projections;
using StoreCrown.Domain.Services;

namespace StoreCrowd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VisitorCounterController : ControllerBase
    {
        private readonly VisitorCounter _visitorCounter;

        public VisitorCounterController(VisitorCounter visitorCounter)
        {
            _visitorCounter = visitorCounter;
        }

        [HttpGet]
        public IEnumerable<ZoneStateProjection> Get()
            => _visitorCounter.GetZones();

        [HttpPost("enter")]
        public ZoneStateProjection VisitorEnteringZone(VisitorEnterZone model)
            => HandleRequest(zoneId: model.ZoneId, s => _visitorCounter.Increase(s));

        [HttpPost("exit")]
        public ZoneStateProjection VisitorExitingZone(VisitorExitZone model)
            => HandleRequest(zoneId: model.ZoneId, s => _visitorCounter.Decrease(s));

        [HttpPost("reset")]
        public ZoneStateProjection ResetZone(ResetZone model)
            => HandleRequest(zoneId: model.ZoneId, s => _visitorCounter.Reset(s));

        private static ZoneStateProjection HandleRequest(string zoneId, Func<string, int> modifier)
            => new ZoneStateProjection
            {
                ZoneId = zoneId,
                Visitors = modifier(zoneId)
            };

    }
}