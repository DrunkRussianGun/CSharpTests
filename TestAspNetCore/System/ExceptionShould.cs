using System;
using FluentAssertions;
using NUnit.Framework;

namespace TestNetCore.System
{
    [TestFixture]
    public class ExceptionShould
    {
        private class Thrower
        {
            public Thrower(Exception exception) => throw exception;
        }
        
        [Test]
        public void WhenOccuredInConstructor_ContainConstructorInStacktrace()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action throwingConstructor = () => new Thrower(new ArithmeticException());

            throwingConstructor.Should().Throw<ArithmeticException>()
                .Which.StackTrace.Should().Contain("Thrower..ctor");
        }
    }
}
