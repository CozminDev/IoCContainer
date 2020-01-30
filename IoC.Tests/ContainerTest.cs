using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static IoC.Container;

namespace IoC.Tests
{
    [TestClass]
    public class ContainerTest
    {
        private Container container;
              
        [TestInitialize]
        public void Initialize()
        {
            container = new Container();
        }
           
        [TestMethod]
        [ExpectedException(typeof(Exception),"Already registered")]
        public void ThrowsExceptionIfTypeAlreadyRegistered()
        {
            container.RegisterType<IMaterial, Plastic>();
            container.RegisterType<IMaterial, Plastic>();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Type not registered")]
        public void ThrowsExceptionIfTypeNotRegistered()
        {
            container.Resolve<IMaterial>();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "ParameterS not registered")]
        public void ThrowsExceptionIfParametersNotRegistered()
        {
            container.RegisterType<IToy, Toy>();
            container.Resolve<IToy>();
        }

        [TestMethod]
        public void GetsInstanceOfInterfaceTypeSuccesfully()
        {
            container.RegisterType<IMaterial, Plastic>();
            var subject = container.Resolve<IMaterial>();

            Assert.IsInstanceOfType(subject, typeof(Plastic));
        }

        [TestMethod]
        public void InitializeFieldWithSpecifiedPlasticTypeSuccessfully()
        {
            container.RegisterType<IMaterial, Plastic>();
            container.RegisterType<IToy, Toy>();
            var subject = (Toy) container.Resolve<IToy>();

            Assert.IsInstanceOfType(subject.plastic, typeof(Plastic));
        }

        [TestMethod]
        public void RegistersGuidGeneratorSingletonLifetimeSuccessfully()
        {
            container.RegisterType<IGuidGenerator, GuidGenerator>(LifeTime.Singleton);

            var obj1 = (GuidGenerator)container.Resolve<IGuidGenerator>();
            var obj2 = (GuidGenerator)container.Resolve<IGuidGenerator>();

            Assert.AreEqual(obj1.Guid, obj2.Guid);
        }


        [TestMethod]
        public void RegisterToySingletonLifetimeSuccessfully()
        {
            container.RegisterType<IMaterial, Plastic>();
            container.RegisterType<IToy, Toy>(LifeTime.Singleton);

            var obj1 = (Toy)container.Resolve<IToy>();
            var obj2 = (Toy)container.Resolve<IToy>();

            Assert.AreEqual(obj1, obj2);
        }


        [TestMethod]
        public void RegistersGuidGeneratorTransientLifetimeSuccessfully()
        {
            container.RegisterType<IGuidGenerator, GuidGenerator>();

            var obj1 = (GuidGenerator)container.Resolve<IGuidGenerator>();
            var obj2 = (GuidGenerator)container.Resolve<IGuidGenerator>();

            Assert.AreNotEqual(obj1.Guid, obj2.Guid);
        }


        [TestMethod]
        public void RegisterToyTransientLifetimeSuccessfully()
        {
            container.RegisterType<IMaterial, Plastic>();
            container.RegisterType<IToy, Toy>();

            var obj1 = (Toy)container.Resolve<IToy>();
            var obj2 = (Toy)container.Resolve<IToy>();

            Assert.AreNotEqual(obj1, obj2);
        }


        public class A { };
        public class B
        {
            public readonly A a;

            public B(A a)
            {
                this.a = a;
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

        public interface IGuidGenerator
        {
            Guid Guid { get; set; }
        }

        public class GuidGenerator : IGuidGenerator
        {
            public Guid Guid { get; set; } = Guid.NewGuid();
        }
    }   
}