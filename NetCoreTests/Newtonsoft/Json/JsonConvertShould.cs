using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace NetCoreTests.Newtonsoft.Json
{
    [TestFixture]
    public class JsonConvertShould
    {
        private class Properties
        {
            public string Property1 { get; set; }
            public string Property1WithGetterOnly => Property1;
            public string Property2WithGetterOnly => Property2;
            public string Property2 { get; set; }

            public Properties(string property1, string property2)
            {
                Property1 = property1;
                Property2 = property2;
            }
        }

        private class PropertiesWithGettersOnly
        {
            [JsonProperty]
            public string Property1WithGetterOnly { get; private set; }

            [JsonProperty]
            public string Property2WithGetterOnly { get; private set; }
        }

        [Test]
        public void SerializePropertiesWithGettersOnly()
        {
            var properties = new Properties("1", "2");

            var serializedProperties = JsonConvert.SerializeObject(properties);

            serializedProperties.Should().Contain(nameof(Properties.Property1WithGetterOnly));
            serializedProperties.Should().Contain(nameof(Properties.Property2WithGetterOnly));
        }

        [Test]
        public void SerializeAndDeserializeProperties()
        {
            var expectedProperties = new Properties("1", "2");

            var serializedProperties = JsonConvert.SerializeObject(expectedProperties);
            var actualProperties = JsonConvert.DeserializeObject<Properties>(serializedProperties);

            actualProperties.Should().BeEquivalentTo(expectedProperties);
        }

        [Test]
        public void IgnoreExtraProperties()
        {
            var expectedProperties = new Properties("1", "2");

            var serializedProperties = JsonConvert.SerializeObject(expectedProperties);
            var actualProperties = JsonConvert.DeserializeObject<PropertiesWithGettersOnly>(serializedProperties);

            actualProperties.Property1WithGetterOnly.Should().Be("1");
            actualProperties.Property2WithGetterOnly.Should().Be("2");
        }
    }
}
