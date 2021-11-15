using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentAssertions;
using FluentAssertions.Common;
using NUnit.Framework;

namespace TestNetCore
{
    [TestFixture]
    public class PropertyInfoShould
    {
        public class AccessToPropertyName
        {
            [Test]
            public void AsFastAsToUsualProperty()
            {
                const int iterationsCount = (int)10e7;
                var instance = new TestClass();
                var propertyInfo = typeof(TestClass).GetPropertyByName(nameof(TestClass.Name));

                var accessToPropertyNameTime = MeasureExecutionTime(
                    "Access to property name",
                    () => LoadTestAccessToPropertyName(propertyInfo, iterationsCount));
                
                var accessToUsualPropertyTime = MeasureExecutionTime(
                    "Access to usual property",
                    () => LoadTestAccessToUsualProperty(instance, iterationsCount));

                accessToPropertyNameTime.Should().BeLessOrEqualTo(accessToUsualPropertyTime * 1.5);
            }

            [Test]
            public void ViaNonCachedExpressionMuchSlowerThanAccessViaNameOf()
            {
                const int iterationsCount = (int)10e7 / 2;

                var accessToPropertyViaExpressionTime = MeasureExecutionTime(
                    "Access to property name via non cached expression",
                    () => LoadTestAccessToPropertyNameViaNonCachedExpression(
                        iterationsCount / 100));
                
                var accessToPropertyNameViaNameOfTime = MeasureExecutionTime(
                    "Access to property name via nameof()",
                    () => LoadTestAccessToPropertyNameViaNameOf(iterationsCount));

                accessToPropertyViaExpressionTime.Should().BeGreaterThan(accessToPropertyNameViaNameOfTime * 3.5);
            }

            [Test]
            public void ViaCachedExpressionSlowerThanAccessViaNameOf()
            {
                const int iterationsCount = (int)10e7 / 2;
                Expression<Func<TestClass, object>> expression = instance => instance.Name;

                var accessToPropertyViaExpressionTime = MeasureExecutionTime(
                    "Access to property name via cached expression",
                    () => LoadTestAccessToPropertyNameViaCachedExpression(expression, iterationsCount));
                
                var accessToPropertyNameViaNameOfTime = MeasureExecutionTime(
                    "Access to property name via nameof()",
                    () => LoadTestAccessToPropertyNameViaNameOf(iterationsCount));

                accessToPropertyViaExpressionTime.Should().BeGreaterThan(accessToPropertyNameViaNameOfTime * 3.5);
            }
        }

        [Test]
        public void WhenGotViaReflectionAndViaExpression_BeSame()
        {
            var propertyInfoViaReflection = typeof(TestClass).GetPropertyByName(nameof(TestClass.Name));
            
            Expression<Func<TestClass, object>> expression = instance => instance.Name;
            var propertyInfoViaExpression = (PropertyInfo)((MemberExpression)expression.Body).Member;

            propertyInfoViaReflection.Should().BeSameAs(propertyInfoViaExpression);
        }

        [Test]
        public void BeComparedWithItselfAsFastAsUsualNumbers()
        {
            const int iterationsCount = (int)10e7;
            var propertyInfo = typeof(TestClass).GetPropertyByName(nameof(TestClass.Name));

            var propertyInfosComparisonTime = MeasureExecutionTime(
                "Comparison of property infos",
                () => LoadTestPropertyInfosComparison(propertyInfo, iterationsCount));

            var numbersComparisonTime = MeasureExecutionTime(
                "Comparison of numbers",
                () => LoadTestNumbersComparison(iterationsCount));

            propertyInfosComparisonTime.Should().BeLessOrEqualTo(numbersComparisonTime * 1.75);
        }

        [Test]
        public void GetHashFasterThanStringHash()
        {
            const int iterationsCount = (int)10e7;
            var propertyInfo = typeof(TestClass).GetPropertyByName(nameof(TestClass.Name));

            var propertyInfoHashingTime = MeasureExecutionTime(
                "Hashing of property info",
                () => LoadTestPropertyInfoHashing(propertyInfo, iterationsCount));

            var stringHashingTime = MeasureExecutionTime(
                "Hashing of string",
                () => LoadTestStringHashing(iterationsCount));

            propertyInfoHashingTime.Should().BeLessOrEqualTo(stringHashingTime / 2);
        }

        [Test]
        public void AccessToPropertyViaReflectionSlowerThanDirectAccess()
        {
            const int iterationsCount = (int)10e7 / 2;
            var instance = new TestClass();
            var propertyInfo = typeof(TestClass).GetPropertyByName(nameof(TestClass.Name));

            var accessToPropertyViaReflectionTime = MeasureExecutionTime(
                "Access to property via reflection",
                () => LoadTestAccessToPropertyValueViaReflection(instance, propertyInfo, iterationsCount));

            var directAccessToPropertyTime = MeasureExecutionTime(
                "Direct access to property",
                () => LoadTestAccessToUsualProperty(instance, iterationsCount));

            accessToPropertyViaReflectionTime.Should().BeGreaterThan(directAccessToPropertyTime * 17.5);
        }

        private class TestClass
        {
            public string Name { get; } = "Name";
        }
    
        private static void LoadTestAccessToUsualProperty(TestClass instance, int iterationsCount)
        {
            for (var i = 0; i < iterationsCount; ++i)
                AccessValue(instance.Name);
        }

        private static void LoadTestAccessToPropertyName(PropertyInfo info, int iterationsCount)
        {
            for (var i = 0; i < iterationsCount; ++i)
                AccessValue(info.Name);
        }

        private static void LoadTestAccessToPropertyValueViaReflection(
            TestClass instance,
            PropertyInfo info,
            int iterationsCount)
        {
            for (var i = 0; i < iterationsCount; ++i)
                AccessValue(info.GetValue(instance));
        }

        private static void LoadTestAccessToPropertyNameViaNameOf(int iterationsCount)
        {
            for (var i = 0; i < iterationsCount; ++i)
                AccessValue(nameof(TestClass.Name));
        }

        private static void LoadTestAccessToPropertyNameViaNonCachedExpression(int iterationsCount)
        {
            for (var i = 0; i < iterationsCount; ++i)
            {
                Expression<Func<TestClass, object>> expression = instance => instance.Name;
                AccessValue(((MemberExpression)expression.Body).Member.Name);
            }
        }

        private static void LoadTestAccessToPropertyNameViaCachedExpression(
            Expression<Func<TestClass, object>> expression,
            int iterationsCount)
        {
            for (var i = 0; i < iterationsCount; ++i)
                AccessValue(((MemberExpression)expression.Body).Member.Name);
        }

        private static void LoadTestPropertyInfosComparison(
            PropertyInfo propertyInfo,
            int iterationsCount)
        {
            for (var i = 0; i < iterationsCount; ++i)
                AccessValue(propertyInfo == propertyInfo);
        }

        private static void LoadTestNumbersComparison(int iterationsCount)
        {
            var number1 = 3;
            var number2 = 6;
            for (var i = 0; i < iterationsCount; ++i)
            {
                unchecked
                {
                    number1 += 2;
                    number2 += 1;
                }
                AccessValue(number1 == number2);
            }
        }
        private static void LoadTestPropertyInfoHashing(
            PropertyInfo propertyInfo,
            int iterationsCount)
        {
            for (var i = 0; i < iterationsCount; ++i)
                AccessValue(propertyInfo.GetHashCode());
        }

        private static void LoadTestStringHashing(int iterationsCount)
        {
            const string @string = "TestString";
            for (var i = 0; i < iterationsCount; ++i)
                AccessValue(@string.GetHashCode());
        }

        private static TimeSpan MeasureExecutionTime(string name, Action action)
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();

            var executionTime = stopwatch.Elapsed;
            TestContext.WriteLine(name + ":" + Environment.NewLine + executionTime);
            return executionTime;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void AccessValue<T>(T value) { }
    }
}
