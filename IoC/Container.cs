using System;
using System.Linq;

namespace IoC
{
    public class Container
    {
        public object GetInstance(Type type)
        {
            var constructor = type.GetConstructors().OrderByDescending(x => x.GetParameters().Length).First();
            var args = constructor.GetParameters().Select(x => GetInstance(x.ParameterType)).ToArray();
            return Activator.CreateInstance(type, args);
        }
    }
}
