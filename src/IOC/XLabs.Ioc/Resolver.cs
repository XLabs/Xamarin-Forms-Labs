namespace XLabs.Ioc
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Wrapper for IResolver instance for quick access.
    /// </summary>
    public static class Resolver
    {
        /// <summary>
        /// The <see cref="IResolver"/> instance.
        /// </summary>
        private static IResolver instance;

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <exception cref="InvalidOperationException">IResolver instance has not been set.</exception>
        /// <exception cref="InvalidOperationException">Instance can only be set once to prevent mix-ups.</exception>
        private static IResolver Instance
        {
            get
            {
                if (!IsSet)
                {
                    throw new InvalidOperationException("IResolver has not been set. Please set it by calling Resolver.SetResolver(resolver) method.");
                }

                return instance;
            }
            set
            {
                if (IsSet)
                {
                    throw new InvalidOperationException("IResolver can only be set once.");
                }

                instance = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether resolver has been set.
        /// </summary>
        public static bool IsSet
        {
            get { return instance != null; }
        }

        /// <summary>
        /// Sets the resolver instance.
        /// </summary>
        /// <param name="resolver">Instance of IResolver implementation.</param>
        /// <exception cref="InvalidOperationException">Instance can only be set once to prevent mix-ups.</exception>
        public static void SetResolver(IResolver resolver)
        {
            Instance = resolver;
        }

        /// <summary>
        /// Resolve a dependency.
        /// </summary>
        /// <typeparam name="T">Type of instance to get.</typeparam>
        /// <returns>An instance of {T} if successful, otherwise null.</returns>
        /// <exception cref="InvalidOperationException">IResolver instance has not been set.</exception>
        public static T Resolve<T>() where T : class
        {
            return Instance.Resolve<T>();
        }

        /// <summary>
        /// Resolve a dependency by type.
        /// </summary>
        /// <param name="type">Type of object.</param>
        /// <returns>An instance to type if found as <see cref="object"/>, otherwise null.</returns>
        /// <exception cref="InvalidOperationException">IResolver instance has not been set.</exception>
        public static object Resolve(Type type)
        {
            return Instance.Resolve(type);
        }

        /// <summary>
        /// Resolve a dependency.
        /// </summary>
        /// <typeparam name="T">Type of instance to get.</typeparam>
        /// <returns>All instances of {T} if successful, otherwise null.</returns>
        /// <exception cref="InvalidOperationException">IResolver instance has not been set.</exception>
        public static IEnumerable<T> ResolveAll<T>() where T : class
        {
            return Instance.ResolveAll<T>();
        }

        /// <summary>
        /// Resolve a dependency by type.
        /// </summary>
        /// <param name="type">Type of object.</param>
        /// <returns>All instances of type if found as <see cref="object"/>, otherwise null.</returns>
        /// <exception cref="InvalidOperationException">IResolver instance has not been set.</exception>
        public static IEnumerable<object> ResolveAll(Type type)
        {
            return Instance.ResolveAll(type);
        }

        /// <summary>
        /// Determines whether the specified type is registed.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is registed; otherwise, <c>false</c>.</returns>
        public static bool IsRegistered(Type type)
        {
            return Instance.IsRegistered(type);
        }

        /// <summary>
        /// Determines whether this instance is registered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns><c>true</c> if this instance is registered; otherwise, <c>false</c>.</returns>
        public static bool IsRegistered<T>() where T : class
        {
            return Instance.IsRegistered<T>();
        }
    }
}
