
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using StoreCrown.Domain.Projections;
using StoreCrown.Domain.Services;
using Xunit;

namespace StoreCrows.Domain.Tests
{
    public class VisitorCounterTests
    {
        private readonly Fixture fixture = new Fixture();

        private VisitorCounter visitorCounter => new VisitorCounter();

        [Fact]
        public void It_can_increment_counter()
        {
            var zoneId = fixture.Create<string>();
            var state = visitorCounter.Increase(zoneId);

            state.Should().Be(1);
        }

        [Fact]
        public void It_can_decrease_counter()
        {
            var zoneId = fixture.Create<string>();
            var zoneCount = fixture.Create<int>();

            var initialState = new[]
            {
                new KeyValuePair<string, int>(zoneId, zoneCount)
            };

            var sut = visitorCounter;
            sut.Initiate(initialState);
            var state = sut.Decrease(zoneId);

            state.Should().Be(zoneCount - 1);
        }

        [Fact]
        public void It_can_not_decrease_below_zero()
        {
            var zoneId = fixture.Create<string>();
            var zoneCount = 0;

            var initialState = new[]
            {
                new KeyValuePair<string, int>(zoneId, zoneCount)
            };

            var sut = visitorCounter;
            sut.Initiate(initialState);
            var state = sut.Decrease(zoneId);

            state.Should().Be(0);
        }

        [Fact]
        public void It_can_reset_counter()
        {
            var zoneId = fixture.Create<string>();
            var zoneCount = fixture.Create<int>();

            var initialState = new[]
            {
                new KeyValuePair<string, int>(zoneId, zoneCount)
            };

            var sut = visitorCounter;
            sut.Initiate(initialState);
            var state = sut.Reset(zoneId);

            state.Should().Be(0);
        }

        [Fact]
        public void It_can_return_state()
        {
            var zoneId = fixture.Create<string>();
            var zoneCount = fixture.Create<int>();

            var initialState = new[]
            {
                new KeyValuePair<string, int>(zoneId, zoneCount)
            };

            var sut = visitorCounter;
            sut.Initiate(initialState);
            var state = sut.GetZone(zoneId);

            state.ZoneId.Should().Be(zoneId);
            state.Visitors.Should().Be(zoneCount);
        }

        [Fact]
        public void It_can_return_all_states()
        {
            var state = fixture.CreateMany<ZoneStateProjection>().ToList();

            var initialState = state.Select(s => new KeyValuePair<string, int>(s.ZoneId, s.Visitors));
            
            var sut = visitorCounter;
            sut.Initiate(initialState);
            var result = sut.GetZones();

            result.Select(x => x.ZoneId + x.Visitors).Should().Contain(state.Select(s => s.ZoneId + s.Visitors));
        }
    }
}
