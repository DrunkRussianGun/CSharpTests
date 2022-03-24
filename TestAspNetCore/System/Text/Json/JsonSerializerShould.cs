using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using NUnit.Framework;

namespace TestNetCore.System.Text.Json
{
	public class JsonSerializerShould
	{
		[TestCaseSource(nameof(DeserializeReadOnlyAndWithPrivateSetters_TestCaseSource))]
		public void DeserializeReadOnlyAndWithPrivateSetters(object expected)
		{

			var serialized = JsonSerializer.SerializeToUtf8Bytes(expected);
			var actual = JsonSerializer.Deserialize(serialized, expected.GetType());
			
			actual.Should().BeEquivalentTo(expected);
		}

		public static IEnumerable<TestCaseData> DeserializeReadOnlyAndWithPrivateSetters_TestCaseSource()
		{
			var withConstructorForReadOnly = new ReadOnlyWithPrivateSetterAndConstructorForReadOnly("ReadOnly");
			withConstructorForReadOnly.PublicSetter("PrivateSetter");
			yield return new TestCaseData(withConstructorForReadOnly).SetName(
				"With constructor only for read-only property");

			var withConstructorForBoth = new ReadOnlyWithPrivateSetterAndConstructorForBoth(
				"ReadOnly",
				"PrivateSetter");
			yield return new TestCaseData(withConstructorForBoth).SetName(
				"With constructor for both properties");
		}

		public class ReadOnlyWithPrivateSetterAndConstructorForReadOnly
		{
			public string ReadOnly { get; }

			[JsonInclude]
			public string WithPrivateSetter { get; private set; }

			public ReadOnlyWithPrivateSetterAndConstructorForReadOnly(string readOnly)
			{
				ReadOnly = readOnly;
			}

			public void PublicSetter(string value) => WithPrivateSetter = value;
		}

		public class ReadOnlyWithPrivateSetterAndConstructorForBoth
		{
			public string ReadOnly { get; }

			[JsonInclude]
			public string WithPrivateSetter { get; private set; }

			public ReadOnlyWithPrivateSetterAndConstructorForBoth(string readOnly, string withPrivateSetter)
			{
				ReadOnly = readOnly;
				WithPrivateSetter = withPrivateSetter;
			}
		}
	}
}