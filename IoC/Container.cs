using System;
using System.Collections.Generic;
using System.Linq;

namespace IoC
{
    public class Container
    {
        private readonly Dictionary<Type, Func<object>> registeredTypes;

        public Container()
        {
            registeredTypes = new Dictionary<Type, Func<object>>();
        }

        public void RegisterType<TInterface,TType>() where TType : TInterface
        {
            if (registeredTypes.ContainsKey(typeof(TInterface)))
            {
                throw new Exception("Already registered");
            }

            registeredTypes.Add(typeof(TInterface), () => GetInstance<TType>());
        }

        public object GetInstance<T>()
        {
            return GetInstance(typeof(T));
        }

        public object GetInstance(Type type)
        {
            if(registeredTypes.ContainsKey(type))
            {
                return registeredTypes[type]();
            }

            var constructor = type.GetConstructors()
                .OrderByDescending(x => x.GetParameters().Length)
                .First();

            var args = constructor.GetParameters()
                .Select(x => GetInstance(x.ParameterType))
                .ToArray();

            return Activator.CreateInstance(type, args);
        }
    }
}