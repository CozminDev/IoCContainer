using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace IoC
{
    public class Container
    {
        private readonly Dictionary<Type, Lazy<TypeDefinition>> registeredTypes;

        public Container()
        {
            registeredTypes = new Dictionary<Type, Lazy<TypeDefinition>>();
        }

        public void RegisterType<TInterface, TType>(LifeTime lifeTime = LifeTime.Transient) where TType : TInterface
        {
            if (registeredTypes.ContainsKey(typeof(TInterface)))
            {
                throw new Exception("Already registered");
            }

            registeredTypes.Add(typeof(TInterface), new Lazy<TypeDefinition>(() => GetTypeDefinition(typeof(TType), lifeTime)));
        }
     
        public object Resolve<T>()
        {
            if (registeredTypes.ContainsKey(typeof(T)))
            {
                return registeredTypes[typeof(T)].Value.Constructor();
            }
            else
            {
                throw new Exception("Type not registered");
            }
        }
       
        private TypeDefinition GetTypeDefinition(Type type, LifeTime lifeTime)
        {
            if (registeredTypes.ContainsKey(type))
            {
                return registeredTypes[type].Value;
            }

            TypeDefinition registeredType = new TypeDefinition();
            registeredType.Type = type;
            registeredType.LifeTime = lifeTime;

            ConstructorInfo constructor = type.GetConstructors()
              .OrderByDescending(x => x.GetParameters().Length)
              .First();

            object[] args = constructor.GetParameters()
                .Select(x => GetInstance(x.ParameterType))
                .ToArray();

            Func<object> lambda = null;

            if(lifeTime == LifeTime.Transient)
            {
                Expression objArguments = Expression.Constant(args);
                Expression objType = Expression.Constant(type);
                Expression call = Expression.Call(typeof(Activator).GetMethod("CreateInstance", new[] { typeof(Type), typeof(object[]) } ), objType, objArguments);

                lambda = Expression.Lambda<Func<object>>(call).Compile();
            }

            if (lifeTime == LifeTime.Singleton)
            {
                object obj = Activator.CreateInstance(type, args);
                lambda = () => obj;
            }
          
            registeredType.Constructor = lambda;

            return registeredType;
        }

        private object GetInstance(Type type)
        {
            if (registeredTypes.ContainsKey(type))
            {
                return registeredTypes[type].Value.Constructor();
            }
            else
            {
                throw new Exception("Parameters not registered");
            }
        }

        public enum LifeTime
        {
            Transient = 0,
            Singleton = 1
        }

        public class TypeDefinition
        {
            public Type Type { get; set; }
            public LifeTime LifeTime { get; set; }
            public Func<object> Constructor { get; set; }
        }
    }
}