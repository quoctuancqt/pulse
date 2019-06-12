namespace Pulse.Common.ResolverFactories
{
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class ResolverFactory
    {
        private static IKernel _kernel;

        public static void SetKernel(IKernel kernel)
        {
            _kernel = kernel;
        }

        public static T GetService<T>()
           where T : class
        {
            return (T)_kernel.GetService(typeof(T));
        }

        public static T CreateInstance<T>(string typeName)
            where T : class
        {

            Type type = Type.GetType(typeName);

            T instance = (T)Activator.CreateInstance(type);

            return instance;
        }

        public static T CreateInstance<T>(string typeName, params object[] args)
            where T : class
        {

            Type type = Type.GetType(typeName);

            T instance = (T)Activator.CreateInstance(type, args);

            return instance;
        }

    }
}
