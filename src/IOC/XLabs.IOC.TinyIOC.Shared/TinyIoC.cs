// ***********************************************************************
// <copyright file="TinyIoC.cs" company="">
//     Copyright © 2014
// </copyright>
// <summary></summary>
// ***********************************************************************

#region Preprocessor Directives
// Uncomment this line if you want the container to automatically
// register the TinyMessenger messenger/event aggregator
//#define TINYMESSENGER

// Preprocessor directives for enabling/disabling functionality
// depending on platform features. If the platform has an appropriate
// #DEFINE then these should be set automatically below.
#define EXPRESSIONS                         // Platform supports System.Linq.Expressions
#define APPDOMAIN_GETASSEMBLIES             // Platform supports getting all assemblies from the AppDomain object
#define UNBOUND_GENERICS_GETCONSTRUCTORS    // Platform supports GetConstructors on unbound generic types
#define GETPARAMETERS_OPEN_GENERICS         // Platform supports GetParameters on open generics
#define RESOLVE_OPEN_GENERICS               // Platform supports resolving open generics

//// NETFX_CORE
//#if NETFX_CORE
//#endif

// CompactFramework / Windows Phone 7
// By default does not support System.Linq.Expressions.
// AppDomain object does not support enumerating all assemblies in the app domain.
#if PocketPC || WINDOWS_PHONE
#undef EXPRESSIONS
#undef APPDOMAIN_GETASSEMBLIES
#undef UNBOUND_GENERICS_GETCONSTRUCTORS
#endif

// PocketPC has a bizarre limitation on enumerating parameters on unbound generic methods.
// We need to use a slower workaround in that case.
#if PocketPC
#undef GETPARAMETERS_OPEN_GENERICS
#undef RESOLVE_OPEN_GENERICS
#endif

#if SILVERLIGHT
#undef APPDOMAIN_GETASSEMBLIES
#endif

#if NETFX_CORE
#undef APPDOMAIN_GETASSEMBLIES
#undef RESOLVE_OPEN_GENERICS
#endif

#endregion

namespace TinyIoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

#if EXPRESSIONS
    using System.Linq.Expressions;
#endif

#if NETFX_CORE
    using System.Threading.Tasks;
    using Windows.Storage.Search;
    using Windows.Storage;
    using Windows.UI.Xaml.Shapes;
#endif

    #region SafeDictionary
    /// <summary>
    /// Class SafeDictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    /// <typeparam name="TValue">The type of the t value.</typeparam>
    public class SafeDictionary<TKey, TValue> : IDisposable
    {
        /// <summary>
        /// The _ padlock
        /// </summary>
        private readonly object _Padlock = new object();
        /// <summary>
        /// The _ dictionary
        /// </summary>
        private readonly Dictionary<TKey, TValue> _Dictionary = new Dictionary<TKey, TValue>();


        /// <summary>
        /// Set key
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>TValue.</returns>
        public TValue this[TKey key]
        {
            set
            {
                lock (_Padlock)
                {
                    TValue current;
                    if (_Dictionary.TryGetValue(key, out current))
                    {
                        var disposable = current as IDisposable;

                        if (disposable != null)
                            disposable.Dispose();
                    }

                    _Dictionary[key] = value;
                }
            }
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_Padlock)
            {
                return _Dictionary.TryGetValue(key, out value);
            }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Remove(TKey key)
        {
            lock (_Padlock)
            {
                return _Dictionary.Remove(key);
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            lock (_Padlock)
            {
                _Dictionary.Clear();
            }
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public IEnumerable<TKey> Keys
        {
            get
            {
                return _Dictionary.Keys;
            }
        }
        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (_Padlock)
            {
                var disposableItems = from item in _Dictionary.Values
                                      where item is IDisposable
                                      select item as IDisposable;

                foreach (var item in disposableItems)
                {
                    item.Dispose();
                }
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
    #endregion

    #region Extensions
    /// <summary>
    /// Class AssemblyExtensions.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Safes the get types.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Type[].</returns>
        public static Type[] SafeGetTypes(this Assembly assembly)
        {
            Type[] assemblies;

            try
            {
                assemblies = assembly.GetTypes();
            }
            catch (System.IO.FileNotFoundException)
            {
                assemblies = new Type[] { };
            }
            catch (NotSupportedException)
            {
                assemblies = new Type[] { };
            }
#if !NETFX_CORE
            catch (ReflectionTypeLoadException e)
            {
                assemblies = e.Types.Where(t => t != null).ToArray();
            }
#endif
            return assemblies;
        }
    }

    /// <summary>
    /// Class TypeExtensions.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// The _generic method cache
        /// </summary>
        private static SafeDictionary<GenericMethodCacheKey, MethodInfo> _genericMethodCache;

        /// <summary>
        /// Initializes static members of the <see cref="TypeExtensions"/> class.
        /// </summary>
        static TypeExtensions()
        {
            _genericMethodCache = new SafeDictionary<GenericMethodCacheKey, MethodInfo>();
        }

        //#if NETFX_CORE
        //		/// <summary>
        //		/// Gets a generic method from a type given the method name, generic types and parameter types
        //		/// </summary>
        //		/// <param name="sourceType">Source type</param>
        //		/// <param name="methodName">Name of the method</param>
        //		/// <param name="genericTypes">Generic types to use to make the method generic</param>
        //		/// <param name="parameterTypes">Method parameters</param>
        //		/// <returns>MethodInfo or null if no matches found</returns>
        //		/// <exception cref="System.Reflection.AmbiguousMatchException"/>
        //		/// <exception cref="System.ArgumentException"/>
        //		public static MethodInfo GetGenericMethod(this Type sourceType, string methodName, Type[] genericTypes, Type[] parameterTypes)
        //		{
        //			MethodInfo method;
        //			var cacheKey = new GenericMethodCacheKey(sourceType, methodName, genericTypes, parameterTypes);

        //			// Shouldn't need any additional locking
        //			// we don't care if we do the method info generation
        //			// more than once before it gets cached.
        //			if (!_genericMethodCache.TryGetValue(cacheKey, out method))
        //			{
        //				method = GetMethod(sourceType, methodName, genericTypes, parameterTypes);
        //				_genericMethodCache[cacheKey] = method;
        //			}

        //			return method;
        //		}
        //#else
        /// <summary>
        /// Gets a generic method from a type given the method name, binding flags, generic types and parameter types
        /// </summary>
        /// <param name="sourceType">Source type</param>
        /// <param name="bindingFlags">Binding flags</param>
        /// <param name="methodName">Name of the method</param>
        /// <param name="genericTypes">Generic types to use to make the method generic</param>
        /// <param name="parameterTypes">Method parameters</param>
        /// <returns>MethodInfo or null if no matches found</returns>
        /// <exception cref="System.Reflection.AmbiguousMatchException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        public static MethodInfo GetGenericMethod(this Type sourceType, BindingFlags bindingFlags, string methodName, Type[] genericTypes, Type[] parameterTypes)
        {
            MethodInfo method;
            var cacheKey = new GenericMethodCacheKey(sourceType, methodName, genericTypes, parameterTypes);

            // Shouldn't need any additional locking
            // we don't care if we do the method info generation
            // more than once before it gets cached.
            if (!_genericMethodCache.TryGetValue(cacheKey, out method))
            {
                method = GetMethod(sourceType, bindingFlags, methodName, genericTypes, parameterTypes);
                _genericMethodCache[cacheKey] = method;
            }

            return method;
        }
        //#endif

#if NETFX_CORE
        private static MethodInfo GetMethod(Type sourceType, BindingFlags flags, string methodName, Type[] genericTypes, Type[] parameterTypes)
        {
            var methods =
                sourceType.GetMethods(flags).Where(
                    mi => string.Equals(methodName, mi.Name, StringComparison.Ordinal)).Where(
                        mi => mi.ContainsGenericParameters).Where(mi => mi.GetGenericArguments().Length == genericTypes.Length).
                    Where(mi => mi.GetParameters().Length == parameterTypes.Length).Select(
                        mi => mi.MakeGenericMethod(genericTypes)).Where(
                            mi => mi.GetParameters().Select(pi => pi.ParameterType).SequenceEqual(parameterTypes)).ToList();

            if (methods.Count > 1)
            {
                throw new AmbiguousMatchException();
            }

            return methods.FirstOrDefault();
        }
#else
        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="genericTypes">The generic types.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <returns>MethodInfo.</returns>
        /// <exception cref="System.Reflection.AmbiguousMatchException"></exception>
        private static MethodInfo GetMethod(Type sourceType, BindingFlags bindingFlags, string methodName, Type[] genericTypes, Type[] parameterTypes)
        {
#if GETPARAMETERS_OPEN_GENERICS
            var methods =
                sourceType.GetMethods(bindingFlags).Where(
                    mi => string.Equals(methodName, mi.Name, StringComparison.Ordinal)).Where(
                        mi => mi.ContainsGenericParameters).Where(mi => mi.GetGenericArguments().Length == genericTypes.Length).
                    Where(mi => mi.GetParameters().Length == parameterTypes.Length).Select(
                        mi => mi.MakeGenericMethod(genericTypes)).Where(
                            mi => mi.GetParameters().Select(pi => pi.ParameterType).SequenceEqual(parameterTypes)).ToList();
#else
            var validMethods =  from method in sourceType.GetMethods(bindingFlags)
                                where method.Name == methodName
                                where method.IsGenericMethod
                                where method.GetGenericArguments().Length == genericTypes.Length
                                let genericMethod = method.MakeGenericMethod(genericTypes)
                                where genericMethod.GetParameters().Count() == parameterTypes.Length
                                where genericMethod.GetParameters().Select(pi => pi.ParameterType).SequenceEqual(parameterTypes)
                                select genericMethod;

            var methods = validMethods.ToList();
#endif
            if (methods.Count > 1)
            {
                throw new AmbiguousMatchException();
            }

            return methods.FirstOrDefault();
        }
#endif

        /// <summary>
        /// Class GenericMethodCacheKey. This class cannot be inherited.
        /// </summary>
        private sealed class GenericMethodCacheKey
        {
            /// <summary>
            /// The _source type
            /// </summary>
            private readonly Type _sourceType;

            /// <summary>
            /// The _method name
            /// </summary>
            private readonly string _methodName;

            /// <summary>
            /// The _generic types
            /// </summary>
            private readonly Type[] _genericTypes;

            /// <summary>
            /// The _parameter types
            /// </summary>
            private readonly Type[] _parameterTypes;

            /// <summary>
            /// The _hash code
            /// </summary>
            private readonly int _hashCode;

            /// <summary>
            /// Initializes a new instance of the <see cref="GenericMethodCacheKey"/> class.
            /// </summary>
            /// <param name="sourceType">Type of the source.</param>
            /// <param name="methodName">Name of the method.</param>
            /// <param name="genericTypes">The generic types.</param>
            /// <param name="parameterTypes">The parameter types.</param>
            public GenericMethodCacheKey(Type sourceType, string methodName, Type[] genericTypes, Type[] parameterTypes)
            {
                _sourceType = sourceType;
                _methodName = methodName;
                _genericTypes = genericTypes;
                _parameterTypes = parameterTypes;
                _hashCode = GenerateHashCode();
            }

            /// <summary>
            /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
            /// </summary>
            /// <param name="obj">The object to compare with the current object.</param>
            /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
            public override bool Equals(object obj)
            {
                var cacheKey = obj as GenericMethodCacheKey;
                if (cacheKey == null)
                    return false;

                if (_sourceType != cacheKey._sourceType)
                    return false;

                if (!String.Equals(_methodName, cacheKey._methodName, StringComparison.Ordinal))
                    return false;

                if (_genericTypes.Length != cacheKey._genericTypes.Length)
                    return false;

                if (_parameterTypes.Length != cacheKey._parameterTypes.Length)
                    return false;

                for (int i = 0; i < _genericTypes.Length; ++i)
                {
                    if (_genericTypes[i] != cacheKey._genericTypes[i])
                        return false;
                }

                for (int i = 0; i < _parameterTypes.Length; ++i)
                {
                    if (_parameterTypes[i] != cacheKey._parameterTypes[i])
                        return false;
                }

                return true;
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
            public override int GetHashCode()
            {
                return _hashCode;
            }

            /// <summary>
            /// Generates the hash code.
            /// </summary>
            /// <returns>System.Int32.</returns>
            private int GenerateHashCode()
            {
                unchecked
                {
                    var result = _sourceType.GetHashCode();

                    result = (result * 397) ^ _methodName.GetHashCode();

                    for (int i = 0; i < _genericTypes.Length; ++i)
                    {
                        result = (result * 397) ^ _genericTypes[i].GetHashCode();
                    }

                    for (int i = 0; i < _parameterTypes.Length; ++i)
                    {
                        result = (result * 397) ^ _parameterTypes[i].GetHashCode();
                    }

                    return result;
                }
            }
        }

    }

    // @mbrit - 2012-05-22 - shim for ForEach call on List<T>...
#if NETFX_CORE
    internal static class ListExtender
    {
        internal static void ForEach<T>(this List<T> list, Action<T> callback)
        {
            foreach (T obj in list)
                callback(obj);
        }
    }
#endif

    #endregion

    #region TinyIoC Exception Types
    /// <summary>
    /// Class TinyIoCResolutionException.
    /// </summary>
    public class TinyIoCResolutionException : Exception
    {
        /// <summary>
        /// The erro r_ text
        /// </summary>
        private const string ERROR_TEXT = "Unable to resolve type: {0}";

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCResolutionException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public TinyIoCResolutionException(Type type)
            : base(String.Format(ERROR_TEXT, type.FullName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCResolutionException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="innerException">The inner exception.</param>
        public TinyIoCResolutionException(Type type, Exception innerException)
            : base(String.Format(ERROR_TEXT, type.FullName), innerException)
        {
        }
    }

    /// <summary>
    /// Class TinyIoCRegistrationTypeException.
    /// </summary>
    public class TinyIoCRegistrationTypeException : Exception
    {
        /// <summary>
        /// The registe r_ erro r_ text
        /// </summary>
        private const string REGISTER_ERROR_TEXT = "Cannot register type {0} - abstract classes or interfaces are not valid implementation types for {1}.";

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCRegistrationTypeException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="factory">The factory.</param>
        public TinyIoCRegistrationTypeException(Type type, string factory)
            : base(String.Format(REGISTER_ERROR_TEXT, type.FullName, factory))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCRegistrationTypeException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="innerException">The inner exception.</param>
        public TinyIoCRegistrationTypeException(Type type, string factory, Exception innerException)
            : base(String.Format(REGISTER_ERROR_TEXT, type.FullName, factory), innerException)
        {
        }
    }

    /// <summary>
    /// Class TinyIoCRegistrationException.
    /// </summary>
    public class TinyIoCRegistrationException : Exception
    {
        /// <summary>
        /// The conver t_ erro r_ text
        /// </summary>
        private const string CONVERT_ERROR_TEXT = "Cannot convert current registration of {0} to {1}";
        /// <summary>
        /// The generi c_ constrain t_ erro r_ text
        /// </summary>
        private const string GENERIC_CONSTRAINT_ERROR_TEXT = "Type {1} is not valid for a registration of type {0}";

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCRegistrationException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        public TinyIoCRegistrationException(Type type, string method)
            : base(String.Format(CONVERT_ERROR_TEXT, type.FullName, method))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCRegistrationException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="innerException">The inner exception.</param>
        public TinyIoCRegistrationException(Type type, string method, Exception innerException)
            : base(String.Format(CONVERT_ERROR_TEXT, type.FullName, method), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCRegistrationException"/> class.
        /// </summary>
        /// <param name="registerType">Type of the register.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        public TinyIoCRegistrationException(Type registerType, Type implementationType)
            : base(String.Format(GENERIC_CONSTRAINT_ERROR_TEXT, registerType.FullName, implementationType.FullName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCRegistrationException"/> class.
        /// </summary>
        /// <param name="registerType">Type of the register.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="innerException">The inner exception.</param>
        public TinyIoCRegistrationException(Type registerType, Type implementationType, Exception innerException)
            : base(String.Format(GENERIC_CONSTRAINT_ERROR_TEXT, registerType.FullName, implementationType.FullName), innerException)
        {
        }
    }

    /// <summary>
    /// Class TinyIoCWeakReferenceException.
    /// </summary>
    public class TinyIoCWeakReferenceException : Exception
    {
        /// <summary>
        /// The erro r_ text
        /// </summary>
        private const string ERROR_TEXT = "Unable to instantiate {0} - referenced object has been reclaimed";

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCWeakReferenceException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public TinyIoCWeakReferenceException(Type type)
            : base(String.Format(ERROR_TEXT, type.FullName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCWeakReferenceException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="innerException">The inner exception.</param>
        public TinyIoCWeakReferenceException(Type type, Exception innerException)
            : base(String.Format(ERROR_TEXT, type.FullName), innerException)
        {
        }
    }

    /// <summary>
    /// Class TinyIoCConstructorResolutionException.
    /// </summary>
    public class TinyIoCConstructorResolutionException : Exception
    {
        /// <summary>
        /// The erro r_ text
        /// </summary>
        private const string ERROR_TEXT = "Unable to resolve constructor for {0} using provided Expression.";

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCConstructorResolutionException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public TinyIoCConstructorResolutionException(Type type)
            : base(String.Format(ERROR_TEXT, type.FullName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCConstructorResolutionException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="innerException">The inner exception.</param>
        public TinyIoCConstructorResolutionException(Type type, Exception innerException)
            : base(String.Format(ERROR_TEXT, type.FullName), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public TinyIoCConstructorResolutionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TinyIoCConstructorResolutionException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Class TinyIoCAutoRegistrationException.
    /// </summary>
    public class TinyIoCAutoRegistrationException : Exception
    {
        /// <summary>
        /// The erro r_ text
        /// </summary>
        private const string ERROR_TEXT = "Duplicate implementation of type {0} found ({1}).";

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCAutoRegistrationException"/> class.
        /// </summary>
        /// <param name="registerType">Type of the register.</param>
        /// <param name="types">The types.</param>
        public TinyIoCAutoRegistrationException(Type registerType, IEnumerable<Type> types)
            : base(String.Format(ERROR_TEXT, registerType, GetTypesString(types)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCAutoRegistrationException"/> class.
        /// </summary>
        /// <param name="registerType">Type of the register.</param>
        /// <param name="types">The types.</param>
        /// <param name="innerException">The inner exception.</param>
        public TinyIoCAutoRegistrationException(Type registerType, IEnumerable<Type> types, Exception innerException)
            : base(String.Format(ERROR_TEXT, registerType, GetTypesString(types)), innerException)
        {
        }

        /// <summary>
        /// Gets the types string.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns>System.String.</returns>
        private static string GetTypesString(IEnumerable<Type> types)
        {
            var typeNames = from type in types
                            select type.FullName;

            return string.Join(",", typeNames.ToArray());
        }
    }
    #endregion

    #region Public Setup / Settings Classes
    /// <summary>
    /// Name/Value pairs for specifying "user" parameters when resolving
    /// </summary>
    public sealed class NamedParameterOverloads : Dictionary<string, object>
    {
        /// <summary>
        /// Froms the i dictionary.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>NamedParameterOverloads.</returns>
        public static NamedParameterOverloads FromIDictionary(IDictionary<string, object> data)
        {
            return data as NamedParameterOverloads ?? new NamedParameterOverloads(data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedParameterOverloads"/> class.
        /// </summary>
        public NamedParameterOverloads()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedParameterOverloads"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public NamedParameterOverloads(IDictionary<string, object> data)
            : base(data)
        {
        }

        /// <summary>
        /// The _ default
        /// </summary>
        private static readonly NamedParameterOverloads _Default = new NamedParameterOverloads();

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>The default.</value>
        public static NamedParameterOverloads Default
        {
            get
            {
                return _Default;
            }
        }
    }

    /// <summary>
    /// Enum UnregisteredResolutionActions
    /// </summary>
    public enum UnregisteredResolutionActions
    {
        /// <summary>
        /// Attempt to resolve type, even if the type isn't registered.
        /// Registered types/options will always take precedence.
        /// </summary>
        AttemptResolve,

        /// <summary>
        /// Fail resolution if type not explicitly registered
        /// </summary>
        Fail,

        /// <summary>
        /// Attempt to resolve unregistered type if requested type is generic
        /// and no registration exists for the specific generic parameters used.
        /// Registered types/options will always take precedence.
        /// </summary>
        GenericsOnly
    }

    /// <summary>
    /// Enum NamedResolutionFailureActions
    /// </summary>
    public enum NamedResolutionFailureActions
    {
        /// <summary>
        /// The attempt unnamed resolution
        /// </summary>
        AttemptUnnamedResolution,
        /// <summary>
        /// The fail
        /// </summary>
        Fail
    }

    /// <summary>
    /// Resolution settings
    /// </summary>
    public sealed class ResolveOptions
    {
        /// <summary>
        /// The _ default
        /// </summary>
        private static readonly ResolveOptions _Default = new ResolveOptions();
        /// <summary>
        /// The _ fail unregistered and name not found
        /// </summary>
        private static readonly ResolveOptions _FailUnregisteredAndNameNotFound = new ResolveOptions() { NamedResolutionFailureAction = NamedResolutionFailureActions.Fail, UnregisteredResolutionAction = UnregisteredResolutionActions.Fail };
        /// <summary>
        /// The _ fail unregistered only
        /// </summary>
        private static readonly ResolveOptions _FailUnregisteredOnly = new ResolveOptions() { NamedResolutionFailureAction = NamedResolutionFailureActions.AttemptUnnamedResolution, UnregisteredResolutionAction = UnregisteredResolutionActions.Fail };
        /// <summary>
        /// The _ fail name not found only
        /// </summary>
        private static readonly ResolveOptions _FailNameNotFoundOnly = new ResolveOptions() { NamedResolutionFailureAction = NamedResolutionFailureActions.Fail, UnregisteredResolutionAction = UnregisteredResolutionActions.AttemptResolve };

        /// <summary>
        /// The _ unregistered resolution action
        /// </summary>
        private UnregisteredResolutionActions _UnregisteredResolutionAction = UnregisteredResolutionActions.AttemptResolve;
        /// <summary>
        /// Gets or sets the unregistered resolution action.
        /// </summary>
        /// <value>The unregistered resolution action.</value>
        public UnregisteredResolutionActions UnregisteredResolutionAction
        {
            get { return _UnregisteredResolutionAction; }
            set { _UnregisteredResolutionAction = value; }
        }

        /// <summary>
        /// The _ named resolution failure action
        /// </summary>
        private NamedResolutionFailureActions _NamedResolutionFailureAction = NamedResolutionFailureActions.Fail;
        /// <summary>
        /// Gets or sets the named resolution failure action.
        /// </summary>
        /// <value>The named resolution failure action.</value>
        public NamedResolutionFailureActions NamedResolutionFailureAction
        {
            get { return _NamedResolutionFailureAction; }
            set { _NamedResolutionFailureAction = value; }
        }

        /// <summary>
        /// Gets the default options (attempt resolution of unregistered types, fail on named resolution if name not found)
        /// </summary>
        /// <value>The default.</value>
        public static ResolveOptions Default
        {
            get
            {
                return _Default;
            }
        }

        /// <summary>
        /// Preconfigured option for attempting resolution of unregistered types and failing on named resolution if name not found
        /// </summary>
        /// <value>The fail name not found only.</value>
        public static ResolveOptions FailNameNotFoundOnly
        {
            get
            {
                return _FailNameNotFoundOnly;
            }
        }

        /// <summary>
        /// Preconfigured option for failing on resolving unregistered types and on named resolution if name not found
        /// </summary>
        /// <value>The fail unregistered and name not found.</value>
        public static ResolveOptions FailUnregisteredAndNameNotFound
        {
            get
            {
                return _FailUnregisteredAndNameNotFound;
            }
        }

        /// <summary>
        /// Preconfigured option for failing on resolving unregistered types, but attempting unnamed resolution if name not found
        /// </summary>
        /// <value>The fail unregistered only.</value>
        public static ResolveOptions FailUnregisteredOnly
        {
            get
            {
                return _FailUnregisteredOnly;
            }
        }
    }
    #endregion

    /// <summary>
    /// Class TinyIoCContainer. This class cannot be inherited.
    /// </summary>
    public sealed partial class TinyIoCContainer : IDisposable
    {
        #region Fake NETFX_CORE Classes
#if NETFX_CORE
        private sealed class MethodAccessException : Exception
        {
        }

        private sealed class AppDomain
        {
            public static AppDomain CurrentDomain { get; private set; }

            static AppDomain()
            {
                CurrentDomain = new AppDomain();
            }

            // @mbrit - 2012-05-30 - in WinRT, this should be done async...
            public async Task<List<Assembly>> GetAssembliesAsync()
            {
                var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

                List<Assembly> assemblies = new List<Assembly>();

                var files = await folder.GetFilesAsync();

                foreach (StorageFile file in files)
                {
                    if (file.FileType == ".dll" || file.FileType == ".exe")
                    {
                        AssemblyName name = new AssemblyName() { Name = System.IO.Path.GetFileNameWithoutExtension(file.Name) };
                        try
                        {
                            var asm = Assembly.Load(name);
                            assemblies.Add(asm);
                        }
                        catch
                        {
                            // ignore exceptions here...
                        }
                    }
                }

                return assemblies;
            }
        }
#endif
        #endregion

        #region "Fluent" API
        /// <summary>
        /// Registration options for "fluent" API
        /// </summary>
        public sealed class RegisterOptions
        {
            /// <summary>
            /// The _ container
            /// </summary>
            private TinyIoCContainer _Container;
            /// <summary>
            /// The _ registration
            /// </summary>
            private TypeRegistration _Registration;

            /// <summary>
            /// Initializes a new instance of the <see cref="RegisterOptions"/> class.
            /// </summary>
            /// <param name="container">The container.</param>
            /// <param name="registration">The registration.</param>
            public RegisterOptions(TinyIoCContainer container, TypeRegistration registration)
            {
                _Container = container;
                _Registration = registration;
            }

            /// <summary>
            /// Make registration a singleton (single instance) if possible
            /// </summary>
            /// <returns>RegisterOptions</returns>
            /// <exception cref="TinyIoC.TinyIoCRegistrationException">singleton</exception>
            public RegisterOptions AsSingleton()
            {
                var currentFactory = _Container.GetCurrentFactory(_Registration);

                if (currentFactory == null)
                    throw new TinyIoCRegistrationException(_Registration.Type, "singleton");

                return _Container.AddUpdateRegistration(_Registration, currentFactory.SingletonVariant);
            }

            /// <summary>
            /// Make registration multi-instance if possible
            /// </summary>
            /// <returns>RegisterOptions</returns>
            /// <exception cref="TinyIoC.TinyIoCRegistrationException">multi-instance</exception>
            public RegisterOptions AsMultiInstance()
            {
                var currentFactory = _Container.GetCurrentFactory(_Registration);

                if (currentFactory == null)
                    throw new TinyIoCRegistrationException(_Registration.Type, "multi-instance");

                return _Container.AddUpdateRegistration(_Registration, currentFactory.MultiInstanceVariant);
            }

            /// <summary>
            /// Make registration hold a weak reference if possible
            /// </summary>
            /// <returns>RegisterOptions</returns>
            /// <exception cref="TinyIoC.TinyIoCRegistrationException">weak reference</exception>
            public RegisterOptions WithWeakReference()
            {
                var currentFactory = _Container.GetCurrentFactory(_Registration);

                if (currentFactory == null)
                    throw new TinyIoCRegistrationException(_Registration.Type, "weak reference");

                return _Container.AddUpdateRegistration(_Registration, currentFactory.WeakReferenceVariant);
            }

            /// <summary>
            /// Make registration hold a strong reference if possible
            /// </summary>
            /// <returns>RegisterOptions</returns>
            /// <exception cref="TinyIoC.TinyIoCRegistrationException">strong reference</exception>
            public RegisterOptions WithStrongReference()
            {
                var currentFactory = _Container.GetCurrentFactory(_Registration);

                if (currentFactory == null)
                    throw new TinyIoCRegistrationException(_Registration.Type, "strong reference");

                return _Container.AddUpdateRegistration(_Registration, currentFactory.StrongReferenceVariant);
            }

#if EXPRESSIONS
            /// <summary>
            /// Make registration using constructor
            /// </summary>
            /// <returns>RegisterOptions</returns>
            public RegisterOptions UsingConstructor<RegisterType>(Expression<Func<RegisterType>> constructor)
            {
                var lambda = constructor as LambdaExpression;
                if (lambda == null)
                    throw new TinyIoCConstructorResolutionException(typeof(RegisterType));

                var newExpression = lambda.Body as NewExpression;
                if (newExpression == null)
                    throw new TinyIoCConstructorResolutionException(typeof(RegisterType));

                var constructorInfo = newExpression.Constructor;
                if (constructorInfo == null)
                    throw new TinyIoCConstructorResolutionException(typeof(RegisterType));

                var currentFactory = _Container.GetCurrentFactory(_Registration);
                if (currentFactory == null)
                    throw new TinyIoCConstructorResolutionException(typeof(RegisterType));

                currentFactory.SetConstructor(constructorInfo);

                return this;
            }
#endif
            /// <summary>
            /// Switches to a custom lifetime manager factory if possible.
            /// Usually used for RegisterOptions "To*" extension methods such as the ASP.Net per-request one.
            /// </summary>
            /// <param name="instance">RegisterOptions instance</param>
            /// <param name="lifetimeProvider">Custom lifetime manager</param>
            /// <param name="errorString">Error string to display if switch fails</param>
            /// <returns>RegisterOptions</returns>
            /// <exception cref="System.ArgumentNullException">
            /// instance;instance is null.
            /// or
            /// lifetimeProvider;lifetimeProvider is null.
            /// </exception>
            /// <exception cref="System.ArgumentException">errorString is null or empty.;errorString</exception>
            /// <exception cref="TinyIoC.TinyIoCRegistrationException"></exception>
            public static RegisterOptions ToCustomLifetimeManager(RegisterOptions instance, ITinyIoCObjectLifetimeProvider lifetimeProvider, string errorString)
            {
                if (instance == null)
                    throw new ArgumentNullException("instance", "instance is null.");

                if (lifetimeProvider == null)
                    throw new ArgumentNullException("lifetimeProvider", "lifetimeProvider is null.");

                if (String.IsNullOrEmpty(errorString))
                    throw new ArgumentException("errorString is null or empty.", "errorString");

                var currentFactory = instance._Container.GetCurrentFactory(instance._Registration);

                if (currentFactory == null)
                    throw new TinyIoCRegistrationException(instance._Registration.Type, errorString);

                return instance._Container.AddUpdateRegistration(instance._Registration, currentFactory.GetCustomObjectLifetimeVariant(lifetimeProvider, errorString));
            }
        }

        /// <summary>
        /// Registration options for "fluent" API when registering multiple implementations
        /// </summary>
        public sealed class MultiRegisterOptions
        {
            /// <summary>
            /// The _ register options
            /// </summary>
            private IEnumerable<RegisterOptions> _RegisterOptions;

            /// <summary>
            /// Initializes a new instance of the MultiRegisterOptions class.
            /// </summary>
            /// <param name="registerOptions">Registration options</param>
            public MultiRegisterOptions(IEnumerable<RegisterOptions> registerOptions)
            {
                _RegisterOptions = registerOptions;
            }

            /// <summary>
            /// Make registration a singleton (single instance) if possible
            /// </summary>
            /// <returns>RegisterOptions</returns>
            public MultiRegisterOptions AsSingleton()
            {
                _RegisterOptions = ExecuteOnAllRegisterOptions(ro => ro.AsSingleton());
                return this;
            }

            /// <summary>
            /// Make registration multi-instance if possible
            /// </summary>
            /// <returns>MultiRegisterOptions</returns>
            public MultiRegisterOptions AsMultiInstance()
            {
                _RegisterOptions = ExecuteOnAllRegisterOptions(ro => ro.AsMultiInstance());
                return this;
            }

            /// <summary>
            /// Executes the on all register options.
            /// </summary>
            /// <param name="action">The action.</param>
            /// <returns>IEnumerable&lt;RegisterOptions&gt;.</returns>
            private IEnumerable<RegisterOptions> ExecuteOnAllRegisterOptions(Func<RegisterOptions, RegisterOptions> action)
            {
                var newRegisterOptions = new List<RegisterOptions>();

                foreach (var registerOption in _RegisterOptions)
                {
                    newRegisterOptions.Add(action(registerOption));
                }

                return newRegisterOptions;
            }
        }
        #endregion

        #region Public API
        #region Child Containers
        /// <summary>
        /// Gets the child container.
        /// </summary>
        /// <returns>TinyIoCContainer.</returns>
        public TinyIoCContainer GetChildContainer()
        {
            return new TinyIoCContainer(this);
        }
        #endregion

        #region Registration
        /// <summary>
        /// Attempt to automatically register all non-generic classes and interfaces in the current app domain.
        /// If more than one class implements an interface then only one implementation will be registered
        /// although no error will be thrown.
        /// </summary>
        public void AutoRegister()
        {
#if APPDOMAIN_GETASSEMBLIES
            AutoRegisterInternal(AppDomain.CurrentDomain.GetAssemblies().Where(a => !IsIgnoredAssembly(a)), true, null);
#elif WINDOWS_UWP
            AutoRegisterInternal(new Assembly[] { this.GetType().GetTypeInfo().Assembly }, true, null);
#else
            AutoRegisterInternal(new Assembly[] {this.GetType().Assembly()}, true, null);
#endif
        }

        /// <summary>
        /// Attempt to automatically register all non-generic classes and interfaces in the current app domain.
        /// Types will only be registered if they pass the supplied registration predicate.
        /// If more than one class implements an interface then only one implementation will be registered
        /// although no error will be thrown.
        /// </summary>
        /// <param name="registrationPredicate">Predicate to determine if a particular type should be registered</param>
        public void AutoRegister(Func<Type, bool> registrationPredicate)
        {
#if APPDOMAIN_GETASSEMBLIES
            AutoRegisterInternal(AppDomain.CurrentDomain.GetAssemblies().Where(a => !IsIgnoredAssembly(a)), true, registrationPredicate);
#elif WINDOWS_UWP
            AutoRegisterInternal(new Assembly[] { this.GetType().GetTypeInfo().Assembly }, true, registrationPredicate);
#else
            AutoRegisterInternal(new Assembly[] { this.GetType().Assembly()}, true, registrationPredicate);
#endif
        }

        /// <summary>
        /// Attempt to automatically register all non-generic classes and interfaces in the current app domain.
        /// </summary>
        /// <param name="ignoreDuplicateImplementations">Whether to ignore duplicate implementations of an interface/base class. False=throw an exception</param>
        /// <exception cref="TinyIoCAutoRegistrationException"></exception>
        public void AutoRegister(bool ignoreDuplicateImplementations)
        {
#if APPDOMAIN_GETASSEMBLIES
            AutoRegisterInternal(AppDomain.CurrentDomain.GetAssemblies().Where(a => !IsIgnoredAssembly(a)), ignoreDuplicateImplementations, null);
#elif WINDOWS_UWP
            AutoRegisterInternal(new Assembly[] { this.GetType().GetTypeInfo().Assembly }, ignoreDuplicateImplementations, null);
#else
            AutoRegisterInternal(new Assembly[] { this.GetType().Assembly() }, ignoreDuplicateImplementations, null);
#endif
        }

        /// <summary>
        /// Attempt to automatically register all non-generic classes and interfaces in the current app domain.
        /// Types will only be registered if they pass the supplied registration predicate.
        /// </summary>
        /// <param name="ignoreDuplicateImplementations">Whether to ignore duplicate implementations of an interface/base class. False=throw an exception</param>
        /// <param name="registrationPredicate">Predicate to determine if a particular type should be registered</param>
        /// <exception cref="TinyIoCAutoRegistrationException"></exception>
        public void AutoRegister(bool ignoreDuplicateImplementations, Func<Type, bool> registrationPredicate)
        {
#if APPDOMAIN_GETASSEMBLIES
            AutoRegisterInternal(AppDomain.CurrentDomain.GetAssemblies().Where(a => !IsIgnoredAssembly(a)), ignoreDuplicateImplementations, registrationPredicate);
#elif WINDOWS_UWP
            AutoRegisterInternal(new Assembly[] { this.GetType().GetTypeInfo().Assembly }, ignoreDuplicateImplementations, registrationPredicate);
#else
            AutoRegisterInternal(new Assembly[] { this.GetType().Assembly() }, ignoreDuplicateImplementations, registrationPredicate);
#endif
        }

        /// <summary>
        /// Attempt to automatically register all non-generic classes and interfaces in the specified assemblies
        /// If more than one class implements an interface then only one implementation will be registered
        /// although no error will be thrown.
        /// </summary>
        /// <param name="assemblies">Assemblies to process</param>
        public void AutoRegister(IEnumerable<Assembly> assemblies)
        {
            AutoRegisterInternal(assemblies, true, null);
        }

        /// <summary>
        /// Attempt to automatically register all non-generic classes and interfaces in the specified assemblies
        /// Types will only be registered if they pass the supplied registration predicate.
        /// If more than one class implements an interface then only one implementation will be registered
        /// although no error will be thrown.
        /// </summary>
        /// <param name="assemblies">Assemblies to process</param>
        /// <param name="registrationPredicate">Predicate to determine if a particular type should be registered</param>
        public void AutoRegister(IEnumerable<Assembly> assemblies, Func<Type, bool> registrationPredicate)
        {
            AutoRegisterInternal(assemblies, true, registrationPredicate);
        }

        /// <summary>
        /// Attempt to automatically register all non-generic classes and interfaces in the specified assemblies
        /// </summary>
        /// <param name="assemblies">Assemblies to process</param>
        /// <param name="ignoreDuplicateImplementations">Whether to ignore duplicate implementations of an interface/base class. False=throw an exception</param>
        /// <exception cref="TinyIoCAutoRegistrationException"></exception>
        public void AutoRegister(IEnumerable<Assembly> assemblies, bool ignoreDuplicateImplementations)
        {
            AutoRegisterInternal(assemblies, ignoreDuplicateImplementations, null);
        }

        /// <summary>
        /// Attempt to automatically register all non-generic classes and interfaces in the specified assemblies
        /// Types will only be registered if they pass the supplied registration predicate.
        /// </summary>
        /// <param name="assemblies">Assemblies to process</param>
        /// <param name="ignoreDuplicateImplementations">Whether to ignore duplicate implementations of an interface/base class. False=throw an exception</param>
        /// <param name="registrationPredicate">Predicate to determine if a particular type should be registered</param>
        /// <exception cref="TinyIoCAutoRegistrationException"></exception>
        public void AutoRegister(IEnumerable<Assembly> assemblies, bool ignoreDuplicateImplementations, Func<Type, bool> registrationPredicate)
        {
            AutoRegisterInternal(assemblies, ignoreDuplicateImplementations, registrationPredicate);
        }

        /// <summary>
        /// Creates/replaces a container class registration with default options.
        /// </summary>
        /// <param name="registerType">Type to register</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register(Type registerType)
        {
            return RegisterInternal(registerType, string.Empty, GetDefaultObjectFactory(registerType, registerType));
        }

        /// <summary>
        /// Creates/replaces a named container class registration with default options.
        /// </summary>
        /// <param name="registerType">Type to register</param>
        /// <param name="name">Name of registration</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register(Type registerType, string name)
        {
            return RegisterInternal(registerType, name, GetDefaultObjectFactory(registerType, registerType));

        }

        /// <summary>
        /// Creates/replaces a container class registration with a given implementation and default options.
        /// </summary>
        /// <param name="registerType">Type to register</param>
        /// <param name="registerImplementation">Type to instantiate that implements RegisterType</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register(Type registerType, Type registerImplementation)
        {
            return this.RegisterInternal(registerType, string.Empty, GetDefaultObjectFactory(registerType, registerImplementation));
        }

        /// <summary>
        /// Creates/replaces a named container class registration with a given implementation and default options.
        /// </summary>
        /// <param name="registerType">Type to register</param>
        /// <param name="registerImplementation">Type to instantiate that implements RegisterType</param>
        /// <param name="name">Name of registration</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register(Type registerType, Type registerImplementation, string name)
        {
            return this.RegisterInternal(registerType, name, GetDefaultObjectFactory(registerType, registerImplementation));
        }

        /// <summary>
        /// Creates/replaces a container class registration with a specific, strong referenced, instance.
        /// </summary>
        /// <param name="registerType">Type to register</param>
        /// <param name="instance">Instance of RegisterType to register</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register(Type registerType, object instance)
        {
            return RegisterInternal(registerType, string.Empty, new InstanceFactory(registerType, registerType, instance));
        }

        /// <summary>
        /// Creates/replaces a named container class registration with a specific, strong referenced, instance.
        /// </summary>
        /// <param name="registerType">Type to register</param>
        /// <param name="instance">Instance of RegisterType to register</param>
        /// <param name="name">Name of registration</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register(Type registerType, object instance, string name)
        {
            return RegisterInternal(registerType, name, new InstanceFactory(registerType, registerType, instance));
        }

        /// <summary>
        /// Creates/replaces a container class registration with a specific, strong referenced, instance.
        /// </summary>
        /// <param name="registerType">Type to register</param>
        /// <param name="registerImplementation">Type of instance to register that implements RegisterType</param>
        /// <param name="instance">Instance of RegisterImplementation to register</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register(Type registerType, Type registerImplementation, object instance)
        {
            return RegisterInternal(registerType, string.Empty, new InstanceFactory(registerType, registerImplementation, instance));
        }

        /// <summary>
        /// Creates/replaces a named container class registration with a specific, strong referenced, instance.
        /// </summary>
        /// <param name="registerType">Type to register</param>
        /// <param name="registerImplementation">Type of instance to register that implements RegisterType</param>
        /// <param name="instance">Instance of RegisterImplementation to register</param>
        /// <param name="name">Name of registration</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register(Type registerType, Type registerImplementation, object instance, string name)
        {
            return RegisterInternal(registerType, name, new InstanceFactory(registerType, registerImplementation, instance));
        }

        /// <summary>
        /// Creates/replaces a container class registration with a user specified factory
        /// </summary>
        /// <param name="registerType">Type to register</param>
        /// <param name="factory">Factory/lambda that returns an instance of RegisterType</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register(Type registerType, Func<TinyIoCContainer, NamedParameterOverloads, object> factory)
        {
            return RegisterInternal(registerType, string.Empty, new DelegateFactory(registerType, factory));
        }

        /// <summary>
        /// Creates/replaces a container class registration with a user specified factory
        /// </summary>
        /// <param name="registerType">Type to register</param>
        /// <param name="factory">Factory/lambda that returns an instance of RegisterType</param>
        /// <param name="name">Name of registation</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register(Type registerType, Func<TinyIoCContainer, NamedParameterOverloads, object> factory, string name)
        {
            return RegisterInternal(registerType, name, new DelegateFactory(registerType, factory));
        }

        /// <summary>
        /// Creates/replaces a container class registration with default options.
        /// </summary>
        /// <typeparam name="RegisterType">The type of the register type.</typeparam>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register<RegisterType>()
            where RegisterType : class
        {
            return this.Register(typeof(RegisterType));
        }

        /// <summary>
        /// Creates/replaces a named container class registration with default options.
        /// </summary>
        /// <typeparam name="RegisterType">The type of the register type.</typeparam>
        /// <param name="name">Name of registration</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register<RegisterType>(string name)
            where RegisterType : class
        {
            return this.Register(typeof(RegisterType), name);
        }

        /// <summary>
        /// Creates/replaces a container class registration with a given implementation and default options.
        /// </summary>
        /// <typeparam name="RegisterType">Type to register</typeparam>
        /// <typeparam name="RegisterImplementation">Type to instantiate that implements RegisterType</typeparam>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register<RegisterType, RegisterImplementation>()
            where RegisterType : class
            where RegisterImplementation : class, RegisterType
        {
            return this.Register(typeof(RegisterType), typeof(RegisterImplementation));
        }

        /// <summary>
        /// Creates/replaces a named container class registration with a given implementation and default options.
        /// </summary>
        /// <typeparam name="RegisterType">Type to register</typeparam>
        /// <typeparam name="RegisterImplementation">Type to instantiate that implements RegisterType</typeparam>
        /// <param name="name">Name of registration</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register<RegisterType, RegisterImplementation>(string name)
            where RegisterType : class
            where RegisterImplementation : class, RegisterType
        {
            return this.Register(typeof(RegisterType), typeof(RegisterImplementation), name);
        }

        /// <summary>
        /// Creates/replaces a container class registration with a specific, strong referenced, instance.
        /// </summary>
        /// <typeparam name="RegisterType">Type to register</typeparam>
        /// <param name="instance">Instance of RegisterType to register</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register<RegisterType>(RegisterType instance)
           where RegisterType : class
        {
            return this.Register(typeof(RegisterType), instance);
        }

        /// <summary>
        /// Creates/replaces a named container class registration with a specific, strong referenced, instance.
        /// </summary>
        /// <typeparam name="RegisterType">Type to register</typeparam>
        /// <param name="instance">Instance of RegisterType to register</param>
        /// <param name="name">Name of registration</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register<RegisterType>(RegisterType instance, string name)
            where RegisterType : class
        {
            return this.Register(typeof(RegisterType), instance, name);
        }

        /// <summary>
        /// Creates/replaces a container class registration with a specific, strong referenced, instance.
        /// </summary>
        /// <typeparam name="RegisterType">Type to register</typeparam>
        /// <typeparam name="RegisterImplementation">Type of instance to register that implements RegisterType</typeparam>
        /// <param name="instance">Instance of RegisterImplementation to register</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register<RegisterType, RegisterImplementation>(RegisterImplementation instance)
            where RegisterType : class
            where RegisterImplementation : class, RegisterType
        {
            return this.Register(typeof(RegisterType), typeof(RegisterImplementation), instance);
        }

        /// <summary>
        /// Creates/replaces a named container class registration with a specific, strong referenced, instance.
        /// </summary>
        /// <typeparam name="RegisterType">Type to register</typeparam>
        /// <typeparam name="RegisterImplementation">Type of instance to register that implements RegisterType</typeparam>
        /// <param name="instance">Instance of RegisterImplementation to register</param>
        /// <param name="name">Name of registration</param>
        /// <returns>RegisterOptions for fluent API</returns>
        public RegisterOptions Register<RegisterType, RegisterImplementation>(RegisterImplementation instance, string name)
            where RegisterType : class
            where RegisterImplementation : class, RegisterType
        {
            return this.Register(typeof(RegisterType), typeof(RegisterImplementation), instance, name);
        }

        /// <summary>
        /// Creates/replaces a container class registration with a user specified factory
        /// </summary>
        /// <typeparam name="RegisterType">Type to register</typeparam>
        /// <param name="factory">Factory/lambda that returns an instance of RegisterType</param>
        /// <returns>RegisterOptions for fluent API</returns>
        /// <exception cref="System.ArgumentNullException">factory</exception>
        public RegisterOptions Register<RegisterType>(Func<TinyIoCContainer, NamedParameterOverloads, RegisterType> factory)
            where RegisterType : class
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return this.Register(typeof(RegisterType), (c, o) => factory(c, o));
        }

        /// <summary>
        /// Creates/replaces a named container class registration with a user specified factory
        /// </summary>
        /// <typeparam name="RegisterType">Type to register</typeparam>
        /// <param name="factory">Factory/lambda that returns an instance of RegisterType</param>
        /// <param name="name">Name of registation</param>
        /// <returns>RegisterOptions for fluent API</returns>
        /// <exception cref="System.ArgumentNullException">factory</exception>
        public RegisterOptions Register<RegisterType>(Func<TinyIoCContainer, NamedParameterOverloads, RegisterType> factory, string name)
            where RegisterType : class
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            return this.Register(typeof(RegisterType), (c, o) => factory(c, o), name);
        }

        /// <summary>
        /// Register multiple implementations of a type.
        /// Internally this registers each implementation using the full name of the class as its registration name.
        /// </summary>
        /// <typeparam name="RegisterType">Type that each implementation implements</typeparam>
        /// <param name="implementationTypes">Types that implement RegisterType</param>
        /// <returns>MultiRegisterOptions for the fluent API</returns>
        public MultiRegisterOptions RegisterMultiple<RegisterType>(IEnumerable<Type> implementationTypes)
        {
            return RegisterMultiple(typeof(RegisterType), implementationTypes);
        }

        /// <summary>
        /// Register multiple implementations of a type.
        /// Internally this registers each implementation using the full name of the class as its registration name.
        /// </summary>
        /// <param name="registrationType">Type that each implementation implements</param>
        /// <param name="implementationTypes">Types that implement RegisterType</param>
        /// <returns>MultiRegisterOptions for the fluent API</returns>
        /// <exception cref="System.ArgumentNullException">types;types is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// types: The same implementation type cannot be specificed multiple times
        /// </exception>
        public MultiRegisterOptions RegisterMultiple(Type registrationType, IEnumerable<Type> implementationTypes)
        {
            if (implementationTypes == null)
                throw new ArgumentNullException("types", "types is null.");

            foreach (var type in implementationTypes)
//#if NETFX_CORE
//				if (!registrationType.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
//#else
                if (!registrationType.IsAssignableFrom(type))
//#endif
                    throw new ArgumentException(String.Format("types: The type {0} is not assignable from {1}", registrationType.FullName, type.FullName));

            if (implementationTypes.Count() != implementationTypes.Distinct().Count())
                throw new ArgumentException("types: The same implementation type cannot be specificed multiple times");

            var registerOptions = new List<RegisterOptions>();

            foreach (var type in implementationTypes)
            {
                registerOptions.Add(Register(registrationType, type, type.FullName));
            }

            return new MultiRegisterOptions(registerOptions);
        }
        #endregion

        #region Resolution
        /// <summary>
        /// Attempts to resolve a type using default options.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public object Resolve(Type resolveType)
        {
            return ResolveInternal(new TypeRegistration(resolveType), NamedParameterOverloads.Default, ResolveOptions.Default);
        }

        /// <summary>
        /// Attempts to resolve a type using specified options.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <param name="options">Resolution options</param>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public object Resolve(Type resolveType, ResolveOptions options)
        {
            return ResolveInternal(new TypeRegistration(resolveType), NamedParameterOverloads.Default, options);
        }

        /// <summary>
        /// Attempts to resolve a type using default options and the supplied name.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <param name="name">Name of registration</param>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public object Resolve(Type resolveType, string name)
        {
            return ResolveInternal(new TypeRegistration(resolveType, name), NamedParameterOverloads.Default, ResolveOptions.Default);
        }

        /// <summary>
        /// Attempts to resolve a type using supplied options and  name.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <param name="name">Name of registration</param>
        /// <param name="options">Resolution options</param>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public object Resolve(Type resolveType, string name, ResolveOptions options)
        {
            return ResolveInternal(new TypeRegistration(resolveType, name), NamedParameterOverloads.Default, options);
        }

        /// <summary>
        /// Attempts to resolve a type using default options and the supplied constructor parameters.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public object Resolve(Type resolveType, NamedParameterOverloads parameters)
        {
            return ResolveInternal(new TypeRegistration(resolveType), parameters, ResolveOptions.Default);
        }

        /// <summary>
        /// Attempts to resolve a type using specified options and the supplied constructor parameters.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <param name="options">Resolution options</param>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public object Resolve(Type resolveType, NamedParameterOverloads parameters, ResolveOptions options)
        {
            return ResolveInternal(new TypeRegistration(resolveType), parameters, options);
        }

        /// <summary>
        /// Attempts to resolve a type using default options and the supplied constructor parameters and name.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <param name="name">Name of registration</param>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public object Resolve(Type resolveType, string name, NamedParameterOverloads parameters)
        {
            return ResolveInternal(new TypeRegistration(resolveType, name), parameters, ResolveOptions.Default);
        }

        /// <summary>
        /// Attempts to resolve a named type using specified options and the supplied constructor parameters.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <param name="name">Name of registration</param>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <param name="options">Resolution options</param>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public object Resolve(Type resolveType, string name, NamedParameterOverloads parameters, ResolveOptions options)
        {
            return ResolveInternal(new TypeRegistration(resolveType, name), parameters, options);
        }

        /// <summary>
        /// Attempts to resolve a type using default options.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public ResolveType Resolve<ResolveType>()
            where ResolveType : class
        {
            return (ResolveType)Resolve(typeof(ResolveType));
        }

        /// <summary>
        /// Attempts to resolve a type using specified options.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="options">Resolution options</param>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public ResolveType Resolve<ResolveType>(ResolveOptions options)
            where ResolveType : class
        {
            return (ResolveType)Resolve(typeof(ResolveType), options);
        }

        /// <summary>
        /// Attempts to resolve a type using default options and the supplied name.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="name">Name of registration</param>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public ResolveType Resolve<ResolveType>(string name)
            where ResolveType : class
        {
            return (ResolveType)Resolve(typeof(ResolveType), name);
        }

        /// <summary>
        /// Attempts to resolve a type using supplied options and  name.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="name">Name of registration</param>
        /// <param name="options">Resolution options</param>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public ResolveType Resolve<ResolveType>(string name, ResolveOptions options)
            where ResolveType : class
        {
            return (ResolveType)Resolve(typeof(ResolveType), name, options);
        }

        /// <summary>
        /// Attempts to resolve a type using default options and the supplied constructor parameters.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public ResolveType Resolve<ResolveType>(NamedParameterOverloads parameters)
            where ResolveType : class
        {
            return (ResolveType)Resolve(typeof(ResolveType), parameters);
        }

        /// <summary>
        /// Attempts to resolve a type using specified options and the supplied constructor parameters.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <param name="options">Resolution options</param>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public ResolveType Resolve<ResolveType>(NamedParameterOverloads parameters, ResolveOptions options)
            where ResolveType : class
        {
            return (ResolveType)Resolve(typeof(ResolveType), parameters, options);
        }

        /// <summary>
        /// Attempts to resolve a type using default options and the supplied constructor parameters and name.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="name">Name of registration</param>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public ResolveType Resolve<ResolveType>(string name, NamedParameterOverloads parameters)
            where ResolveType : class
        {
            return (ResolveType)Resolve(typeof(ResolveType), name, parameters);
        }

        /// <summary>
        /// Attempts to resolve a named type using specified options and the supplied constructor parameters.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="name">Name of registration</param>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <param name="options">Resolution options</param>
        /// <returns>Instance of type</returns>
        /// <exception cref="TinyIoCResolutionException">Unable to resolve the type.</exception>
        public ResolveType Resolve<ResolveType>(string name, NamedParameterOverloads parameters, ResolveOptions options)
            where ResolveType : class
        {
            return (ResolveType)Resolve(typeof(ResolveType), name, parameters, options);
        }

        /// <summary>
        /// Attempts to predict whether a given type can be resolved with default options.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        public bool CanResolve(Type resolveType)
        {
            return CanResolveInternal(new TypeRegistration(resolveType), NamedParameterOverloads.Default, ResolveOptions.Default);
        }

        /// <summary>
        /// Attempts to predict whether a given named type can be resolved with default options.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <param name="name">The name.</param>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        private bool CanResolve(Type resolveType, string name)
        {
            return CanResolveInternal(new TypeRegistration(resolveType, name), NamedParameterOverloads.Default, ResolveOptions.Default);
        }

        /// <summary>
        /// Attempts to predict whether a given type can be resolved with the specified options.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <param name="options">Resolution options</param>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        public bool CanResolve(Type resolveType, ResolveOptions options)
        {
            return CanResolveInternal(new TypeRegistration(resolveType), NamedParameterOverloads.Default, options);
        }

        /// <summary>
        /// Attempts to predict whether a given named type can be resolved with the specified options.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <param name="name">Name of registration</param>
        /// <param name="options">Resolution options</param>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        public bool CanResolve(Type resolveType, string name, ResolveOptions options)
        {
            return CanResolveInternal(new TypeRegistration(resolveType, name), NamedParameterOverloads.Default, options);
        }

        /// <summary>
        /// Attempts to predict whether a given type can be resolved with the supplied constructor parameters and default options.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <param name="parameters">User supplied named parameter overloads</param>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        public bool CanResolve(Type resolveType, NamedParameterOverloads parameters)
        {
            return CanResolveInternal(new TypeRegistration(resolveType), parameters, ResolveOptions.Default);
        }

        /// <summary>
        /// Attempts to predict whether a given named type can be resolved with the supplied constructor parameters and default options.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <param name="name">Name of registration</param>
        /// <param name="parameters">User supplied named parameter overloads</param>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        public bool CanResolve(Type resolveType, string name, NamedParameterOverloads parameters)
        {
            return CanResolveInternal(new TypeRegistration(resolveType, name), parameters, ResolveOptions.Default);
        }

        /// <summary>
        /// Attempts to predict whether a given type can be resolved with the supplied constructor parameters options.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <param name="parameters">User supplied named parameter overloads</param>
        /// <param name="options">Resolution options</param>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        public bool CanResolve(Type resolveType, NamedParameterOverloads parameters, ResolveOptions options)
        {
            return CanResolveInternal(new TypeRegistration(resolveType), parameters, options);
        }

        /// <summary>
        /// Attempts to predict whether a given named type can be resolved with the supplied constructor parameters options.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <param name="resolveType">Type to resolve</param>
        /// <param name="name">Name of registration</param>
        /// <param name="parameters">User supplied named parameter overloads</param>
        /// <param name="options">Resolution options</param>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        public bool CanResolve(Type resolveType, string name, NamedParameterOverloads parameters, ResolveOptions options)
        {
            return CanResolveInternal(new TypeRegistration(resolveType, name), parameters, options);
        }

        /// <summary>
        /// Attempts to predict whether a given type can be resolved with default options.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        public bool CanResolve<ResolveType>()
            where ResolveType : class
        {
            return CanResolve(typeof(ResolveType));
        }

        /// <summary>
        /// Attempts to predict whether a given named type can be resolved with default options.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        public bool CanResolve<ResolveType>(string name)
            where ResolveType : class
        {
            return CanResolve(typeof(ResolveType), name);
        }

        /// <summary>
        /// Attempts to predict whether a given type can be resolved with the specified options.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="options">Resolution options</param>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        public bool CanResolve<ResolveType>(ResolveOptions options)
            where ResolveType : class
        {
            return CanResolve(typeof(ResolveType), options);
        }

        /// <summary>
        /// Attempts to predict whether a given named type can be resolved with the specified options.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="name">Name of registration</param>
        /// <param name="options">Resolution options</param>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        public bool CanResolve<ResolveType>(string name, ResolveOptions options)
            where ResolveType : class
        {
            return CanResolve(typeof(ResolveType), name, options);
        }

        /// <summary>
        /// Attempts to predict whether a given type can be resolved with the supplied constructor parameters and default options.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="parameters">User supplied named parameter overloads</param>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        public bool CanResolve<ResolveType>(NamedParameterOverloads parameters)
            where ResolveType : class
        {
            return CanResolve(typeof(ResolveType), parameters);
        }

        /// <summary>
        /// Attempts to predict whether a given named type can be resolved with the supplied constructor parameters and default options.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="name">Name of registration</param>
        /// <param name="parameters">User supplied named parameter overloads</param>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        public bool CanResolve<ResolveType>(string name, NamedParameterOverloads parameters)
            where ResolveType : class
        {
            return CanResolve(typeof(ResolveType), name, parameters);
        }

        /// <summary>
        /// Attempts to predict whether a given type can be resolved with the supplied constructor parameters options.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="parameters">User supplied named parameter overloads</param>
        /// <param name="options">Resolution options</param>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        public bool CanResolve<ResolveType>(NamedParameterOverloads parameters, ResolveOptions options)
            where ResolveType : class
        {
            return CanResolve(typeof(ResolveType), parameters, options);
        }

        /// <summary>
        /// Attempts to predict whether a given named type can be resolved with the supplied constructor parameters options.
        /// Parameters are used in conjunction with normal container resolution to find the most suitable constructor (if one exists).
        /// All user supplied parameters must exist in at least one resolvable constructor of RegisterType or resolution will fail.
        /// Note: Resolution may still fail if user defined factory registations fail to construct objects when called.
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="name">Name of registration</param>
        /// <param name="parameters">User supplied named parameter overloads</param>
        /// <param name="options">Resolution options</param>
        /// <returns>Bool indicating whether the type can be resolved</returns>
        public bool CanResolve<ResolveType>(string name, NamedParameterOverloads parameters, ResolveOptions options)
            where ResolveType : class
        {
            return CanResolve(typeof(ResolveType), name, parameters, options);
        }

        /// <summary>
        /// Attemps to resolve a type using the default options
        /// </summary>
        /// <param name="resolveType">Type of the resolve.</param>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve(Type resolveType, out object resolvedType)
        {
            try
            {
                resolvedType = Resolve(resolveType);
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = null;
                return false;
            }
        }

        /// <summary>
        /// Attemps to resolve a type using the given options
        /// </summary>
        /// <param name="resolveType">Type of the resolve.</param>
        /// <param name="options">Resolution options</param>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve(Type resolveType, ResolveOptions options, out object resolvedType)
        {
            try
            {
                resolvedType = Resolve(resolveType, options);
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = null;
                return false;
            }
        }

        /// <summary>
        /// Attemps to resolve a type using the default options and given name
        /// </summary>
        /// <param name="resolveType">Type of the resolve.</param>
        /// <param name="name">Name of registration</param>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve(Type resolveType, string name, out object resolvedType)
        {
            try
            {
                resolvedType = Resolve(resolveType, name);
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = null;
                return false;
            }
        }

        /// <summary>
        /// Attemps to resolve a type using the given options and name
        /// </summary>
        /// <param name="resolveType">Type of the resolve.</param>
        /// <param name="name">Name of registration</param>
        /// <param name="options">Resolution options</param>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve(Type resolveType, string name, ResolveOptions options, out object resolvedType)
        {
            try
            {
                resolvedType = Resolve(resolveType, name, options);
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = null;
                return false;
            }
        }

        /// <summary>
        /// Attemps to resolve a type using the default options and supplied constructor parameters
        /// </summary>
        /// <param name="resolveType">Type of the resolve.</param>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve(Type resolveType, NamedParameterOverloads parameters, out object resolvedType)
        {
            try
            {
                resolvedType = Resolve(resolveType, parameters);
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = null;
                return false;
            }
        }

        /// <summary>
        /// Attemps to resolve a type using the default options and supplied name and constructor parameters
        /// </summary>
        /// <param name="resolveType">Type of the resolve.</param>
        /// <param name="name">Name of registration</param>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve(Type resolveType, string name, NamedParameterOverloads parameters, out object resolvedType)
        {
            try
            {
                resolvedType = Resolve(resolveType, name, parameters);
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = null;
                return false;
            }
        }

        /// <summary>
        /// Attemps to resolve a type using the supplied options and constructor parameters
        /// </summary>
        /// <param name="resolveType">Type of the resolve.</param>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <param name="options">Resolution options</param>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve(Type resolveType, NamedParameterOverloads parameters, ResolveOptions options, out object resolvedType)
        {
            try
            {
                resolvedType = Resolve(resolveType, parameters, options);
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = null;
                return false;
            }
        }

        /// <summary>
        /// Attemps to resolve a type using the supplied name, options and constructor parameters
        /// </summary>
        /// <param name="resolveType">Type of the resolve.</param>
        /// <param name="name">Name of registration</param>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <param name="options">Resolution options</param>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve(Type resolveType, string name, NamedParameterOverloads parameters, ResolveOptions options, out object resolvedType)
        {
            try
            {
                resolvedType = Resolve(resolveType, name, parameters, options);
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = null;
                return false;
            }
        }

        /// <summary>
        /// Attemps to resolve a type using the default options
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve<ResolveType>(out ResolveType resolvedType)
            where ResolveType : class
        {
            try
            {
                resolvedType = Resolve<ResolveType>();
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = default(ResolveType);
                return false;
            }
        }

        /// <summary>
        /// Attemps to resolve a type using the given options
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="options">Resolution options</param>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve<ResolveType>(ResolveOptions options, out ResolveType resolvedType)
            where ResolveType : class
        {
            try
            {
                resolvedType = Resolve<ResolveType>(options);
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = default(ResolveType);
                return false;
            }
        }

        /// <summary>
        /// Attemps to resolve a type using the default options and given name
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="name">Name of registration</param>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve<ResolveType>(string name, out ResolveType resolvedType)
            where ResolveType : class
        {
            try
            {
                resolvedType = Resolve<ResolveType>(name);
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = default(ResolveType);
                return false;
            }
        }

        /// <summary>
        /// Attemps to resolve a type using the given options and name
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="name">Name of registration</param>
        /// <param name="options">Resolution options</param>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve<ResolveType>(string name, ResolveOptions options, out ResolveType resolvedType)
            where ResolveType : class
        {
            try
            {
                resolvedType = Resolve<ResolveType>(name, options);
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = default(ResolveType);
                return false;
            }
        }

        /// <summary>
        /// Attemps to resolve a type using the default options and supplied constructor parameters
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve<ResolveType>(NamedParameterOverloads parameters, out ResolveType resolvedType)
            where ResolveType : class
        {
            try
            {
                resolvedType = Resolve<ResolveType>(parameters);
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = default(ResolveType);
                return false;
            }
        }

        /// <summary>
        /// Attemps to resolve a type using the default options and supplied name and constructor parameters
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="name">Name of registration</param>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve<ResolveType>(string name, NamedParameterOverloads parameters, out ResolveType resolvedType)
            where ResolveType : class
        {
            try
            {
                resolvedType = Resolve<ResolveType>(name, parameters);
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = default(ResolveType);
                return false;
            }
        }

        /// <summary>
        /// Attemps to resolve a type using the supplied options and constructor parameters
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <param name="options">Resolution options</param>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve<ResolveType>(NamedParameterOverloads parameters, ResolveOptions options, out ResolveType resolvedType)
            where ResolveType : class
        {
            try
            {
                resolvedType = Resolve<ResolveType>(parameters, options);
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = default(ResolveType);
                return false;
            }
        }

        /// <summary>
        /// Attemps to resolve a type using the supplied name, options and constructor parameters
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolve</typeparam>
        /// <param name="name">Name of registration</param>
        /// <param name="parameters">User specified constructor parameters</param>
        /// <param name="options">Resolution options</param>
        /// <param name="resolvedType">Resolved type or default if resolve fails</param>
        /// <returns>True if resolved sucessfully, false otherwise</returns>
        public bool TryResolve<ResolveType>(string name, NamedParameterOverloads parameters, ResolveOptions options, out ResolveType resolvedType)
            where ResolveType : class
        {
            try
            {
                resolvedType = Resolve<ResolveType>(name, parameters, options);
                return true;
            }
            catch (TinyIoCResolutionException)
            {
                resolvedType = default(ResolveType);
                return false;
            }
        }

        /// <summary>
        /// Returns all registrations of a type
        /// </summary>
        /// <param name="resolveType">Type of the resolve.</param>
        /// <param name="includeUnnamed">Whether to include un-named (default) registrations</param>
        /// <returns>IEnumerable</returns>
        public IEnumerable<object> ResolveAll(Type resolveType, bool includeUnnamed)
        {
            return ResolveAllInternal(resolveType, includeUnnamed);
        }

        /// <summary>
        /// Returns all registrations of a type, both named and unnamed
        /// </summary>
        /// <param name="resolveType">Type of the resolve.</param>
        /// <returns>IEnumerable</returns>
        public IEnumerable<object> ResolveAll(Type resolveType)
        {
            return ResolveAll(resolveType, false);
        }

        /// <summary>
        /// Returns all registrations of a type
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolveAll</typeparam>
        /// <param name="includeUnnamed">Whether to include un-named (default) registrations</param>
        /// <returns>IEnumerable</returns>
        public IEnumerable<ResolveType> ResolveAll<ResolveType>(bool includeUnnamed)
            where ResolveType : class
        {
            return this.ResolveAll(typeof(ResolveType), includeUnnamed).Cast<ResolveType>();
        }

        /// <summary>
        /// Returns all registrations of a type, both named and unnamed
        /// </summary>
        /// <typeparam name="ResolveType">Type to resolveAll</typeparam>
        /// <returns>IEnumerable</returns>
        public IEnumerable<ResolveType> ResolveAll<ResolveType>()
            where ResolveType : class
        {
            return ResolveAll<ResolveType>(true);
        }

        /// <summary>
        /// Attempts to resolve all public property dependencies on the given object.
        /// </summary>
        /// <param name="input">Object to "build up"</param>
        public void BuildUp(object input)
        {
            BuildUpInternal(input, ResolveOptions.Default);
        }

        /// <summary>
        /// Attempts to resolve all public property dependencies on the given object using the given resolve options.
        /// </summary>
        /// <param name="input">Object to "build up"</param>
        /// <param name="resolveOptions">Resolve options to use</param>
        public void BuildUp(object input, ResolveOptions resolveOptions)
        {
            BuildUpInternal(input, resolveOptions);
        }
        #endregion
        #endregion

        #region Object Factories
        /// <summary>
        /// Provides custom lifetime management for ASP.Net per-request lifetimes etc.
        /// </summary>
        public interface ITinyIoCObjectLifetimeProvider
        {
            /// <summary>
            /// Gets the stored object if it exists, or null if not
            /// </summary>
            /// <returns>Object instance or null</returns>
            object GetObject();

            /// <summary>
            /// Store the object
            /// </summary>
            /// <param name="value">Object to store</param>
            void SetObject(object value);

            /// <summary>
            /// Release the object
            /// </summary>
            void ReleaseObject();
        }

        /// <summary>
        /// Class ObjectFactoryBase.
        /// </summary>
        private abstract class ObjectFactoryBase
        {
            /// <summary>
            /// Whether to assume this factory sucessfully constructs its objects
            /// Generally set to true for delegate style factories as CanResolve cannot delve
            /// into the delegates they contain.
            /// </summary>
            /// <value><c>true</c> if [assume construction]; otherwise, <c>false</c>.</value>
            public virtual bool AssumeConstruction { get { return false; } }

            /// <summary>
            /// The type the factory instantiates
            /// </summary>
            /// <value>The type of the creates.</value>
            public abstract Type CreatesType { get; }

            /// <summary>
            /// Constructor to use, if specified
            /// </summary>
            /// <value>The constructor.</value>
            public ConstructorInfo Constructor { get; protected set; }

            /// <summary>
            /// Create the type
            /// </summary>
            /// <param name="requestedType">Type user requested to be resolved</param>
            /// <param name="container">Container that requested the creation</param>
            /// <param name="parameters">Any user parameters passed</param>
            /// <param name="options">The options.</param>
            /// <returns>System.Object.</returns>
            public abstract object GetObject(Type requestedType, TinyIoCContainer container, NamedParameterOverloads parameters, ResolveOptions options);

            /// <summary>
            /// Gets the singleton variant.
            /// </summary>
            /// <value>The singleton variant.</value>
            /// <exception cref="TinyIoC.TinyIoCRegistrationException">singleton</exception>
            public virtual ObjectFactoryBase SingletonVariant
            {
                get
                {
                    throw new TinyIoCRegistrationException(this.GetType(), "singleton");
                }
            }

            /// <summary>
            /// Gets the multi instance variant.
            /// </summary>
            /// <value>The multi instance variant.</value>
            /// <exception cref="TinyIoC.TinyIoCRegistrationException">multi-instance</exception>
            public virtual ObjectFactoryBase MultiInstanceVariant
            {
                get
                {
                    throw new TinyIoCRegistrationException(this.GetType(), "multi-instance");
                }
            }

            /// <summary>
            /// Gets the strong reference variant.
            /// </summary>
            /// <value>The strong reference variant.</value>
            /// <exception cref="TinyIoC.TinyIoCRegistrationException">strong reference</exception>
            public virtual ObjectFactoryBase StrongReferenceVariant
            {
                get
                {
                    throw new TinyIoCRegistrationException(this.GetType(), "strong reference");
                }
            }

            /// <summary>
            /// Gets the weak reference variant.
            /// </summary>
            /// <value>The weak reference variant.</value>
            /// <exception cref="TinyIoC.TinyIoCRegistrationException">weak reference</exception>
            public virtual ObjectFactoryBase WeakReferenceVariant
            {
                get
                {
                    throw new TinyIoCRegistrationException(this.GetType(), "weak reference");
                }
            }

            /// <summary>
            /// Gets the custom object lifetime variant.
            /// </summary>
            /// <param name="lifetimeProvider">The lifetime provider.</param>
            /// <param name="errorString">The error string.</param>
            /// <returns>ObjectFactoryBase.</returns>
            /// <exception cref="TinyIoC.TinyIoCRegistrationException"></exception>
            public virtual ObjectFactoryBase GetCustomObjectLifetimeVariant(ITinyIoCObjectLifetimeProvider lifetimeProvider, string errorString)
            {
                throw new TinyIoCRegistrationException(this.GetType(), errorString);
            }

            /// <summary>
            /// Sets the constructor.
            /// </summary>
            /// <param name="constructor">The constructor.</param>
            public virtual void SetConstructor(ConstructorInfo constructor)
            {
                Constructor = constructor;
            }

            /// <summary>
            /// Gets the factory for child container.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="parent">The parent.</param>
            /// <param name="child">The child.</param>
            /// <returns>ObjectFactoryBase.</returns>
            public virtual ObjectFactoryBase GetFactoryForChildContainer(Type type, TinyIoCContainer parent, TinyIoCContainer child)
            {
                return this;
            }
        }

        /// <summary>
        /// IObjectFactory that creates new instances of types for each resolution
        /// </summary>
        private class MultiInstanceFactory : ObjectFactoryBase
        {
            /// <summary>
            /// The register type
            /// </summary>
            private readonly Type registerType;
            /// <summary>
            /// The register implementation
            /// </summary>
            private readonly Type registerImplementation;
            /// <summary>
            /// The type the factory instantiates
            /// </summary>
            /// <value>The type of the creates.</value>
            public override Type CreatesType { get { return this.registerImplementation; } }

            /// <summary>
            /// Initializes a new instance of the <see cref="MultiInstanceFactory"/> class.
            /// </summary>
            /// <param name="registerType">Type of the register.</param>
            /// <param name="registerImplementation">The register implementation.</param>
            /// <exception cref="TinyIoC.TinyIoCRegistrationTypeException">
            /// MultiInstanceFactory
            /// or
            /// MultiInstanceFactory
            /// </exception>
            public MultiInstanceFactory(Type registerType, Type registerImplementation)
            {
#if NETFX_CORE
                if (registerImplementation.GetTypeInfo().IsAbstract || registerImplementation.GetTypeInfo().IsInterface)
                    throw new TinyIoCRegistrationTypeException(registerImplementation, "MultiInstanceFactory");
#else
                if (registerImplementation.IsAbstract() || registerImplementation.IsInterface())
                    throw new TinyIoCRegistrationTypeException(registerImplementation, "MultiInstanceFactory");
#endif
                if (!IsValidAssignment(registerType, registerImplementation))
                    throw new TinyIoCRegistrationTypeException(registerImplementation, "MultiInstanceFactory");

                this.registerType = registerType;
                this.registerImplementation = registerImplementation;
            }

            /// <summary>
            /// Create the type
            /// </summary>
            /// <param name="requestedType">Type user requested to be resolved</param>
            /// <param name="container">Container that requested the creation</param>
            /// <param name="parameters">Any user parameters passed</param>
            /// <param name="options">The options.</param>
            /// <returns>System.Object.</returns>
            /// <exception cref="TinyIoC.TinyIoCResolutionException"></exception>
            public override object GetObject(Type requestedType, TinyIoCContainer container, NamedParameterOverloads parameters, ResolveOptions options)
            {
                try
                {
                    return container.ConstructType(requestedType, this.registerImplementation, Constructor, parameters, options);
                }
                catch (TinyIoCResolutionException ex)
                {
                    throw new TinyIoCResolutionException(this.registerType, ex);
                }
            }

            /// <summary>
            /// Gets the singleton variant.
            /// </summary>
            /// <value>The singleton variant.</value>
            public override ObjectFactoryBase SingletonVariant
            {
                get
                {
                    return new SingletonFactory(this.registerType, this.registerImplementation);
                }
            }

            /// <summary>
            /// Gets the custom object lifetime variant.
            /// </summary>
            /// <param name="lifetimeProvider">The lifetime provider.</param>
            /// <param name="errorString">The error string.</param>
            /// <returns>ObjectFactoryBase.</returns>
            public override ObjectFactoryBase GetCustomObjectLifetimeVariant(ITinyIoCObjectLifetimeProvider lifetimeProvider, string errorString)
            {
                return new CustomObjectLifetimeFactory(this.registerType, this.registerImplementation, lifetimeProvider, errorString);
            }

            /// <summary>
            /// Gets the multi instance variant.
            /// </summary>
            /// <value>The multi instance variant.</value>
            public override ObjectFactoryBase MultiInstanceVariant
            {
                get
                {
                    return this;
                }
            }
        }

        /// <summary>
        /// IObjectFactory that invokes a specified delegate to construct the object
        /// </summary>
        private class DelegateFactory : ObjectFactoryBase
        {
            /// <summary>
            /// The register type
            /// </summary>
            private readonly Type registerType;

            /// <summary>
            /// The _factory
            /// </summary>
            private Func<TinyIoCContainer, NamedParameterOverloads, object> _factory;

            /// <summary>
            /// Whether to assume this factory sucessfully constructs its objects
            /// Generally set to true for delegate style factories as CanResolve cannot delve
            /// into the delegates they contain.
            /// </summary>
            /// <value><c>true</c> if [assume construction]; otherwise, <c>false</c>.</value>
            public override bool AssumeConstruction { get { return true; } }

            /// <summary>
            /// The type the factory instantiates
            /// </summary>
            /// <value>The type of the creates.</value>
            public override Type CreatesType { get { return this.registerType; } }

            /// <summary>
            /// Create the type
            /// </summary>
            /// <param name="requestedType">Type user requested to be resolved</param>
            /// <param name="container">Container that requested the creation</param>
            /// <param name="parameters">Any user parameters passed</param>
            /// <param name="options">The options.</param>
            /// <returns>System.Object.</returns>
            /// <exception cref="TinyIoC.TinyIoCResolutionException"></exception>
            public override object GetObject(Type requestedType, TinyIoCContainer container, NamedParameterOverloads parameters, ResolveOptions options)
            {
                try
                {
                    return _factory.Invoke(container, parameters);
                }
                catch (Exception ex)
                {
                    throw new TinyIoCResolutionException(this.registerType, ex);
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="DelegateFactory"/> class.
            /// </summary>
            /// <param name="registerType">Type of the register.</param>
            /// <param name="factory">The factory.</param>
            /// <exception cref="System.ArgumentNullException">factory</exception>
            public DelegateFactory( Type registerType, Func<TinyIoCContainer, NamedParameterOverloads, object> factory)
            {
                if (factory == null)
                    throw new ArgumentNullException("factory");

                _factory = factory;

                this.registerType = registerType;
            }

            /// <summary>
            /// Gets the weak reference variant.
            /// </summary>
            /// <value>The weak reference variant.</value>
            public override ObjectFactoryBase WeakReferenceVariant
            {
                get
                {
                    return new WeakDelegateFactory(this.registerType, _factory);
                }
            }

            /// <summary>
            /// Gets the strong reference variant.
            /// </summary>
            /// <value>The strong reference variant.</value>
            public override ObjectFactoryBase StrongReferenceVariant
            {
                get
                {
                    return this;
                }
            }

            /// <summary>
            /// Sets the constructor.
            /// </summary>
            /// <param name="constructor">The constructor.</param>
            /// <exception cref="TinyIoC.TinyIoCConstructorResolutionException">Constructor selection is not possible for delegate factory registrations</exception>
            public override void SetConstructor(ConstructorInfo constructor)
            {
                throw new TinyIoCConstructorResolutionException("Constructor selection is not possible for delegate factory registrations");
            }
        }

        /// <summary>
        /// IObjectFactory that invokes a specified delegate to construct the object
        /// Holds the delegate using a weak reference
        /// </summary>
        private class WeakDelegateFactory : ObjectFactoryBase
        {
            /// <summary>
            /// The register type
            /// </summary>
            private readonly Type registerType;

            /// <summary>
            /// The _factory
            /// </summary>
            private WeakReference _factory;

            /// <summary>
            /// Whether to assume this factory sucessfully constructs its objects
            /// Generally set to true for delegate style factories as CanResolve cannot delve
            /// into the delegates they contain.
            /// </summary>
            /// <value><c>true</c> if [assume construction]; otherwise, <c>false</c>.</value>
            public override bool AssumeConstruction { get { return true; } }

            /// <summary>
            /// The type the factory instantiates
            /// </summary>
            /// <value>The type of the creates.</value>
            public override Type CreatesType { get { return this.registerType; } }

            /// <summary>
            /// Create the type
            /// </summary>
            /// <param name="requestedType">Type user requested to be resolved</param>
            /// <param name="container">Container that requested the creation</param>
            /// <param name="parameters">Any user parameters passed</param>
            /// <param name="options">The options.</param>
            /// <returns>System.Object.</returns>
            /// <exception cref="TinyIoC.TinyIoCWeakReferenceException"></exception>
            /// <exception cref="TinyIoC.TinyIoCResolutionException"></exception>
            public override object GetObject(Type requestedType, TinyIoCContainer container, NamedParameterOverloads parameters, ResolveOptions options)
            {
                var factory = _factory.Target as Func<TinyIoCContainer, NamedParameterOverloads, object>;

                if (factory == null)
                    throw new TinyIoCWeakReferenceException(this.registerType);

                try
                {
                    return factory.Invoke(container, parameters);
                }
                catch (Exception ex)
                {
                    throw new TinyIoCResolutionException(this.registerType, ex);
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="WeakDelegateFactory"/> class.
            /// </summary>
            /// <param name="registerType">Type of the register.</param>
            /// <param name="factory">The factory.</param>
            /// <exception cref="System.ArgumentNullException">factory</exception>
            public WeakDelegateFactory(Type registerType, Func<TinyIoCContainer, NamedParameterOverloads, object> factory)
            {
                if (factory == null)
                    throw new ArgumentNullException("factory");

                _factory = new WeakReference(factory);

                this.registerType = registerType;
            }

            /// <summary>
            /// Gets the strong reference variant.
            /// </summary>
            /// <value>The strong reference variant.</value>
            /// <exception cref="TinyIoC.TinyIoCWeakReferenceException"></exception>
            public override ObjectFactoryBase StrongReferenceVariant
            {
                get
                {
                    var factory = _factory.Target as Func<TinyIoCContainer, NamedParameterOverloads, object>;

                    if (factory == null)
                        throw new TinyIoCWeakReferenceException(this.registerType);

                    return new DelegateFactory(this.registerType, factory);
                }
            }

            /// <summary>
            /// Gets the weak reference variant.
            /// </summary>
            /// <value>The weak reference variant.</value>
            public override ObjectFactoryBase WeakReferenceVariant
            {
                get
                {
                    return this;
                }
            }

            /// <summary>
            /// Sets the constructor.
            /// </summary>
            /// <param name="constructor">The constructor.</param>
            /// <exception cref="TinyIoC.TinyIoCConstructorResolutionException">Constructor selection is not possible for delegate factory registrations</exception>
            public override void SetConstructor(ConstructorInfo constructor)
            {
                throw new TinyIoCConstructorResolutionException("Constructor selection is not possible for delegate factory registrations");
            }
        }

        /// <summary>
        /// Stores an particular instance to return for a type
        /// </summary>
        private class InstanceFactory : ObjectFactoryBase, IDisposable
        {
            /// <summary>
            /// The register type
            /// </summary>
            private readonly Type registerType;
            /// <summary>
            /// The register implementation
            /// </summary>
            private readonly Type registerImplementation;
            /// <summary>
            /// The _instance
            /// </summary>
            private object _instance;

            /// <summary>
            /// Whether to assume this factory sucessfully constructs its objects
            /// Generally set to true for delegate style factories as CanResolve cannot delve
            /// into the delegates they contain.
            /// </summary>
            /// <value><c>true</c> if [assume construction]; otherwise, <c>false</c>.</value>
            public override bool AssumeConstruction { get { return true; } }

            /// <summary>
            /// Initializes a new instance of the <see cref="InstanceFactory"/> class.
            /// </summary>
            /// <param name="registerType">Type of the register.</param>
            /// <param name="registerImplementation">The register implementation.</param>
            /// <param name="instance">The instance.</param>
            /// <exception cref="TinyIoC.TinyIoCRegistrationTypeException">InstanceFactory</exception>
            public InstanceFactory(Type registerType, Type registerImplementation, object instance)
            {
                if (!IsValidAssignment(registerType, registerImplementation))
                    throw new TinyIoCRegistrationTypeException(registerImplementation, "InstanceFactory");

                this.registerType = registerType;
                this.registerImplementation = registerImplementation;
                _instance = instance;
            }

            /// <summary>
            /// The type the factory instantiates
            /// </summary>
            /// <value>The type of the creates.</value>
            public override Type CreatesType
            {
                get { return this.registerImplementation; }
            }

            /// <summary>
            /// Create the type
            /// </summary>
            /// <param name="requestedType">Type user requested to be resolved</param>
            /// <param name="container">Container that requested the creation</param>
            /// <param name="parameters">Any user parameters passed</param>
            /// <param name="options">The options.</param>
            /// <returns>System.Object.</returns>
            public override object GetObject(Type requestedType, TinyIoCContainer container, NamedParameterOverloads parameters, ResolveOptions options)
            {
                return _instance;
            }

            /// <summary>
            /// Gets the multi instance variant.
            /// </summary>
            /// <value>The multi instance variant.</value>
            public override ObjectFactoryBase MultiInstanceVariant
            {
                get { return new MultiInstanceFactory(this.registerType, this.registerImplementation); }
            }

            /// <summary>
            /// Gets the weak reference variant.
            /// </summary>
            /// <value>The weak reference variant.</value>
            public override ObjectFactoryBase WeakReferenceVariant
            {
                get
                {
                    return new WeakInstanceFactory(this.registerType, this.registerImplementation, this._instance);
                }
            }

            /// <summary>
            /// Gets the strong reference variant.
            /// </summary>
            /// <value>The strong reference variant.</value>
            public override ObjectFactoryBase StrongReferenceVariant
            {
                get
                {
                    return this;
                }
            }

            /// <summary>
            /// Sets the constructor.
            /// </summary>
            /// <param name="constructor">The constructor.</param>
            /// <exception cref="TinyIoC.TinyIoCConstructorResolutionException">Constructor selection is not possible for instance factory registrations</exception>
            public override void SetConstructor(ConstructorInfo constructor)
            {
                throw new TinyIoCConstructorResolutionException("Constructor selection is not possible for instance factory registrations");
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                var disposable = _instance as IDisposable;

                if (disposable != null)
                    disposable.Dispose();
            }
        }

        /// <summary>
        /// Stores an particular instance to return for a type
        /// Stores the instance with a weak reference
        /// </summary>
        private class WeakInstanceFactory : ObjectFactoryBase, IDisposable
        {
            /// <summary>
            /// The register type
            /// </summary>
            private readonly Type registerType;
            /// <summary>
            /// The register implementation
            /// </summary>
            private readonly Type registerImplementation;
            /// <summary>
            /// The _instance
            /// </summary>
            private readonly WeakReference _instance;

            /// <summary>
            /// Initializes a new instance of the <see cref="WeakInstanceFactory"/> class.
            /// </summary>
            /// <param name="registerType">Type of the register.</param>
            /// <param name="registerImplementation">The register implementation.</param>
            /// <param name="instance">The instance.</param>
            /// <exception cref="TinyIoC.TinyIoCRegistrationTypeException">WeakInstanceFactory</exception>
            public WeakInstanceFactory(Type registerType, Type registerImplementation, object instance)
            {
                if (!IsValidAssignment(registerType, registerImplementation))
                    throw new TinyIoCRegistrationTypeException(registerImplementation, "WeakInstanceFactory");

                this.registerType = registerType;
                this.registerImplementation = registerImplementation;
                _instance = new WeakReference(instance);
            }

            /// <summary>
            /// The type the factory instantiates
            /// </summary>
            /// <value>The type of the creates.</value>
            public override Type CreatesType
            {
                get { return this.registerImplementation; }
            }

            /// <summary>
            /// Create the type
            /// </summary>
            /// <param name="requestedType">Type user requested to be resolved</param>
            /// <param name="container">Container that requested the creation</param>
            /// <param name="parameters">Any user parameters passed</param>
            /// <param name="options">The options.</param>
            /// <returns>System.Object.</returns>
            /// <exception cref="TinyIoC.TinyIoCWeakReferenceException"></exception>
            public override object GetObject(Type requestedType, TinyIoCContainer container, NamedParameterOverloads parameters, ResolveOptions options)
            {
                var instance = _instance.Target;

                if (instance == null)
                    throw new TinyIoCWeakReferenceException(this.registerType);

                return instance;
            }

            /// <summary>
            /// Gets the multi instance variant.
            /// </summary>
            /// <value>The multi instance variant.</value>
            public override ObjectFactoryBase MultiInstanceVariant
            {
                get
                {
                    return new MultiInstanceFactory(this.registerType, this.registerImplementation);
                }
            }

            /// <summary>
            /// Gets the weak reference variant.
            /// </summary>
            /// <value>The weak reference variant.</value>
            public override ObjectFactoryBase WeakReferenceVariant
            {
                get
                {
                    return this;
                }
            }

            /// <summary>
            /// Gets the strong reference variant.
            /// </summary>
            /// <value>The strong reference variant.</value>
            /// <exception cref="TinyIoC.TinyIoCWeakReferenceException"></exception>
            public override ObjectFactoryBase StrongReferenceVariant
            {
                get
                {
                    var instance = _instance.Target;

                    if (instance == null)
                        throw new TinyIoCWeakReferenceException(this.registerType);

                    return new InstanceFactory(this.registerType, this.registerImplementation, instance);
                }
            }

            /// <summary>
            /// Sets the constructor.
            /// </summary>
            /// <param name="constructor">The constructor.</param>
            /// <exception cref="TinyIoC.TinyIoCConstructorResolutionException">Constructor selection is not possible for instance factory registrations</exception>
            public override void SetConstructor(ConstructorInfo constructor)
            {
                throw new TinyIoCConstructorResolutionException("Constructor selection is not possible for instance factory registrations");
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                var disposable = _instance.Target as IDisposable;

                if (disposable != null)
                    disposable.Dispose();
            }
        }

        /// <summary>
        /// A factory that lazy instantiates a type and always returns the same instance
        /// </summary>
        private class SingletonFactory : ObjectFactoryBase, IDisposable
        {
            /// <summary>
            /// The register type
            /// </summary>
            private readonly Type registerType;
            /// <summary>
            /// The register implementation
            /// </summary>
            private readonly Type registerImplementation;
            /// <summary>
            /// The singleton lock
            /// </summary>
            private readonly object SingletonLock = new object();
            /// <summary>
            /// The _ current
            /// </summary>
            private object _Current;

            /// <summary>
            /// Initializes a new instance of the <see cref="SingletonFactory"/> class.
            /// </summary>
            /// <param name="registerType">Type of the register.</param>
            /// <param name="registerImplementation">The register implementation.</param>
            /// <exception cref="TinyIoC.TinyIoCRegistrationTypeException">
            /// SingletonFactory
            /// or
            /// SingletonFactory
            /// </exception>
            public SingletonFactory(Type registerType, Type registerImplementation)
            {
#if NETFX_CORE
                if (registerImplementation.GetTypeInfo().IsAbstract || registerImplementation.GetTypeInfo().IsInterface)
#else
                if (registerImplementation.IsAbstract() || registerImplementation.IsInterface())
#endif
                    throw new TinyIoCRegistrationTypeException(registerImplementation, "SingletonFactory");

                if (!IsValidAssignment(registerType, registerImplementation))
                    throw new TinyIoCRegistrationTypeException(registerImplementation, "SingletonFactory");

                this.registerType = registerType;
                this.registerImplementation = registerImplementation;
            }

            /// <summary>
            /// The type the factory instantiates
            /// </summary>
            /// <value>The type of the creates.</value>
            public override Type CreatesType
            {
                get { return this.registerImplementation; }
            }

            /// <summary>
            /// Create the type
            /// </summary>
            /// <param name="requestedType">Type user requested to be resolved</param>
            /// <param name="container">Container that requested the creation</param>
            /// <param name="parameters">Any user parameters passed</param>
            /// <param name="options">The options.</param>
            /// <returns>System.Object.</returns>
            /// <exception cref="System.ArgumentException">Cannot specify parameters for singleton types</exception>
            public override object GetObject(Type requestedType, TinyIoCContainer container, NamedParameterOverloads parameters, ResolveOptions options)
            {
                if (parameters.Count != 0)
                    throw new ArgumentException("Cannot specify parameters for singleton types");

                lock (SingletonLock)
                    if (_Current == null)
                        _Current = container.ConstructType(requestedType, this.registerImplementation, Constructor, options);

                return _Current;
            }

            /// <summary>
            /// Gets the singleton variant.
            /// </summary>
            /// <value>The singleton variant.</value>
            public override ObjectFactoryBase SingletonVariant
            {
                get
                {
                    return this;
                }
            }

            /// <summary>
            /// Gets the custom object lifetime variant.
            /// </summary>
            /// <param name="lifetimeProvider">The lifetime provider.</param>
            /// <param name="errorString">The error string.</param>
            /// <returns>ObjectFactoryBase.</returns>
            public override ObjectFactoryBase GetCustomObjectLifetimeVariant(ITinyIoCObjectLifetimeProvider lifetimeProvider, string errorString)
            {
                return new CustomObjectLifetimeFactory(this.registerType, this.registerImplementation, lifetimeProvider, errorString);
            }

            /// <summary>
            /// Gets the multi instance variant.
            /// </summary>
            /// <value>The multi instance variant.</value>
            public override ObjectFactoryBase MultiInstanceVariant
            {
                get
                {
                    return new MultiInstanceFactory(this.registerType, this.registerImplementation);
                }
            }

            /// <summary>
            /// Gets the factory for child container.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="parent">The parent.</param>
            /// <param name="child">The child.</param>
            /// <returns>ObjectFactoryBase.</returns>
            public override ObjectFactoryBase GetFactoryForChildContainer(Type type, TinyIoCContainer parent, TinyIoCContainer child)
            {
                // We make sure that the singleton is constructed before the child container takes the factory.
                // Otherwise the results would vary depending on whether or not the parent container had resolved
                // the type before the child container does.
                GetObject(type, parent, NamedParameterOverloads.Default, ResolveOptions.Default);
                return this;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (this._Current == null)
                    return;

                var disposable = this._Current as IDisposable;

                if (disposable != null)
                    disposable.Dispose();
            }
        }

        /// <summary>
        /// A factory that offloads lifetime to an external lifetime provider
        /// </summary>
        private class CustomObjectLifetimeFactory : ObjectFactoryBase, IDisposable
        {
            /// <summary>
            /// The singleton lock
            /// </summary>
            private readonly object SingletonLock = new object();
            /// <summary>
            /// The register type
            /// </summary>
            private readonly Type registerType;
            /// <summary>
            /// The register implementation
            /// </summary>
            private readonly Type registerImplementation;
            /// <summary>
            /// The _ lifetime provider
            /// </summary>
            private readonly ITinyIoCObjectLifetimeProvider _LifetimeProvider;

            /// <summary>
            /// Initializes a new instance of the <see cref="CustomObjectLifetimeFactory"/> class.
            /// </summary>
            /// <param name="registerType">Type of the register.</param>
            /// <param name="registerImplementation">The register implementation.</param>
            /// <param name="lifetimeProvider">The lifetime provider.</param>
            /// <param name="errorMessage">The error message.</param>
            /// <exception cref="System.ArgumentNullException">lifetimeProvider;lifetimeProvider is null.</exception>
            /// <exception cref="TinyIoC.TinyIoCRegistrationTypeException">
            /// SingletonFactory
            /// or
            /// </exception>
            public CustomObjectLifetimeFactory(Type registerType, Type registerImplementation, ITinyIoCObjectLifetimeProvider lifetimeProvider, string errorMessage)
            {
                if (lifetimeProvider == null)
                    throw new ArgumentNullException("lifetimeProvider", "lifetimeProvider is null.");

                if (!IsValidAssignment(registerType, registerImplementation))
                    throw new TinyIoCRegistrationTypeException(registerImplementation, "SingletonFactory");

#if NETFX_CORE
                if (registerImplementation.GetTypeInfo().IsAbstract || registerImplementation.GetTypeInfo().IsInterface)
#else
                if (registerImplementation.IsAbstract() || registerImplementation.IsInterface())
#endif
                    throw new TinyIoCRegistrationTypeException(registerImplementation, errorMessage);

                this.registerType = registerType;
                this.registerImplementation = registerImplementation;
                _LifetimeProvider = lifetimeProvider;
            }

            /// <summary>
            /// The type the factory instantiates
            /// </summary>
            /// <value>The type of the creates.</value>
            public override Type CreatesType
            {
                get { return this.registerImplementation; }
            }

            /// <summary>
            /// Create the type
            /// </summary>
            /// <param name="requestedType">Type user requested to be resolved</param>
            /// <param name="container">Container that requested the creation</param>
            /// <param name="parameters">Any user parameters passed</param>
            /// <param name="options">The options.</param>
            /// <returns>System.Object.</returns>
            public override object GetObject(Type requestedType, TinyIoCContainer container, NamedParameterOverloads parameters, ResolveOptions options)
            {
                object current;

                lock (SingletonLock)
                {
                    current = _LifetimeProvider.GetObject();
                    if (current == null)
                    {
                        current = container.ConstructType(requestedType, this.registerImplementation, Constructor, options);
                        _LifetimeProvider.SetObject(current);
                    }
                }

                return current;
            }

            /// <summary>
            /// Gets the singleton variant.
            /// </summary>
            /// <value>The singleton variant.</value>
            public override ObjectFactoryBase SingletonVariant
            {
                get
                {
                    _LifetimeProvider.ReleaseObject();
                    return new SingletonFactory(this.registerType, this.registerImplementation);
                }
            }

            /// <summary>
            /// Gets the multi instance variant.
            /// </summary>
            /// <value>The multi instance variant.</value>
            public override ObjectFactoryBase MultiInstanceVariant
            {
                get
                {
                    _LifetimeProvider.ReleaseObject();
                    return new MultiInstanceFactory(this.registerType, this.registerImplementation);
                }
            }

            /// <summary>
            /// Gets the custom object lifetime variant.
            /// </summary>
            /// <param name="lifetimeProvider">The lifetime provider.</param>
            /// <param name="errorString">The error string.</param>
            /// <returns>ObjectFactoryBase.</returns>
            public override ObjectFactoryBase GetCustomObjectLifetimeVariant(ITinyIoCObjectLifetimeProvider lifetimeProvider, string errorString)
            {
                _LifetimeProvider.ReleaseObject();
                return new CustomObjectLifetimeFactory(this.registerType, this.registerImplementation, lifetimeProvider, errorString);
            }

            /// <summary>
            /// Gets the factory for child container.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="parent">The parent.</param>
            /// <param name="child">The child.</param>
            /// <returns>ObjectFactoryBase.</returns>
            public override ObjectFactoryBase GetFactoryForChildContainer(Type type, TinyIoCContainer parent, TinyIoCContainer child)
            {
                // We make sure that the singleton is constructed before the child container takes the factory.
                // Otherwise the results would vary depending on whether or not the parent container had resolved
                // the type before the child container does.
                GetObject(type, parent, NamedParameterOverloads.Default, ResolveOptions.Default);
                return this;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                _LifetimeProvider.ReleaseObject();
            }
        }
        #endregion

        #region Singleton Container
        /// <summary>
        /// The _ current
        /// </summary>
        private static readonly TinyIoCContainer _Current = new TinyIoCContainer();

        /// <summary>
        /// Initializes static members of the <see cref="TinyIoCContainer"/> class.
        /// </summary>
        static TinyIoCContainer()
        {
        }

        /// <summary>
        /// Lazy created Singleton instance of the container for simple scenarios
        /// </summary>
        /// <value>The current.</value>
        public static TinyIoCContainer Current
        {
            get
            {
                return _Current;
            }
        }
        #endregion

        #region Type Registrations
        /// <summary>
        /// Class TypeRegistration. This class cannot be inherited.
        /// </summary>
        public sealed class TypeRegistration
        {
            /// <summary>
            /// The _hash code
            /// </summary>
            private int _hashCode;

            /// <summary>
            /// Gets the type.
            /// </summary>
            /// <value>The type.</value>
            public Type Type { get; private set; }
            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="TypeRegistration"/> class.
            /// </summary>
            /// <param name="type">The type.</param>
            public TypeRegistration(Type type)
                : this(type, string.Empty)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TypeRegistration"/> class.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="name">The name.</param>
            public TypeRegistration(Type type, string name)
            {
                Type = type;
                Name = name;

                _hashCode = String.Concat(Type.FullName, "|", Name).GetHashCode();
            }

            /// <summary>
            /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
            /// </summary>
            /// <param name="obj">The object to compare with the current object.</param>
            /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
            public override bool Equals(object obj)
            {
                var typeRegistration = obj as TypeRegistration;

                if (typeRegistration == null)
                    return false;

                if (Type != typeRegistration.Type)
                    return false;

                if (String.Compare(Name, typeRegistration.Name, StringComparison.Ordinal) != 0)
                    return false;

                return true;
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
            public override int GetHashCode()
            {
                return _hashCode;
            }
        }
        /// <summary>
        /// The _ registered types
        /// </summary>
        private readonly SafeDictionary<TypeRegistration, ObjectFactoryBase> _RegisteredTypes;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCContainer"/> class.
        /// </summary>
        public TinyIoCContainer()
        {
            _RegisteredTypes = new SafeDictionary<TypeRegistration, ObjectFactoryBase>();

            RegisterDefaultTypes();
        }

        /// <summary>
        /// The _ parent
        /// </summary>
        TinyIoCContainer _Parent;
        /// <summary>
        /// Initializes a new instance of the <see cref="TinyIoCContainer"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        private TinyIoCContainer(TinyIoCContainer parent)
            : this()
        {
            _Parent = parent;
        }
        #endregion

        #region Internal Methods
        /// <summary>
        /// The _ automatic register lock
        /// </summary>
        private readonly object _AutoRegisterLock = new object();
        /// <summary>
        /// Automatics the register internal.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="ignoreDuplicateImplementations">if set to <c>true</c> [ignore duplicate implementations].</param>
        /// <param name="registrationPredicate">The registration predicate.</param>
        /// <exception cref="TinyIoC.TinyIoCAutoRegistrationException"></exception>
        private void AutoRegisterInternal(IEnumerable<Assembly> assemblies, bool ignoreDuplicateImplementations, Func<Type, bool> registrationPredicate)
        {
            lock (_AutoRegisterLock)
            {
                var types = assemblies.SelectMany(a => a.SafeGetTypes()).Where(t => !IsIgnoredType(t, registrationPredicate)).ToList();

                var concreteTypes = from type in types
                                    where (type.GetTypeInfo().IsClass == true) && (type.GetTypeInfo().IsAbstract == false) && (type != this.GetType() && (type.DeclaringType != this.GetType()) && (!type.GetTypeInfo().IsGenericTypeDefinition))
                                    select type;

                foreach (var type in concreteTypes)
                {
                    try
                    {
                        RegisterInternal(type, string.Empty, GetDefaultObjectFactory(type, type));
                    }
                    catch (MethodAccessException)
                    {
                        // Ignore methods we can't access - added for Silverlight
                    }
                }

                var abstractInterfaceTypes = from type in types
                                             where ((type.GetTypeInfo().IsInterface == true || type.GetTypeInfo().IsAbstract == true) && (type.DeclaringType != this.GetType()) && (!type.GetTypeInfo().IsGenericTypeDefinition))
                                             select type;

                foreach (var type in abstractInterfaceTypes)
                {
                    var implementations = from implementationType in concreteTypes
                                          where implementationType.GetInterfaces().Contains(type) || implementationType.GetTypeInfo().BaseType == type
                                          select implementationType;

                    if (!ignoreDuplicateImplementations && implementations.Count() > 1)
                        throw new TinyIoCAutoRegistrationException(type, implementations);

                    var firstImplementation = implementations.FirstOrDefault();
                    if (firstImplementation != null)
                    {
                        try
                        {
                            RegisterInternal(type, string.Empty, GetDefaultObjectFactory(type, firstImplementation));
                        }
                        catch (MethodAccessException)
                        {
                            // Ignore methods we can't access - added for Silverlight
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether [is ignored assembly] [the specified assembly].
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns><c>true</c> if [is ignored assembly] [the specified assembly]; otherwise, <c>false</c>.</returns>
        private bool IsIgnoredAssembly(Assembly assembly)
        {
            // TODO - find a better way to remove "system" assemblies from the auto registration
            var ignoreChecks = new List<Func<Assembly, bool>>()
            {
                asm => asm.FullName.StartsWith("Microsoft.", StringComparison.Ordinal),
                asm => asm.FullName.StartsWith("System.", StringComparison.Ordinal),
                asm => asm.FullName.StartsWith("System,", StringComparison.Ordinal),
                asm => asm.FullName.StartsWith("CR_ExtUnitTest", StringComparison.Ordinal),
                asm => asm.FullName.StartsWith("mscorlib,", StringComparison.Ordinal),
                asm => asm.FullName.StartsWith("CR_VSTest", StringComparison.Ordinal),
                asm => asm.FullName.StartsWith("DevExpress.CodeRush", StringComparison.Ordinal),
            };

            foreach (var check in ignoreChecks)
            {
                if (check(assembly))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is ignored type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="registrationPredicate">The registration predicate.</param>
        /// <returns><c>true</c> if [is ignored type] [the specified type]; otherwise, <c>false</c>.</returns>
        private bool IsIgnoredType(Type type, Func<Type, bool> registrationPredicate)
        {
            // TODO - find a better way to remove "system" types from the auto registration
            var ignoreChecks = new List<Func<Type, bool>>()
            {
                t => t.FullName.StartsWith("System.", StringComparison.Ordinal),
                t => t.FullName.StartsWith("Microsoft.", StringComparison.Ordinal),
#if NETFX_CORE
                t => t.GetTypeInfo().IsPrimitive,
#else
                t => t.IsPrimitive(),
#endif
#if !UNBOUND_GENERICS_GETCONSTRUCTORS
                t => t.IsGenericTypeDefinition(),
#endif
#if NETFX_CORE
                t => (t.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length == 0) && !(t.GetTypeInfo().IsInterface || t.GetTypeInfo().IsAbstract),
#else
                t => (t.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length == 0) && !(t.IsInterface() || t.IsAbstract()),
#endif
            };

            if (registrationPredicate != null)
            {
                ignoreChecks.Add(t => !registrationPredicate(t));
            }

            foreach (var check in ignoreChecks)
            {
                if (check(type))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Registers the default types.
        /// </summary>
        private void RegisterDefaultTypes()
        {
            Register<TinyIoCContainer>(this);

#if TINYMESSENGER
            // Only register the TinyMessenger singleton if we are the root container
            if (_Parent == null)
                Register<TinyMessenger.ITinyMessengerHub, TinyMessenger.TinyMessengerHub>();
#endif
        }

        /// <summary>
        /// Gets the current factory.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>ObjectFactoryBase.</returns>
        private ObjectFactoryBase GetCurrentFactory(TypeRegistration registration)
        {
            ObjectFactoryBase current = null;

            _RegisteredTypes.TryGetValue(registration, out current);

            return current;
        }

        /// <summary>
        /// Registers the internal.
        /// </summary>
        /// <param name="registerType">Type of the register.</param>
        /// <param name="name">The name.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>RegisterOptions.</returns>
        private RegisterOptions RegisterInternal(Type registerType, string name, ObjectFactoryBase factory)
        {
            var typeRegistration = new TypeRegistration(registerType, name);

            return AddUpdateRegistration(typeRegistration, factory);
        }

        /// <summary>
        /// Adds the update registration.
        /// </summary>
        /// <param name="typeRegistration">The type registration.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>RegisterOptions.</returns>
        private RegisterOptions AddUpdateRegistration(TypeRegistration typeRegistration, ObjectFactoryBase factory)
        {
            _RegisteredTypes[typeRegistration] = factory;

            return new RegisterOptions(this, typeRegistration);
        }

        /// <summary>
        /// Removes the registration.
        /// </summary>
        /// <param name="typeRegistration">The type registration.</param>
        private void RemoveRegistration(TypeRegistration typeRegistration)
        {
            _RegisteredTypes.Remove(typeRegistration);
        }

        /// <summary>
        /// Gets the default object factory.
        /// </summary>
        /// <param name="registerType">Type of the register.</param>
        /// <param name="registerImplementation">The register implementation.</param>
        /// <returns>ObjectFactoryBase.</returns>
        private ObjectFactoryBase GetDefaultObjectFactory(Type registerType, Type registerImplementation)
        {
#if NETFX_CORE
            if (registerType.GetTypeInfo().IsInterface || registerType.GetTypeInfo().IsAbstract)
#else
            if (registerType.IsInterface() || registerType.IsAbstract())
#endif
                return new SingletonFactory(registerType, registerImplementation);

            return new MultiInstanceFactory(registerType, registerImplementation);
        }

        /// <summary>
        /// Determines whether this instance [can resolve internal] the specified registration.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="options">The options.</param>
        /// <returns><c>true</c> if this instance [can resolve internal] the specified registration; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">parameters</exception>
        private bool CanResolveInternal(TypeRegistration registration, NamedParameterOverloads parameters, ResolveOptions options)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            Type checkType = registration.Type;
            string name = registration.Name;

            ObjectFactoryBase factory;
            if (_RegisteredTypes.TryGetValue(new TypeRegistration(checkType, name), out factory))
            {
                if (factory.AssumeConstruction)
                    return true;

                if (factory.Constructor == null)
                    return (GetBestConstructor(factory.CreatesType, parameters, options) != null) ? true : false;
                else
                    return CanConstruct(factory.Constructor, parameters, options);
            }

            // Fail if requesting named resolution and settings set to fail if unresolved
            // Or bubble up if we have a parent
            if (!String.IsNullOrEmpty(name) && options.NamedResolutionFailureAction == NamedResolutionFailureActions.Fail)
                return (_Parent != null) ? _Parent.CanResolveInternal(registration, parameters, options) : false;

            // Attemped unnamed fallback container resolution if relevant and requested
            if (!String.IsNullOrEmpty(name) && options.NamedResolutionFailureAction == NamedResolutionFailureActions.AttemptUnnamedResolution)
            {
                if (_RegisteredTypes.TryGetValue(new TypeRegistration(checkType), out factory))
                {
                    if (factory.AssumeConstruction)
                        return true;

                    return (GetBestConstructor(factory.CreatesType, parameters, options) != null) ? true : false;
                }
            }

            // Check if type is an automatic lazy factory request
            if (IsAutomaticLazyFactoryRequest(checkType))
                return true;

            // Check if type is an IEnumerable<ResolveType>
            if (IsIEnumerableRequest(registration.Type))
                return true;

            // Attempt unregistered construction if possible and requested
            // If we cant', bubble if we have a parent
#if NETFX_CORE
            if ((options.UnregisteredResolutionAction == UnregisteredResolutionActions.AttemptResolve) || (checkType.GetTypeInfo().IsGenericType && options.UnregisteredResolutionAction == UnregisteredResolutionActions.GenericsOnly))
#else
            if ((options.UnregisteredResolutionAction == UnregisteredResolutionActions.AttemptResolve) || (checkType.IsGenericType() && options.UnregisteredResolutionAction == UnregisteredResolutionActions.GenericsOnly))
#endif
                return (GetBestConstructor(checkType, parameters, options) != null) ? true : (_Parent != null) ? _Parent.CanResolveInternal(registration, parameters, options) : false;

                // Bubble resolution up the container tree if we have a parent
                if (_Parent != null)
                return _Parent.CanResolveInternal(registration, parameters, options);

            return false;
        }

        /// <summary>
        /// Determines whether [is i enumerable request] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is i enumerable request] [the specified type]; otherwise, <c>false</c>.</returns>
        private bool IsIEnumerableRequest(Type type)
        {
#if NETFX_CORE
            if (!type.GetTypeInfo().IsGenericType)
#else
            if (!type.IsGenericType())
#endif
                return false;

            Type genericType = type.GetGenericTypeDefinition();

            if (genericType == typeof(IEnumerable<>))
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether [is automatic lazy factory request] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is automatic lazy factory request] [the specified type]; otherwise, <c>false</c>.</returns>
        private bool IsAutomaticLazyFactoryRequest(Type type)
        {
#if NETFX_CORE
            if (!type.GetTypeInfo().IsGenericType)
#else
            if (!type.IsGenericType())
#endif
                return false;

                Type genericType = type.GetGenericTypeDefinition();

            // Just a func
            if (genericType == typeof(Func<>))
                return true;

            // 2 parameter func with string as first parameter (name)
#if NETFX_CORE
            if ((genericType == typeof(Func<,>) && type.GetTypeInfo().GenericTypeArguments[0] == typeof(string)))
#else
            if ((genericType == typeof(Func<,>) && type.GetGenericArguments()[0] == typeof(string)))
#endif
                return true;

            // 3 parameter func with string as first parameter (name) and IDictionary<string, object> as second (parameters)
#if NETFX_CORE
            if ((genericType == typeof(Func<,,>) && type.GetTypeInfo().GenericTypeArguments[0] == typeof(string) && type.GetTypeInfo().GenericTypeArguments[1] == typeof(IDictionary<String, object>)))
#else
            if ((genericType == typeof(Func<,,>) && type.GetGenericArguments()[0] == typeof(string) && type.GetGenericArguments()[1] == typeof(IDictionary<String, object>)))
#endif
                return true;

            return false;
        }

        /// <summary>
        /// Gets the parent object factory.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>ObjectFactoryBase.</returns>
        private ObjectFactoryBase GetParentObjectFactory(TypeRegistration registration)
        {
            if (_Parent == null)
                return null;

            ObjectFactoryBase factory;
            if (_Parent._RegisteredTypes.TryGetValue(registration, out factory))
            {
                return factory.GetFactoryForChildContainer(registration.Type, _Parent, this);
            }

            return _Parent.GetParentObjectFactory(registration);
        }

        /// <summary>
        /// Resolves the internal.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="options">The options.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="TinyIoC.TinyIoCResolutionException">
        /// </exception>
        private object ResolveInternal(TypeRegistration registration, NamedParameterOverloads parameters, ResolveOptions options)
        {
            ObjectFactoryBase factory;

            // Attempt container resolution
            if (_RegisteredTypes.TryGetValue(registration, out factory))
            {
                try
                {
                    return factory.GetObject(registration.Type, this, parameters, options);
                }
                catch (TinyIoCResolutionException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new TinyIoCResolutionException(registration.Type, ex);
                }
            }

#if RESOLVE_OPEN_GENERICS
            // Attempt container resolution of open generic
            if (registration.Type.IsGenericType())
            {
                var openTypeRegistration = new TypeRegistration(registration.Type.GetGenericTypeDefinition(),
                                                                registration.Name);

                if (_RegisteredTypes.TryGetValue(openTypeRegistration, out factory))
                {
                    try
                    {
                        return factory.GetObject(registration.Type, this, parameters, options);
                    }
                    catch (TinyIoCResolutionException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new TinyIoCResolutionException(registration.Type, ex);
                    }
                }
            }
#endif

            // Attempt to get a factory from parent if we can
            var bubbledObjectFactory = GetParentObjectFactory(registration);
            if (bubbledObjectFactory != null)
            {
                try
                {
                    return bubbledObjectFactory.GetObject(registration.Type, this, parameters, options);
                }
                catch (TinyIoCResolutionException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new TinyIoCResolutionException(registration.Type, ex);
                }
            }

            // Fail if requesting named resolution and settings set to fail if unresolved
            if (!String.IsNullOrEmpty(registration.Name) && options.NamedResolutionFailureAction == NamedResolutionFailureActions.Fail)
                throw new TinyIoCResolutionException(registration.Type);

            // Attemped unnamed fallback container resolution if relevant and requested
            if (!String.IsNullOrEmpty(registration.Name) && options.NamedResolutionFailureAction == NamedResolutionFailureActions.AttemptUnnamedResolution)
            {
                if (_RegisteredTypes.TryGetValue(new TypeRegistration(registration.Type, string.Empty), out factory))
                {
                    try
                    {
                        return factory.GetObject(registration.Type, this, parameters, options);
                    }
                    catch (TinyIoCResolutionException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new TinyIoCResolutionException(registration.Type, ex);
                    }
                }
            }

#if EXPRESSIONS
            // Attempt to construct an automatic lazy factory if possible
            if (IsAutomaticLazyFactoryRequest(registration.Type))
                return GetLazyAutomaticFactoryRequest(registration.Type);
#endif
            if (IsIEnumerableRequest(registration.Type))
                return GetIEnumerableRequest(registration.Type);

            // Attempt unregistered construction if possible and requested
#if NETFX_CORE
            var registrationType = registration.Type.GetTypeInfo();
            if ((options.UnregisteredResolutionAction == UnregisteredResolutionActions.AttemptResolve) || (registrationType.IsGenericType && options.UnregisteredResolutionAction == UnregisteredResolutionActions.GenericsOnly))
            {
                if (!registrationType.IsAbstract && !registrationType.IsInterface)
                    return ConstructType(null, registration.Type, parameters, options);
            }
#else
            if ((options.UnregisteredResolutionAction == UnregisteredResolutionActions.AttemptResolve) || (registration.Type.IsGenericType() && options.UnregisteredResolutionAction == UnregisteredResolutionActions.GenericsOnly))
            {
                if (!registration.Type.IsAbstract() && !registration.Type.IsInterface())
                    return ConstructType(null, registration.Type, parameters, options);
            }
#endif
            // Unable to resolve - throw
            throw new TinyIoCResolutionException(registration.Type);
        }

#if EXPRESSIONS
        private object GetLazyAutomaticFactoryRequest(Type type)
        {
#if NETFX_CORE
            if (!type.GetTypeInfo().IsGenericType)
#else
            if (!type.IsGenericType())
#endif
                return null;

            Type genericType = type.GetGenericTypeDefinition();
#if NETFX_CORE
            Type[] genericArguments = type.GetTypeInfo().GenericTypeArguments.ToArray();
#else
            Type[] genericArguments = type.GetGenericArguments();
#endif

            // Just a func
            if (genericType == typeof(Func<>))
            {
                Type returnType = genericArguments[0];

#if NETFX_CORE
                MethodInfo resolveMethod = typeof(TinyIoCContainer).GetTypeInfo().GetDeclaredMethods("Resolve").First(mi => !mi.GetParameters().Any());
#else
                MethodInfo resolveMethod = typeof(TinyIoCContainer).GetMethod("Resolve", new Type[] { });
#endif
                resolveMethod = resolveMethod.MakeGenericMethod(returnType);

                var resolveCall = Expression.Call(Expression.Constant(this), resolveMethod);

                var resolveLambda = Expression.Lambda(resolveCall).Compile();

                return resolveLambda;
            }

            // 2 parameter func with string as first parameter (name)
            if ((genericType == typeof(Func<,>)) && (genericArguments[0] == typeof(string)))
            {
                Type returnType = genericArguments[1];

#if NETFX_CORE
                MethodInfo resolveMethod = typeof(TinyIoCContainer).GetTypeInfo().GetDeclaredMethods("Resolve").First(mi => mi.GetParameters().Length == 1 && mi.GetParameters()[0].GetType() == typeof(String));
#else
                MethodInfo resolveMethod = typeof(TinyIoCContainer).GetMethod("Resolve", new Type[] { typeof(String) });
#endif
                resolveMethod = resolveMethod.MakeGenericMethod(returnType);

                ParameterExpression[] resolveParameters = new ParameterExpression[] { Expression.Parameter(typeof(String), "name") };
                var resolveCall = Expression.Call(Expression.Constant(this), resolveMethod, resolveParameters);

                var resolveLambda = Expression.Lambda(resolveCall, resolveParameters).Compile();

                return resolveLambda;
            }

            // 3 parameter func with string as first parameter (name) and IDictionary<string, object> as second (parameters)
#if NETFX_CORE
            if ((genericType == typeof(Func<,,>) && type.GenericTypeArguments[0] == typeof(string) && type.GenericTypeArguments[1] == typeof(IDictionary<string, object>)))
#else
            if ((genericType == typeof(Func<,,>) && type.GetGenericArguments()[0] == typeof(string) && type.GetGenericArguments()[1] == typeof(IDictionary<string, object>)))
#endif
            {
                Type returnType = genericArguments[2];

                var name = Expression.Parameter(typeof(string), "name");
                var parameters = Expression.Parameter(typeof(IDictionary<string, object>), "parameters");

#if NETFX_CORE
                MethodInfo resolveMethod = typeof(TinyIoCContainer).GetTypeInfo().GetDeclaredMethods("Resolve").First(mi => mi.GetParameters().Length == 2 && mi.GetParameters()[0].GetType() == typeof(String) && mi.GetParameters()[1].GetType() == typeof(NamedParameterOverloads));
#else
                MethodInfo resolveMethod = typeof(TinyIoCContainer).GetMethod("Resolve", new Type[] { typeof(String), typeof(NamedParameterOverloads) });
#endif
                resolveMethod = resolveMethod.MakeGenericMethod(returnType);

                var resolveCall = Expression.Call(Expression.Constant(this), resolveMethod, name, Expression.Call(typeof(NamedParameterOverloads), "FromIDictionary", null, parameters));

                var resolveLambda = Expression.Lambda(resolveCall, name, parameters).Compile();

                return resolveLambda;
            }

            throw new TinyIoCResolutionException(type);
        }
#endif
                /// <summary>
                /// Gets the i enumerable request.
                /// </summary>
                /// <param name="type">The type.</param>
                /// <returns>System.Object.</returns>
        private object GetIEnumerableRequest(Type type)
        {
#if NETFX_CORE
            var genericResolveAllMethod = this.GetType().GetGenericMethod(BindingFlags.Public | BindingFlags.Instance, "ResolveAll", type.GenericTypeArguments, new[] { typeof(bool) });
#else
            var genericResolveAllMethod = this.GetType().GetGenericMethod(BindingFlags.Public | BindingFlags.Instance, "ResolveAll", type.GetGenericArguments(), new[] { typeof(bool) });
#endif

            return genericResolveAllMethod.Invoke(this, new object[] { false });
        }

        /// <summary>
        /// Determines whether this instance can construct the specified ctor.
        /// </summary>
        /// <param name="ctor">The ctor.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="options">The options.</param>
        /// <returns><c>true</c> if this instance can construct the specified ctor; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">parameters</exception>
        private bool CanConstruct(ConstructorInfo ctor, NamedParameterOverloads parameters, ResolveOptions options)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            foreach (var parameter in ctor.GetParameters())
            {
                if (string.IsNullOrEmpty(parameter.Name))
                    return false;

                var isParameterOverload = parameters.ContainsKey(parameter.Name);

#if NETFX_CORE
                if (parameter.ParameterType.GetTypeInfo().IsPrimitive && !isParameterOverload)
#else
                if (parameter.ParameterType.IsPrimitive() && !isParameterOverload)
#endif
                    return false;

                if (!isParameterOverload && !CanResolveInternal(new TypeRegistration(parameter.ParameterType), NamedParameterOverloads.Default, options))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the best constructor.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="options">The options.</param>
        /// <returns>ConstructorInfo.</returns>
        /// <exception cref="System.ArgumentNullException">parameters</exception>
        private ConstructorInfo GetBestConstructor(Type type, NamedParameterOverloads parameters, ResolveOptions options)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

#if NETFX_CORE
            if (type.GetTypeInfo().IsValueType)
#else
            if (type.IsValueType())
#endif
                return null;

            // Get constructors in reverse order based on the number of parameters
            // i.e. be as "greedy" as possible so we satify the most amount of dependencies possible
            var ctors = this.GetTypeConstructors(type);

            return ctors.FirstOrDefault(ctor => this.CanConstruct(ctor, parameters, options));
        }

        /// <summary>
        /// Gets the type constructors.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>IEnumerable&lt;ConstructorInfo&gt;.</returns>
        private IEnumerable<ConstructorInfo> GetTypeConstructors(Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().DeclaredConstructors.OrderByDescending(ctor => ctor.GetParameters().Count());
#else
            return type.GetConstructors().OrderByDescending(ctor => ctor.GetParameters().Count());
#endif
        }

        /// <summary>
        /// Constructs the type.
        /// </summary>
        /// <param name="requestedType">Type of the requested.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="options">The options.</param>
        /// <returns>System.Object.</returns>
        private object ConstructType(Type requestedType, Type implementationType, ResolveOptions options)
        {
            return ConstructType(requestedType, implementationType, null, NamedParameterOverloads.Default, options);
        }

        /// <summary>
        /// Constructs the type.
        /// </summary>
        /// <param name="requestedType">Type of the requested.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="constructor">The constructor.</param>
        /// <param name="options">The options.</param>
        /// <returns>System.Object.</returns>
        private object ConstructType(Type requestedType, Type implementationType, ConstructorInfo constructor, ResolveOptions options)
        {
            return ConstructType(requestedType, implementationType, constructor, NamedParameterOverloads.Default, options);
        }

        /// <summary>
        /// Constructs the type.
        /// </summary>
        /// <param name="requestedType">Type of the requested.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="options">The options.</param>
        /// <returns>System.Object.</returns>
        private object ConstructType(Type requestedType, Type implementationType, NamedParameterOverloads parameters, ResolveOptions options)
        {
            return ConstructType(requestedType, implementationType, null, parameters, options);
        }

        /// <summary>
        /// Constructs the type.
        /// </summary>
        /// <param name="requestedType">Type of the requested.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="constructor">The constructor.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="options">The options.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="TinyIoC.TinyIoCResolutionException">
        /// </exception>
        private object ConstructType(Type requestedType, Type implementationType, ConstructorInfo constructor, NamedParameterOverloads parameters, ResolveOptions options)
        {
            var typeToConstruct = implementationType;

#if RESOLVE_OPEN_GENERICS
            if (implementationType.IsGenericTypeDefinition())
            {
#if NETFX_CORE
                if (requestedType == null || !requestedType.IsGenericType() || !requestedType.GenericTypeArguments.Any())
#else
                if (requestedType == null || !requestedType.IsGenericType() || !requestedType.GetGenericArguments().Any())
#endif
                    throw new TinyIoCResolutionException(typeToConstruct);

#if NETFX_CORE
                typeToConstruct = typeToConstruct.MakeGenericType(requestedType.GenericTypeArguments);
#else
                typeToConstruct = typeToConstruct.MakeGenericType(requestedType.GetGenericArguments());
#endif
            }
#endif

            if (constructor == null)
            {
                // Try and get the best constructor that we can construct
                // if we can't construct any then get the constructor
                // with the least number of parameters so we can throw a meaningful
                // resolve exception
                constructor = GetBestConstructor(typeToConstruct, parameters, options) ?? GetTypeConstructors(typeToConstruct).LastOrDefault();
            }

            if (constructor == null)
                throw new TinyIoCResolutionException(typeToConstruct);

            var ctorParams = constructor.GetParameters();
            object[] args = new object[ctorParams.Count()];

            for (int parameterIndex = 0; parameterIndex < ctorParams.Count(); parameterIndex++)
            {
                var currentParam = ctorParams[parameterIndex];

                try
                {
                    args[parameterIndex] = parameters.ContainsKey(currentParam.Name) ?
                                            parameters[currentParam.Name] :
                                            ResolveInternal(
                                                new TypeRegistration(currentParam.ParameterType),
                                                NamedParameterOverloads.Default,
                                                options);
                }
                catch (TinyIoCResolutionException ex)
                {
                    // If a constructor parameter can't be resolved
                    // it will throw, so wrap it and throw that this can't
                    // be resolved.
                    throw new TinyIoCResolutionException(typeToConstruct, ex);
                }
                catch (Exception ex)
                {
                    throw new TinyIoCResolutionException(typeToConstruct, ex);
                }
            }

            try
            {
                return constructor.Invoke(args);
            }
            catch (Exception ex)
            {
                throw new TinyIoCResolutionException(typeToConstruct, ex);
            }
        }

        /// <summary>
        /// Builds up internal.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="resolveOptions">The resolve options.</param>
        private void BuildUpInternal(object input, ResolveOptions resolveOptions)
        {
#if NETFX_CORE
            var properties = from property in input.GetType().GetTypeInfo().DeclaredProperties
                             where (property.GetMethod != null) && (property.SetMethod != null) && !property.PropertyType.GetTypeInfo().IsValueType
                             select property;
#else
            var properties = from property in input.GetType().GetProperties()
                             where (property.GetGetMethod() != null) && (property.GetSetMethod() != null) && !property.PropertyType.IsValueType()
                             select property;
#endif

            foreach (var property in properties)
            {
                if (property.GetValue(input, null) == null)
                {
                    try
                    {
                        property.SetValue(input, ResolveInternal(new TypeRegistration(property.PropertyType), NamedParameterOverloads.Default, resolveOptions), null);
                    }
                    catch (TinyIoCResolutionException)
                    {
                        // Catch any resolution errors and ignore them
                    }
                }
            }
        }

        /// <summary>
        /// Gets the type of the parent registrations for.
        /// </summary>
        /// <param name="resolveType">Type of the resolve.</param>
        /// <returns>IEnumerable&lt;TypeRegistration&gt;.</returns>
        private IEnumerable<TypeRegistration> GetParentRegistrationsForType(Type resolveType)
        {
            if (_Parent == null)
                return new TypeRegistration[] { };

            var registrations = _Parent._RegisteredTypes.Keys.Where(tr => tr.Type == resolveType);

            return registrations.Concat(_Parent.GetParentRegistrationsForType(resolveType));
        }

        /// <summary>
        /// Resolves all internal.
        /// </summary>
        /// <param name="resolveType">Type of the resolve.</param>
        /// <param name="includeUnnamed">if set to <c>true</c> [include unnamed].</param>
        /// <returns>IEnumerable&lt;System.Object&gt;.</returns>
        private IEnumerable<object> ResolveAllInternal(Type resolveType, bool includeUnnamed)
        {
            var registrations = _RegisteredTypes.Keys.Where(tr => tr.Type == resolveType).Concat(GetParentRegistrationsForType(resolveType));

            if (!includeUnnamed)
                registrations = registrations.Where(tr => tr.Name != string.Empty);

            return registrations.Select(registration => this.ResolveInternal(registration, NamedParameterOverloads.Default, ResolveOptions.Default));
        }

        /// <summary>
        /// Determines whether [is valid assignment] [the specified register type].
        /// </summary>
        /// <param name="registerType">Type of the register.</param>
        /// <param name="registerImplementation">The register implementation.</param>
        /// <returns><c>true</c> if [is valid assignment] [the specified register type]; otherwise, <c>false</c>.</returns>
        private static bool IsValidAssignment(Type registerType, Type registerImplementation)
        {
#if NETFX_CORE
            var registerTypeDef = registerType.GetTypeInfo();
            var registerImplementationDef = registerImplementation.GetTypeInfo();

            if (!registerTypeDef.IsGenericTypeDefinition)
            {
                if (!registerTypeDef.IsAssignableFrom(registerImplementationDef))
                    return false;
            }
            else
            {
                if (registerTypeDef.IsInterface)
                {
                    if (!registerImplementationDef.ImplementedInterfaces.Any(t => t.GetTypeInfo().Name == registerTypeDef.Name))
                        return false;
                }
                else if (registerTypeDef.IsAbstract && registerImplementationDef.BaseType != registerType)
                {
                    return false;
                }
            }
#else
            if (!registerType.IsGenericTypeDefinition())
            {
                if (!registerType.IsAssignableFrom(registerImplementation))
                    return false;
            }
            else
            {
                if (registerType.IsInterface())
                {
#if WINDOWS_PHONE
                    if  (registerImplementation.GetInterface(registerType.Name, true) == null)
#else
                    if (!registerImplementation.FindInterfaces((t, o) => t.Name == registerType.Name, null).Any())
#endif
                        return false;
                }
                else if (registerType.IsAbstract() && registerImplementation.BaseType() != registerType)
                {
                    return false;
                }
            }
#endif
            return true;
        }

#endregion

#region IDisposable Members
        /// <summary>
        /// The disposed
        /// </summary>
        bool disposed = false;
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;

                _RegisteredTypes.Dispose();

                GC.SuppressFinalize(this);
            }
        }

#endregion
    }
}

// reverse shim for WinRT SR changes...
#if !NETFX_CORE

namespace System.Reflection
{
    /// <summary>
    /// Class ReverseTypeExtender.
    /// </summary>
    public static class ReverseTypeExtender
    {
        /// <summary>
        /// Determines whether the specified type is class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is class; otherwise, <c>false</c>.</returns>
        public static bool IsClass(this Type type)
        {
            return type.IsClass;
        }

        /// <summary>
        /// Determines whether the specified type is abstract.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is abstract; otherwise, <c>false</c>.</returns>
        public static bool IsAbstract(this Type type)
        {
            return type.IsAbstract;
        }

        /// <summary>
        /// Determines whether the specified type is interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is interface; otherwise, <c>false</c>.</returns>
        public static bool IsInterface(this Type type)
        {
            return type.IsInterface;
        }

        /// <summary>
        /// Determines whether the specified type is primitive.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is primitive; otherwise, <c>false</c>.</returns>
        public static bool IsPrimitive(this Type type)
        {
            return type.IsPrimitive;
        }

        /// <summary>
        /// Determines whether [is value type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is value type] [the specified type]; otherwise, <c>false</c>.</returns>
        public static bool IsValueType(this Type type)
        {
            return type.IsValueType;
        }

        /// <summary>
        /// Determines whether [is generic type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is generic type] [the specified type]; otherwise, <c>false</c>.</returns>
        public static bool IsGenericType(this Type type)
        {
            return type.IsGenericType;
        }

        /// <summary>
        /// Determines whether [is generic parameter] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is generic parameter] [the specified type]; otherwise, <c>false</c>.</returns>
        public static bool IsGenericParameter(this Type type)
        {
            return type.IsGenericParameter;
        }

        /// <summary>
        /// Determines whether [is generic type definition] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is generic type definition] [the specified type]; otherwise, <c>false</c>.</returns>
        public static bool IsGenericTypeDefinition(this Type type)
        {
            return type.IsGenericTypeDefinition;
        }

        /// <summary>
        /// Bases the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Type.</returns>
        public static Type BaseType(this Type type)
        {
            return type.BaseType;
        }

        /// <summary>
        /// Assemblies the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Assembly.</returns>
        public static Assembly Assembly(this Type type)
        {
            return type.Assembly;
        }
    }
}
#endif
