using FluentAssertions;

namespace NetCoreTests.System;

public class DateTimeShould
{
	[TestCase(DateTimeKind.Unspecified)]
	[TestCase(DateTimeKind.Utc)]
	public void ConvertToUtcDateTimeOffsetViaConstructor(DateTimeKind dateTimeKind)
	{
		var dateTime = new DateTime(2023, 03, 03, 01, 01, 01, dateTimeKind);
		var expected = new DateTimeOffset(2023, 03, 03, 01, 01, 01, TimeSpan.Zero);

		var actual = new DateTimeOffset(dateTime, TimeSpan.Zero);

		actual.Should().Be(expected);
	}

	[Test]
	public void ConvertLocalToUtcDateTimeOffset()
	{
		var dateTime = new DateTime(2023, 03, 03, 01, 01, 01, DateTimeKind.Local);
		var expected = new DateTimeOffset(2023, 03, 03, 01, 01, 01, TimeSpan.Zero);

		var actual = (DateTimeOffset)DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

		actual.Should().Be(expected);
	}

	[Test]
	public void ConvertToUtcDateTimeOffsetViaSpecifyKind([Values] DateTimeKind dateTimeKind)
	{
		var dateTime = new DateTime(2023, 03, 03, 01, 01, 01, dateTimeKind);
		var expected = new DateTimeOffset(2023, 03, 03, 01, 01, 01, TimeSpan.Zero);

		var actual = (DateTimeOffset)DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

		actual.Should().Be(expected);
	}

	[TestCase(DateTimeKind.Local)]
	[TestCase(DateTimeKind.Unspecified)]
	public void ConvertToLocalDateTimeOffsetViaOperator(DateTimeKind dateTimeKind)
	{
		var dateTime = new DateTime(2023, 03, 03, 01, 01, 01, dateTimeKind);
		var expected = new DateTimeOffset(2023, 03, 03, 01, 01, 01, TimeZoneInfo.Local.BaseUtcOffset);

		var actual = (DateTimeOffset)dateTime;

		actual.Should().Be(expected);
	}

	[Test]
	public void ConvertToUtcDateTimeOffsetViaOperator()
	{
		var dateTime = new DateTime(2023, 03, 03, 01, 01, 01, DateTimeKind.Utc);
		var expected = new DateTimeOffset(2023, 03, 03, 01, 01, 01, TimeSpan.Zero);

		var actual = (DateTimeOffset)dateTime;

		actual.Should().Be(expected);
	}
}
