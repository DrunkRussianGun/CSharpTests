using System;
using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using NUnit.Framework;

// ReSharper disable LocalVariableHidesMember

namespace NetCoreTests.System
{
    [TestFixture]
    public class FormatDateTimeShould
    {
        private static readonly DateTime dateTime = new DateTime(2020, 02, 29, 23, 59, 59);

        [Test]
        public void ReturnExpectedDateTimeString_ForGivenFormat()
        {
            var dateTimeString = dateTime.ToString("yyyy-MM-dd_HH-mm-ss");

            dateTimeString.Should().Be("2020-02-29_23-59-59");
        }

        [TestCaseSource(nameof(ReturnExpectedDateTimeString_TestCaseSource))]
        public void ReturnExpectedDateTimeString_ForGivenFormatProvider(
            IFormatProvider format,
            string expectedDateTimeString)
        {
            var dateTimeString = dateTime.ToString(format);

            dateTimeString.Should().Be(expectedDateTimeString);
        }

        public static IEnumerable<TestCaseData> ReturnExpectedDateTimeString_TestCaseSource()
        {
            yield return new TestCaseData(
                    CultureInfo.InvariantCulture,
                    "02/29/2020 23:59:59")
                .SetName("ReturnExpectedDateTimeString_ForInvariantCultureInfo");
            yield return new TestCaseData(
                    CultureInfo.GetCultureInfoByIetfLanguageTag("ru"),
                    "29.02.2020 23:59:59")
                .SetName("ReturnExpectedDateTimeString_ForRussianCultureInfo");
        }

        [Test]
        public void ParseDateTimeString()
        {
            var dateTime = DateTime.Parse("04/18/2021 05:02:21", CultureInfo.InvariantCulture);

            dateTime.Should().Be(new DateTime(2021, 04, 18, 05, 02, 21));
        }
    }
}
