using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IoC.Tests
{   
    [TestClass]
    public class ContainerTest
    {
        private Container container;

        public class A { };
        public class B
        {
            public readonly A clasa;

            public B(A clasa)
            {
                this.clasa = clasa;
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            container = new Container();
        }
        [TestMethod]
        public void CreatesAInstanceWithNoParams()
        {
            var subject = (A)container.GetInstance(typeof(A));
            Assert.IsInstanceOfType(subject, typeof(A));
        }

        [TestMethod]
        public void InitializeField()
        {
            var subject = (B)container.GetInstance(typeof(B));
            Assert.IsInstanceOfType(subject.clasa, typeof(A));
        }
    }
}
