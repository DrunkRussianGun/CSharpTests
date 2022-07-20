using FluentAssertions;
using Newtonsoft.Json;
using NodaTime;
using NUnit.Framework;

namespace NetCoreTests.NodaTime
{
    [TestFixture]
    public class PeriodShould
    {
        [Test]
        public void ReturnCorrectString()
        {
            var period = Period.FromDays(1);

            var periodString = period.ToString();

            periodString.Should().Be("P1D");
        }

        [Test]
        public void SerializeToJson()
        {
            var period = Period.FromDays(1);

            var serializedPeriod = JsonConvert.SerializeObject(period);

            serializedPeriod.Should().Be("\"P1D\"");
        }

        [Test]
        public void SerializeToAndDeserializeFromJson()
        {
            var expectedPeriod = Period.FromDays(1);

            var serializedPeriod = JsonConvert.SerializeObject(expectedPeriod);
            var actualPeriod = JsonConvert.DeserializeObject<Period>(serializedPeriod);

            actualPeriod.Should().BeEquivalentTo(expectedPeriod);
        }

        [TestCase(60, 1)]
        [TestCase(61, 2)]
        public void BeConvertableFromDaysToMonths(int periodInDays, int expectedMonthsCount)
        {
            var currentDateTime = new LocalDateTime(2021, 08, 01, 00, 00);
            var period = Period.FromDays(periodInDays);

            var actualMonthsCount = Period
                .Between(currentDateTime, currentDateTime + period, PeriodUnits.Months)
                .Months;

            actualMonthsCount.Should().Be(expectedMonthsCount);
        }

        [TestCase(1, 1)]
        [TestCase(2, 2)]
        public void BeConvertableFromMonthsToMonths(int periodInMonths, int expectedMonthsCount)
        {
            var currentDateTime = new LocalDateTime(2021, 08, 01, 00, 00);
            var period = Period.FromMonths(periodInMonths);

            var actualMonthsCount = Period
                .Between(currentDateTime, currentDateTime + period, PeriodUnits.Months)
                .Months;

            actualMonthsCount.Should().Be(expectedMonthsCount);
        }
    }
}
