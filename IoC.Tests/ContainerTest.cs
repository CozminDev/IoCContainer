using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IoC.Tests
{
    [TestClass]
    public class ContainerTest
    {
        private Container container;
        class A { };

        [TestInitialize]
        public void Initialize()
        {
            container = new Container();
        }
        [TestMethod]
        public void CreatesAInstanceWithNoParams()
        {
            var subject = (A) container.GetInstance(typeof(A));
            Assert.IsInstanceOfType(subject, typeof(A));
        }
    }
}
