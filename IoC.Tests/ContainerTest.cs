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

        public interface IMaterial
        {
          int Weight { get; }
        }

        public class Plastic : IMaterial
        {
            public int Weight => 12;
        }

        public interface IToy
        {
            int Size { get; }
        }

        public class Toy : IToy
        {
            public readonly IMaterial plastic;

            public int Size => 10;

            public Toy(IMaterial plastic)
            {
                this.plastic = plastic;
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
        public void InitializeFieldWithSpecifiedAType()
        {
            var subject = (B)container.GetInstance(typeof(B));
            Assert.IsInstanceOfType(subject.clasa, typeof(A));
        }

        [TestMethod]
        public void GenericGetInstanceMethod()
        {
            var subject = (B) container.GetInstance<B>();
            Assert.IsInstanceOfType(subject.clasa, typeof(A));
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception),"Already registered")]
        public void ThrowsExceptionIfTypeAlreadyRegistered()
        {
            container.RegisterType<IMaterial, Plastic>();
            container.RegisterType<IMaterial, Plastic>();
        }

        [TestMethod]
        public void GetsInstanceOfInterfaceType()
        {
            container.RegisterType<IMaterial, Plastic>();
            var subject = container.GetInstance<IMaterial>();

            Assert.IsInstanceOfType(subject, typeof(Plastic));
        }

        [TestMethod]
        public void InitializeFieldWithSpecifiedPlasticType()
        {
            container.RegisterType<IMaterial, Plastic>();
            container.RegisterType<IToy, Toy>();
            var subject = (Toy) container.GetInstance<IToy>();

            Assert.IsInstanceOfType(subject.plastic, typeof(Plastic));
        }
    }

    
}