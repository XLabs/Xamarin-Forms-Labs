using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Toolkit.Services
{
    /// <summary>
    /// Simple dependency container implementation
    /// </summary>
    public class SimpleContainer : IDependencyContainer
    {
        private readonly IResolver resolver;
        private readonly Dictionary<Type, List<object>> services;
        private readonly Dictionary<Type, List<Func<IResolver, object>>> registeredServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleContainer" /> class.
        /// </summary>
        public SimpleContainer()
        {
            this.resolver = new Resolver(this.ResolveAll);
            this.services = new Dictionary<Type, List<object>>();
            this.registeredServices = new Dictionary<Type, List<Func<IResolver, object>>>();
        }

        #region IDependencyContainer Members
        /// <summary>
        /// Gets the resolver from the container
        /// </summary>
        /// <returns>An instance of <see cref="IResolver"/></returns>
        public IResolver GetResolver()
        {
            return this.resolver;
        }

        /// <summary>
        /// Registers an instance of T to be stored in the container.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <param name="instance">Instance of type T.</param>
        /// <returns>An instance of <see cref="SimpleContainer"/></returns>
        public IDependencyContainer Register<T>(T instance) where T : class
        {
            var type = typeof(T);
            List<object> list;

            if (!this.services.TryGetValue(type, out list))
            {
                list = new List<object>();
                this.services.Add(type, list);
            }

            list.Add(instance);
            return this;
        }

        /// <summary>
        /// Registers a type to instantiate for type T.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <typeparam name="TImpl">Type to register for instantiation.</typeparam>
        /// <returns>An instance of <see cref="SimpleContainer"/></returns>
        public IDependencyContainer Register<T, TImpl>()
            where T : class
            where TImpl : T
        {
            return this.Register<T>(t => Activator.CreateInstance<TImpl>() as T);
        }

        /// <summary>
        /// Tries to register a type
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <param name="type">Type of implementation</param>
        /// <returns>An instance of <see cref="SimpleContainer"/></returns>
        public IDependencyContainer Register<T>(Type type) where T : class
        {
            return this.Register(typeof(T), type);
        }

        /// <summary>
        /// Tries to register a type
        /// </summary>
        /// <param name="type">Type to register.</param>
        /// <param name="impl">Type that implements registered type.</param>
        /// <returns>An instance of <see cref="SimpleContainer"/></returns>
        public IDependencyContainer Register(Type type, Type impl)
        {
            List<Func<IResolver, object>> list;
            if (!this.registeredServices.TryGetValue(type, out list))
            {
                list = new List<Func<IResolver, object>>();
                this.registeredServices.Add(type, list);
            }

            list.Add(t => Activator.CreateInstance(impl));

            return this;
        }

        /// <summary>
        /// Registers a function which returns an instance of type T.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <param name="func">Function which returns an instance of T.</param>
        /// <returns>An instance of <see cref="SimpleContainer"/></returns>
        public IDependencyContainer Register<T>(Func<IResolver, T> func) where T : class
        {
            var type = typeof(T);
            List<Func<IResolver, object>> list;
            if (!this.registeredServices.TryGetValue(type, out list))
            {
                list = new List<Func<IResolver, object>>();
                this.registeredServices.Add(type, list);
            }

            list.Add(func);

            return this;
        }

        private IEnumerable<object> ResolveAll(Type type)
        {
            List<object> list;
            if (this.services.TryGetValue(type, out list))
            {
                foreach (var service in list)
                {
                    yield return service;
                }
            }

            List<Func<IResolver, object>> getter;
            if (this.registeredServices.TryGetValue(type, out getter))
            {
                foreach (var serviceFunc in getter)
                {
                    yield return serviceFunc(this.resolver);
                }
            }
        }

        #endregion

        private class Resolver : IResolver
        {
            private Func<Type, IEnumerable<object>> resolveObjectDelegate;

            internal Resolver(Func<Type, IEnumerable<object>> resolveObjectDelegate)
            {
                this.resolveObjectDelegate = resolveObjectDelegate;
            }

            #region IResolver Members

            public T Resolve<T>() where T : class
            {
                return this.ResolveAll<T>().FirstOrDefault() as T;
            }

            public object Resolve(Type type)
            {
                return this.ResolveAll(type).FirstOrDefault();
            }

            public IEnumerable<T> ResolveAll<T>() where T : class
            {
                return this.resolveObjectDelegate(typeof(T)).Cast<T>();
            }

            public IEnumerable<object> ResolveAll(Type type)
            {
                return this.resolveObjectDelegate(type);
            }

            #endregion
        }
    }
}
