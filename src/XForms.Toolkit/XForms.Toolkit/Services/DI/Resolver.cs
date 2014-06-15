using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Toolkit.Services
{
    /// <summary>
    /// Wrapper for IResolver instance for quick access
    /// </summary>
    public static class Resolver
    {
        private static IResolver instance;

        /// <summary>
        /// Sets the resolver instance
        /// </summary>
        /// <param name="resolver">Instance of IResolver implementation</param>
        /// <exception cref="InvalidOperationException">Instance can only be set once to prevent mixups.</exception>
        public static void SetResolver(IResolver resolver)
        {
            if (instance != null)
            {
                throw new InvalidOperationException("IResolver can only be set once.");
            }

            instance = resolver;
        }

        /// <summary>
        /// Resolve a dependency
        /// </summary>
        /// <typeparam name="T">Type of instance to get</typeparam>
        /// <returns>An instance of {T} if successful, otherwise null.</returns>
        public static T Resolve<T>() where T : class
        {
            return instance.Resolve<T>();
        }

        /// <summary>
        /// Resolve a dependency by type
        /// </summary>
        /// <param name="type">Type of object</param>
        /// <returns>An instance to type if found as <see cref="object"/>, otherwise null.</returns>
        public static object Resolve(Type type)
        {
            return instance.Resolve(type);
        }

        /// <summary>
        /// Resolve a dependency
        /// </summary>
        /// <typeparam name="T">Type of instance to get</typeparam>
        /// <returns>All instances of {T} if successful, otherwise null.</returns>
        public static IEnumerable<T> ResolveAll<T>() where T : class
        {
            return instance.ResolveAll<T>();
        }

        /// <summary>
        /// Resolve a dependency by type
        /// </summary>
        /// <param name="type">Type of object</param>
        /// <returns>All instances of type if found as <see cref="object"/>, otherwise null.</returns>
        public static IEnumerable<object> ResolveAll(Type type)
        {
            return instance.ResolveAll(type);
        }
    }
}
