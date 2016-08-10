// ***********************************************************************
// Assembly         : XLabs.Serialization.ServiceStack.WP8
// Author           : rmarinho
// Created          : 09-08-2015
//
// Last Modified By : rmarinho
// Last Modified On : 09-08-2015
// ***********************************************************************
// <copyright file="ReflectionExtensions.cs" company="XLabs">
//     Copyright © ServiceStack 2013 & XLabs
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using ServiceStack.Text.Support;
#if WINDOWS_PHONE
using System.Linq.Expressions;
#endif

namespace ServiceStack.Text
{

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	public delegate EmptyCtorDelegate EmptyCtorFactoryDelegate(Type type);
	public delegate object EmptyCtorDelegate();
#pragma warning restore CS1591

	/// <summary>
	/// ReflectionExtensions
	/// </summary>
	public static class ReflectionExtensions
	{
		/// <summary>
		/// The default value types
		/// </summary>
		private static Dictionary<Type, object> DefaultValueTypes = new Dictionary<Type, object>();

		/// <summary>
		/// Gets the default value.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Object.</returns>
		public static object GetDefaultValue(this Type type)
        {
            if (!type.IsValueType()) return null;

            object defaultValue;
            if (DefaultValueTypes.TryGetValue(type, out defaultValue)) return defaultValue;

            defaultValue = Activator.CreateInstance(type);

            Dictionary<Type, object> snapshot, newCache;
            do
            {
                snapshot = DefaultValueTypes;
                newCache = new Dictionary<Type, object>(DefaultValueTypes);
                newCache[type] = defaultValue;

            } while (!ReferenceEquals(
                Interlocked.CompareExchange(ref DefaultValueTypes, newCache, snapshot), snapshot));

            return defaultValue;
        }

		/// <summary>
		/// Determines whether [is instance of] [the specified this or base type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="thisOrBaseType">Type of the this or base.</param>
		/// <returns><c>true</c> if [is instance of] [the specified this or base type]; otherwise, <c>false</c>.</returns>
		public static bool IsInstanceOf(this Type type, Type thisOrBaseType)
        {
            while (type != null)
            {
                if (type == thisOrBaseType)
                    return true;

                type = type.BaseType();
            }
            return false;
        }

		/// <summary>
		/// Determines whether [has generic type] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if [has generic type] [the specified type]; otherwise, <c>false</c>.</returns>
		public static bool HasGenericType(this Type type)
        {
            while (type != null)
            {
                if (type.IsGeneric())
                    return true;

                type = type.BaseType();
            }
            return false;
        }

		/// <summary>
		/// Gets the type of the generic.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>Type.</returns>
		public static Type GetGenericType(this Type type)
        {
            while (type != null)
            {
                if (type.IsGeneric())
                    return type;

                type = type.BaseType();
            }
            return null;
        }

		/// <summary>
		/// Gets the type with generic type definition of any.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="genericTypeDefinitions">The generic type definitions.</param>
		/// <returns>Type.</returns>
		public static Type GetTypeWithGenericTypeDefinitionOfAny(this Type type, params Type[] genericTypeDefinitions)
        {
            foreach (var genericTypeDefinition in genericTypeDefinitions)
            {
                var genericType = type.GetTypeWithGenericTypeDefinitionOf(genericTypeDefinition);
                if (genericType == null && type == genericTypeDefinition)
                {
                    genericType = type;
                }

                if (genericType != null)
                    return genericType;
            }
            return null;
        }

		/// <summary>
		/// Determines whether [is or has generic interface type of] [the specified generic type definition].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="genericTypeDefinition">The generic type definition.</param>
		/// <returns><c>true</c> if [is or has generic interface type of] [the specified generic type definition]; otherwise, <c>false</c>.</returns>
		public static bool IsOrHasGenericInterfaceTypeOf(this Type type, Type genericTypeDefinition)
        {
            return (type.GetTypeWithGenericTypeDefinitionOf(genericTypeDefinition) != null)
                || (type == genericTypeDefinition);
        }

		/// <summary>
		/// Gets the type with generic type definition of.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="genericTypeDefinition">The generic type definition.</param>
		/// <returns>Type.</returns>
		public static Type GetTypeWithGenericTypeDefinitionOf(this Type type, Type genericTypeDefinition)
        {
            foreach (var t in type.GetTypeInterfaces())
            {
                if (t.IsGeneric() && t.GetGenericTypeDefinition() == genericTypeDefinition)
                {
                    return t;
                }
            }

            var genericType = type.GetGenericType();
            if (genericType != null && genericType.GetGenericTypeDefinition() == genericTypeDefinition)
            {
                return genericType;
            }

            return null;
        }

		/// <summary>
		/// Gets the type with interface of.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="interfaceType">Type of the interface.</param>
		/// <returns>Type.</returns>
		public static Type GetTypeWithInterfaceOf(this Type type, Type interfaceType)
        {
            if (type == interfaceType) return interfaceType;

            foreach (var t in type.GetTypeInterfaces())
            {
                if (t == interfaceType)
                    return t;
            }

            return null;
        }

		/// <summary>
		/// Determines whether the specified interface type has interface.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="interfaceType">Type of the interface.</param>
		/// <returns><c>true</c> if the specified interface type has interface; otherwise, <c>false</c>.</returns>
		public static bool HasInterface(this Type type, Type interfaceType)
        {
            foreach (var t in type.GetTypeInterfaces())
            {
                if (t == interfaceType)
                    return true;
            }
            return false;
        }

		/// <summary>
		/// Alls the type of the have interfaces of.
		/// </summary>
		/// <param name="assignableFromType">Type of the assignable from.</param>
		/// <param name="types">The types.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public static bool AllHaveInterfacesOfType(
            this Type assignableFromType, params Type[] types)
        {
            foreach (var type in types)
            {
                if (assignableFromType.GetTypeWithInterfaceOf(type) == null) return false;
            }
            return true;
        }

		/// <summary>
		/// Determines whether [is numeric type] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if [is numeric type] [the specified type]; otherwise, <c>false</c>.</returns>
		public static bool IsNumericType(this Type type)
        {
            if (type == null) return false;

            if (type.IsEnum) //TypeCode can be TypeCode.Int32
            {
                return JsConfig.TreatEnumAsInteger || type.IsEnumFlags();
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;

                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }
                    if (type.IsEnum)
                    {
                        return JsConfig.TreatEnumAsInteger || type.IsEnumFlags();
                    }
                    return false;
            }
            return false;
        }

		/// <summary>
		/// Determines whether [is integer type] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if [is integer type] [the specified type]; otherwise, <c>false</c>.</returns>
		public static bool IsIntegerType(this Type type)
        {
            if (type == null) return false;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;

                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;
        }

		/// <summary>
		/// Determines whether [is real number type] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if [is real number type] [the specified type]; otherwise, <c>false</c>.</returns>
		public static bool IsRealNumberType(this Type type)
        {
            if (type == null) return false;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;

                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;
        }

		/// <summary>
		/// Gets the type with generic interface of.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="genericInterfaceType">Type of the generic interface.</param>
		/// <returns>Type.</returns>
		public static Type GetTypeWithGenericInterfaceOf(this Type type, Type genericInterfaceType)
        {
            foreach (var t in type.GetTypeInterfaces())
            {
                if (t.IsGeneric() && t.GetGenericTypeDefinition() == genericInterfaceType) 
                    return t;
            }

            if (!type.IsGeneric()) return null;

            var genericType = type.GetGenericType();
            return genericType.GetGenericTypeDefinition() == genericInterfaceType
                    ? genericType
                    : null;
        }

		/// <summary>
		/// Determines whether [has any type definitions of] [the specified these generic types].
		/// </summary>
		/// <param name="genericType">Type of the generic.</param>
		/// <param name="theseGenericTypes">The these generic types.</param>
		/// <returns><c>true</c> if [has any type definitions of] [the specified these generic types]; otherwise, <c>false</c>.</returns>
		public static bool HasAnyTypeDefinitionsOf(this Type genericType, params Type[] theseGenericTypes)
        {
            if (!genericType.IsGeneric()) return false;

            var genericTypeDefinition = genericType.GenericTypeDefinition();

            foreach (var thisGenericType in theseGenericTypes)
            {
                if (genericTypeDefinition == thisGenericType)
                    return true;
            }

            return false;
        }

		/// <summary>
		/// Gets the generic arguments if both have same generic definition type and arguments.
		/// </summary>
		/// <param name="assignableFromType">Type of the assignable from.</param>
		/// <param name="typeA">The type a.</param>
		/// <param name="typeB">The type b.</param>
		/// <returns>Type[].</returns>
		public static Type[] GetGenericArgumentsIfBothHaveSameGenericDefinitionTypeAndArguments(
            this Type assignableFromType, Type typeA, Type typeB)
        {
            var typeAInterface = typeA.GetTypeWithGenericInterfaceOf(assignableFromType);
            if (typeAInterface == null) return null;

            var typeBInterface = typeB.GetTypeWithGenericInterfaceOf(assignableFromType);
            if (typeBInterface == null) return null;

            var typeAGenericArgs = typeAInterface.GetTypeGenericArguments();
            var typeBGenericArgs = typeBInterface.GetTypeGenericArguments();

            if (typeAGenericArgs.Length != typeBGenericArgs.Length) return null;

            for (var i = 0; i < typeBGenericArgs.Length; i++)
            {
                if (typeAGenericArgs[i] != typeBGenericArgs[i])
                {
                    return null;
                }
            }

            return typeAGenericArgs;
        }

		/// <summary>
		/// Gets the generic arguments if both have convertible generic definition type and arguments.
		/// </summary>
		/// <param name="assignableFromType">Type of the assignable from.</param>
		/// <param name="typeA">The type a.</param>
		/// <param name="typeB">The type b.</param>
		/// <returns>TypePair.</returns>
		public static TypePair GetGenericArgumentsIfBothHaveConvertibleGenericDefinitionTypeAndArguments(
            this Type assignableFromType, Type typeA, Type typeB)
        {
            var typeAInterface = typeA.GetTypeWithGenericInterfaceOf(assignableFromType);
            if (typeAInterface == null) return null;

            var typeBInterface = typeB.GetTypeWithGenericInterfaceOf(assignableFromType);
            if (typeBInterface == null) return null;

            var typeAGenericArgs = typeAInterface.GetTypeGenericArguments();
            var typeBGenericArgs = typeBInterface.GetTypeGenericArguments();

            if (typeAGenericArgs.Length != typeBGenericArgs.Length) return null;

            for (var i = 0; i < typeBGenericArgs.Length; i++)
            {
                if (!AreAllStringOrValueTypes(typeAGenericArgs[i], typeBGenericArgs[i]))
                {
                    return null;
                }
            }

            return new TypePair(typeAGenericArgs, typeBGenericArgs);
        }

		/// <summary>
		/// Ares all string or value types.
		/// </summary>
		/// <param name="types">The types.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public static bool AreAllStringOrValueTypes(params Type[] types)
        {
            foreach (var type in types)
            {
                if (!(type == typeof(string) || type.IsValueType())) return false;
            }
            return true;
        }

		/// <summary>
		/// The constructor methods
		/// </summary>
		static Dictionary<Type, EmptyCtorDelegate> ConstructorMethods = new Dictionary<Type, EmptyCtorDelegate>();
		/// <summary>
		/// Gets the constructor method.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>EmptyCtorDelegate.</returns>
		public static EmptyCtorDelegate GetConstructorMethod(Type type)
        {
            EmptyCtorDelegate emptyCtorFn;
            if (ConstructorMethods.TryGetValue(type, out emptyCtorFn)) return emptyCtorFn;

            emptyCtorFn = GetConstructorMethodToCache(type);

            Dictionary<Type, EmptyCtorDelegate> snapshot, newCache;
            do
            {
                snapshot = ConstructorMethods;
                newCache = new Dictionary<Type, EmptyCtorDelegate>(ConstructorMethods);
                newCache[type] = emptyCtorFn;

            } while (!ReferenceEquals(
                Interlocked.CompareExchange(ref ConstructorMethods, newCache, snapshot), snapshot));

            return emptyCtorFn;
        }

		/// <summary>
		/// The type names map
		/// </summary>
		static Dictionary<string, EmptyCtorDelegate> TypeNamesMap = new Dictionary<string, EmptyCtorDelegate>();
		/// <summary>
		/// Gets the constructor method.
		/// </summary>
		/// <param name="typeName">Name of the type.</param>
		/// <returns>EmptyCtorDelegate.</returns>
		public static EmptyCtorDelegate GetConstructorMethod(string typeName)
        {
            EmptyCtorDelegate emptyCtorFn;
            if (TypeNamesMap.TryGetValue(typeName, out emptyCtorFn)) return emptyCtorFn;

            var type = JsConfig.TypeFinder.Invoke(typeName);
            if (type == null) return null;
            emptyCtorFn = GetConstructorMethodToCache(type);

            Dictionary<string, EmptyCtorDelegate> snapshot, newCache;
            do
            {
                snapshot = TypeNamesMap;
                newCache = new Dictionary<string, EmptyCtorDelegate>(TypeNamesMap);
                newCache[typeName] = emptyCtorFn;

            } while (!ReferenceEquals(
                Interlocked.CompareExchange(ref TypeNamesMap, newCache, snapshot), snapshot));

            return emptyCtorFn;
        }

		/// <summary>
		/// Gets the constructor method to cache.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>EmptyCtorDelegate.</returns>
		public static EmptyCtorDelegate GetConstructorMethodToCache(Type type)
        {
            if (type.IsInterface)
            {
                if (type.HasGenericType())
                {
                    var genericType = type.GetTypeWithGenericTypeDefinitionOfAny(
                        typeof(IDictionary<,>));

                    if (genericType != null)
                    {
                        var keyType = genericType.GenericTypeArguments()[0];
                        var valueType = genericType.GenericTypeArguments()[1];
                        return GetConstructorMethodToCache(typeof(Dictionary<,>).MakeGenericType(keyType, valueType));
                    }

                    genericType = type.GetTypeWithGenericTypeDefinitionOfAny(
                        typeof(IEnumerable<>),
                        typeof(ICollection<>),
                        typeof(IList<>));

                    if (genericType != null)
                    {
                        var elementType = genericType.GenericTypeArguments()[0];
                        return GetConstructorMethodToCache(typeof(List<>).MakeGenericType(elementType));
                    }
                }
            }
            else if (type.IsArray)
            {
                return () => Array.CreateInstance(type.GetElementType(), 0);
            }

            var emptyCtor = type.GetEmptyConstructor();
            if (emptyCtor != null)
            {

#if MONOTOUCH || c|| XBOX || NETFX_CORE
				return () => Activator.CreateInstance(type);
#elif WINDOWS_PHONE
                return Expression.Lambda<EmptyCtorDelegate>(Expression.New(type)).Compile();
#else
#if SILVERLIGHT
                var dm = new System.Reflection.Emit.DynamicMethod("MyCtor", type, Type.EmptyTypes);
#else
                var dm = new System.Reflection.Emit.DynamicMethod("MyCtor", type, Type.EmptyTypes, typeof(ReflectionExtensions).Module, true);
#endif
                var ilgen = dm.GetILGenerator();
                ilgen.Emit(System.Reflection.Emit.OpCodes.Nop);
                ilgen.Emit(System.Reflection.Emit.OpCodes.Newobj, emptyCtor);
                ilgen.Emit(System.Reflection.Emit.OpCodes.Ret);

                return (EmptyCtorDelegate)dm.CreateDelegate(typeof(EmptyCtorDelegate));
#endif
            }

#if (SILVERLIGHT && !WINDOWS_PHONE) || XBOX
            return () => Activator.CreateInstance(type);
#elif WINDOWS_PHONE
            return Expression.Lambda<EmptyCtorDelegate>(Expression.New(type)).Compile();
#else
            if (type == typeof(string))
                return () => String.Empty;

            //Anonymous types don't have empty constructors
            return () => FormatterServices.GetUninitializedObject(type);
#endif
        }

		/// <summary>
		/// Class TypeMeta.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		private static class TypeMeta<T>
        {
			/// <summary>
			/// The empty ctor function
			/// </summary>
			public static readonly EmptyCtorDelegate EmptyCtorFn;
			/// <summary>
			/// Initializes static members of the <see cref="TypeMeta{T}" /> class.
			/// </summary>
			static TypeMeta()
            {
                EmptyCtorFn = GetConstructorMethodToCache(typeof(T));
            }
        }

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>System.Object.</returns>
		public static object CreateInstance<T>()
        {
            return TypeMeta<T>.EmptyCtorFn();
        }

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Object.</returns>
		public static object CreateInstance(this Type type)
        {
            var ctorFn = GetConstructorMethod(type);
            return ctorFn();
        }

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <param name="typeName">Name of the type.</param>
		/// <returns>System.Object.</returns>
		public static object CreateInstance(string typeName)
        {
            var ctorFn = GetConstructorMethod(typeName);
            return ctorFn();
        }

		/// <summary>
		/// Gets the public properties.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>PropertyInfo[].</returns>
		public static PropertyInfo[] GetPublicProperties(this Type type)
        {
            if (type.IsInterface())
            {
                var propertyInfos = new List<PropertyInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);

                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetTypeInterfaces())
                    {
                        if (considered.Contains(subInterface)) continue;

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetTypesPublicProperties();

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            return type.GetTypesPublicProperties()
                .Where(t => t.GetIndexParameters().Length == 0) // ignore indexed properties
                .ToArray();
        }

		/// <summary>
		/// The data contract
		/// </summary>
		const string DataContract = "DataContractAttribute";
		/// <summary>
		/// The data member
		/// </summary>
		const string DataMember = "DataMemberAttribute";
		/// <summary>
		/// The ignore data member
		/// </summary>
		const string IgnoreDataMember = "IgnoreDataMemberAttribute";

		/// <summary>
		/// Gets the serializable properties.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>PropertyInfo[].</returns>
		public static PropertyInfo[] GetSerializableProperties(this Type type)
        {
            var publicProperties = GetPublicProperties(type);
            var publicReadableProperties = publicProperties.Where(x => x.PropertyGetMethod() != null);

            if (type.IsDto())
            {
                return !Env.IsMono
                    ? publicReadableProperties.Where(attr => 
                        attr.IsDefined(typeof(DataMemberAttribute), false)).ToArray()
                    : publicReadableProperties.Where(attr => 
                        attr.CustomAttributes(false).Any(x => x.GetType().Name == DataMember)).ToArray();
            }

            // else return those properties that are not decorated with IgnoreDataMember
            return publicReadableProperties.Where(prop => !prop.CustomAttributes(false).Any(attr => attr.GetType().Name == IgnoreDataMember)).ToArray();
        }

		/// <summary>
		/// Gets the serializable fields.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>FieldInfo[].</returns>
		public static FieldInfo[] GetSerializableFields(this Type type)
        {
            if (type.IsDto()) {
                return new FieldInfo[0];
            }
            
            var publicFields = type.GetPublicFields();

            // else return those properties that are not decorated with IgnoreDataMember
            return publicFields.Where(prop => !prop.CustomAttributes(false).Any(attr => attr.GetType().Name == IgnoreDataMember)).ToArray();
        }

		/// <summary>
		/// Determines whether the specified type has attribute.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if the specified type has attribute; otherwise, <c>false</c>.</returns>
		public static bool HasAttr<T>(this Type type) where T : Attribute
        {
            return type.HasAttribute<T>();
        }

#if !SILVERLIGHT && !MONOTOUCH
		/// <summary>
		/// The type accessor map
		/// </summary>
		static readonly Dictionary<Type, FastMember.TypeAccessor> typeAccessorMap 
            = new Dictionary<Type, FastMember.TypeAccessor>();
#endif

		/// <summary>
		/// Gets the data contract.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>DataContractAttribute.</returns>
		public static DataContractAttribute GetDataContract(this Type type)
        {
            var dataContract = type.FirstAttribute<DataContractAttribute>();

#if !SILVERLIGHT && !MONOTOUCH && !XBOX
            if (dataContract == null && Env.IsMono)
                return type.GetWeakDataContract();
#endif
            return dataContract;
        }

		/// <summary>
		/// Gets the data member.
		/// </summary>
		/// <param name="pi">The pi.</param>
		/// <returns>DataMemberAttribute.</returns>
		public static DataMemberAttribute GetDataMember(this PropertyInfo pi)
        {
            var dataMember = pi.CustomAttributes(typeof(DataMemberAttribute), false)
                .FirstOrDefault() as DataMemberAttribute;

#if !SILVERLIGHT && !MONOTOUCH && !XBOX
            if (dataMember == null && Env.IsMono)
                return pi.GetWeakDataMember();
#endif
            return dataMember;
        }

		/// <summary>
		/// Gets the data member.
		/// </summary>
		/// <param name="pi">The pi.</param>
		/// <returns>DataMemberAttribute.</returns>
		public static DataMemberAttribute GetDataMember(this FieldInfo pi)
        {
            var dataMember = pi.CustomAttributes(typeof(DataMemberAttribute), false)
                .FirstOrDefault() as DataMemberAttribute;

#if !SILVERLIGHT && !MONOTOUCH && !XBOX
            if (dataMember == null && Env.IsMono)
                return pi.GetWeakDataMember();
#endif
            return dataMember;
        }

#if !SILVERLIGHT && !MONOTOUCH && !XBOX
		/// <summary>
		/// Gets the weak data contract.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>DataContractAttribute.</returns>
		public static DataContractAttribute GetWeakDataContract(this Type type)
        {
            var attr = type.CustomAttributes().FirstOrDefault(x => x.GetType().Name == DataContract);
            if (attr != null)
            {
                var attrType = attr.GetType();

                FastMember.TypeAccessor accessor;
                lock (typeAccessorMap)
                {
                    if (!typeAccessorMap.TryGetValue(attrType, out accessor))
                        typeAccessorMap[attrType] = accessor = FastMember.TypeAccessor.Create(attr.GetType());
                }

                return new DataContractAttribute {
                    Name = (string)accessor[attr, "Name"],
                    Namespace = (string)accessor[attr, "Namespace"],
                };
            }
            return null;
        }

		/// <summary>
		/// Gets the weak data member.
		/// </summary>
		/// <param name="pi">The pi.</param>
		/// <returns>DataMemberAttribute.</returns>
		public static DataMemberAttribute GetWeakDataMember(this PropertyInfo pi)
        {
            var attr = pi.CustomAttributes().FirstOrDefault(x => x.GetType().Name == DataMember);
            if (attr != null)
            {
                var attrType = attr.GetType();

                FastMember.TypeAccessor accessor;
                lock (typeAccessorMap)
                {
                    if (!typeAccessorMap.TryGetValue(attrType, out accessor))
                        typeAccessorMap[attrType] = accessor = FastMember.TypeAccessor.Create(attr.GetType());
                }

                var newAttr = new DataMemberAttribute {
                    Name = (string) accessor[attr, "Name"],
                    EmitDefaultValue = (bool)accessor[attr, "EmitDefaultValue"],
                    IsRequired = (bool)accessor[attr, "IsRequired"],
                };

                var order = (int)accessor[attr, "Order"];
                if (order >= 0)
                    newAttr.Order = order; //Throws Exception if set to -1

                return newAttr;
            }
            return null;
        }

		/// <summary>
		/// Gets the weak data member.
		/// </summary>
		/// <param name="pi">The pi.</param>
		/// <returns>DataMemberAttribute.</returns>
		public static DataMemberAttribute GetWeakDataMember(this FieldInfo pi)
        {
            var attr = pi.CustomAttributes().FirstOrDefault(x => x.GetType().Name == DataMember);
            if (attr != null)
            {
                var attrType = attr.GetType();

                FastMember.TypeAccessor accessor;
                lock (typeAccessorMap)
                {
                    if (!typeAccessorMap.TryGetValue(attrType, out accessor))
                        typeAccessorMap[attrType] = accessor = FastMember.TypeAccessor.Create(attr.GetType());
                }

                var newAttr = new DataMemberAttribute
                {
                    Name = (string)accessor[attr, "Name"],
                    EmitDefaultValue = (bool)accessor[attr, "EmitDefaultValue"],
                    IsRequired = (bool)accessor[attr, "IsRequired"],
                };

                var order = (int)accessor[attr, "Order"];
                if (order >= 0)
                    newAttr.Order = order; //Throws Exception if set to -1

                return newAttr;
            }
            return null;
        }
#endif
    }

	/// <summary>
	/// Class PlatformExtensions.
	/// </summary>
	public static class PlatformExtensions //Because WinRT is a POS
    {
		/// <summary>
		/// Determines whether the specified type is interface.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if the specified type is interface; otherwise, <c>false</c>.</returns>
		public static bool IsInterface(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().IsInterface;
#else
            return type.IsInterface;
#endif
        }

		/// <summary>
		/// Determines whether the specified type is array.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if the specified type is array; otherwise, <c>false</c>.</returns>
		public static bool IsArray(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().IsArray;
#else
            return type.IsArray;
#endif
        }

		/// <summary>
		/// Determines whether [is value type] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if [is value type] [the specified type]; otherwise, <c>false</c>.</returns>
		public static bool IsValueType(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().IsValueType;
#else
            return type.IsValueType;
#endif
        }

		/// <summary>
		/// Determines whether the specified type is generic.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if the specified type is generic; otherwise, <c>false</c>.</returns>
		public static bool IsGeneric(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().IsGenericType;
#else
            return type.IsGenericType;
#endif
        }

		/// <summary>
		/// Bases the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>Type.</returns>
		public static Type BaseType(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().BaseType;
#else
            return type.BaseType;
#endif
        }

		/// <summary>
		/// Reflecteds the type.
		/// </summary>
		/// <param name="pi">The pi.</param>
		/// <returns>Type.</returns>
		public static Type ReflectedType(this PropertyInfo pi)
        {
#if NETFX_CORE
            return pi.PropertyType;
#else
            return pi.ReflectedType;
#endif
        }

		/// <summary>
		/// Reflecteds the type.
		/// </summary>
		/// <param name="fi">The fi.</param>
		/// <returns>Type.</returns>
		public static Type ReflectedType(this FieldInfo fi)
        {
#if NETFX_CORE
            return fi.FieldType;
#else
            return fi.ReflectedType;
#endif
        }

		/// <summary>
		/// Generics the type definition.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>Type.</returns>
		public static Type GenericTypeDefinition(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().GetGenericTypeDefinition();
#else
            return type.GetGenericTypeDefinition();
#endif
        }

		/// <summary>
		/// Gets the type interfaces.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>Type[].</returns>
		public static Type[] GetTypeInterfaces(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().ImplementedInterfaces.ToArray();
#else
            return type.GetInterfaces();
#endif
        }

		/// <summary>
		/// Gets the type generic arguments.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>Type[].</returns>
		public static Type[] GetTypeGenericArguments(this Type type)
        {
#if NETFX_CORE
            return type.GenericTypeArguments;
#else
            return type.GetGenericArguments();
#endif
        }

		/// <summary>
		/// Gets the empty constructor.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>ConstructorInfo.</returns>
		public static ConstructorInfo GetEmptyConstructor(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c => c.GetParameters().Count() == 0);
#else
            return type.GetConstructor(Type.EmptyTypes);
#endif
        }

		/// <summary>
		/// Gets the types public properties.
		/// </summary>
		/// <param name="subType">Type of the sub.</param>
		/// <returns>PropertyInfo[].</returns>
		internal static PropertyInfo[] GetTypesPublicProperties(this Type subType)
        {
#if NETFX_CORE 
            return subType.GetRuntimeProperties().ToArray();
#else
            return subType.GetProperties(
                BindingFlags.FlattenHierarchy |
                BindingFlags.Public |
                BindingFlags.Instance);
#endif
        }

		/// <summary>
		/// Propertieses the specified type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>PropertyInfo[].</returns>
		public static PropertyInfo[] Properties(this Type type)
        {
#if NETFX_CORE 
            return type.GetRuntimeProperties().ToArray();
#else
            return type.GetProperties();
#endif
        }

		/// <summary>
		/// Gets the public fields.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>FieldInfo[].</returns>
		public static FieldInfo[] GetPublicFields(this Type type)
        {
            if (type.IsInterface())
            {
                return new FieldInfo[0];
            }

#if NETFX_CORE
            return type.GetRuntimeFields().Where(p => p.IsPublic && !p.IsStatic).ToArray();
#else
            return type.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance)
            .ToArray();
#endif
        }

		/// <summary>
		/// Gets the public members.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>MemberInfo[].</returns>
		public static MemberInfo[] GetPublicMembers(this Type type)
        {

#if NETFX_CORE
            var members = new List<MemberInfo>();
            members.AddRange(type.GetRuntimeFields().Where(p => p.IsPublic && !p.IsStatic));
            members.AddRange(type.GetPublicProperties());
            return members.ToArray();
#else
            return type.GetMembers(BindingFlags.Public | BindingFlags.Instance);
#endif
        }

		/// <summary>
		/// Gets all public members.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>MemberInfo[].</returns>
		public static MemberInfo[] GetAllPublicMembers(this Type type)
        {

#if NETFX_CORE
            var members = new List<MemberInfo>();
            members.AddRange(type.GetRuntimeFields().Where(p => p.IsPublic && !p.IsStatic));
            members.AddRange(type.GetPublicProperties());
            return members.ToArray();
#else
            return type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
#endif
        }

		/// <summary>
		/// Determines whether the specified inherit has attribute.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type">The type.</param>
		/// <param name="inherit">if set to <c>true</c> [inherit].</param>
		/// <returns><c>true</c> if the specified inherit has attribute; otherwise, <c>false</c>.</returns>
		public static bool HasAttribute<T>(this Type type, bool inherit = true) where T : Attribute
        {
            return type.CustomAttributes(inherit).Any(x => x.GetType() == typeof(T));
        }

		/// <summary>
		/// Attributeses the type of the of.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type">The type.</param>
		/// <param name="inherit">The inherit.</param>
		/// <returns>System.Collections.Generic.IEnumerable&lt;T&gt;.</returns>
		public static IEnumerable<T> AttributesOfType<T>(this Type type, bool inherit = true) where T : Attribute
		{
#if NETFX_CORE
            return type.GetTypeInfo().GetCustomAttributes<T>(inherit);
#else
			return type.GetCustomAttributes(inherit).OfType<T>();
#endif
		}

		const string DataContract = "DataContractAttribute";
		/// <summary>
		/// Determines whether the specified type is dto.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Boolean.</returns>
		public static bool IsDto(this Type type)
		{
#if NETFX_CORE
            return type.GetTypeInfo().IsDefined(typeof(DataContractAttribute), false);
#else
			return !Env.IsMono
				   ? type.IsDefined(typeof(DataContractAttribute), false)
				   : type.GetCustomAttributes(true).Any(x => x.GetType().Name == DataContract);
#endif
		}

		/// <summary>
		/// Properties the get method.
		/// </summary>
		/// <param name="pi">The pi.</param>
		/// <param name="nonPublic">The non public.</param>
		/// <returns>System.Reflection.MethodInfo.</returns>
		public static MethodInfo PropertyGetMethod(this PropertyInfo pi, bool nonPublic = false)
		{
#if NETFX_CORE
            return pi.GetMethod;
#else
			return pi.GetGetMethod(false);
#endif
		}

		/// <summary>
		/// Interfaceses the specified type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Type[].</returns>
		public static Type[] Interfaces(this Type type)
		{
#if NETFX_CORE
            return type.GetTypeInfo().ImplementedInterfaces.ToArray();
            //return type.GetTypeInfo().ImplementedInterfaces
            //    .FirstOrDefault(x => !x.GetTypeInfo().ImplementedInterfaces
            //        .Any(y => y.GetTypeInfo().ImplementedInterfaces.Contains(y)));
#else
			return type.GetInterfaces();
#endif
		}

		/// <summary>
		/// Alls the properties.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Reflection.PropertyInfo[].</returns>
		public static PropertyInfo[] AllProperties(this Type type)
		{
#if NETFX_CORE
            return type.GetRuntimeProperties().ToArray();
#else
			return type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
#endif
		}

		/// <summary>
		/// Customs the attributes.
		/// </summary>
		/// <param name="propertyInfo">The property information.</param>
		/// <param name="inherit">The inherit.</param>
		/// <returns>System.Object[].</returns>
		public static object[] CustomAttributes(this PropertyInfo propertyInfo, bool inherit = true)
		{
#if NETFX_CORE
            return propertyInfo.GetCustomAttributes(inherit).ToArray();
#else
			return propertyInfo.GetCustomAttributes(inherit);
#endif
		}

		/// <summary>
		/// Customs the attributes.
		/// </summary>
		/// <param name="propertyInfo">The property information.</param>
		/// <param name="attrType">Type of the attribute.</param>
		/// <param name="inherit">The inherit.</param>
		/// <returns>System.Object[].</returns>
		public static object[] CustomAttributes(this PropertyInfo propertyInfo, Type attrType, bool inherit = true)
		{
#if NETFX_CORE
            return propertyInfo.GetCustomAttributes(inherit).Where(x => x.GetType() == attrType).ToArray();
#else
			return propertyInfo.GetCustomAttributes(attrType, inherit);
#endif
		}

		/// <summary>
		/// Customs the attributes.
		/// </summary>
		/// <param name="fieldInfo">The field information.</param>
		/// <param name="inherit">The inherit.</param>
		/// <returns>System.Object[].</returns>
		public static object[] CustomAttributes(this FieldInfo fieldInfo, bool inherit = true)
		{
#if NETFX_CORE
            return fieldInfo.GetCustomAttributes(inherit).ToArray();
#else
			return fieldInfo.GetCustomAttributes(inherit);
#endif
		}

		/// <summary>
		/// Customs the attributes.
		/// </summary>
		/// <param name="fieldInfo">The field information.</param>
		/// <param name="attrType">Type of the attribute.</param>
		/// <param name="inherit">The inherit.</param>
		/// <returns>System.Object[].</returns>
		public static object[] CustomAttributes(this FieldInfo fieldInfo, Type attrType, bool inherit = true)
		{
#if NETFX_CORE
            return fieldInfo.GetCustomAttributes(inherit).Where(x => x.GetType() == attrType).ToArray();
#else
			return fieldInfo.GetCustomAttributes(attrType, inherit);
#endif
		}

		/// <summary>
		/// Customs the attributes.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="inherit">The inherit.</param>
		/// <returns>System.Object[].</returns>
		public static object[] CustomAttributes(this Type type, bool inherit = true)
		{
#if NETFX_CORE
            return type.GetTypeInfo().GetCustomAttributes(inherit).ToArray();
#else
			return type.GetCustomAttributes(inherit);
#endif
		}

		/// <summary>
		/// Customs the attributes.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="attrType">Type of the attribute.</param>
		/// <param name="inherit">The inherit.</param>
		/// <returns>System.Object[].</returns>
		public static object[] CustomAttributes(this Type type, Type attrType, bool inherit = true)
		{
#if NETFX_CORE
            return type.GetTypeInfo().GetCustomAttributes(inherit).Where(x => x.GetType() == attrType).ToArray();
#else
			return type.GetCustomAttributes(attrType, inherit);
#endif
		}

		/// <summary>
		/// Firsts the attribute.
		/// </summary>
		/// <typeparam name="TAttr">The type of the t attribute.</typeparam>
		/// <param name="type">The type.</param>
		/// <param name="inherit">The inherit.</param>
		/// <returns>TAttr.</returns>
		public static TAttr FirstAttribute<TAttr>(this Type type, bool inherit = true) where TAttr : Attribute
		{
#if NETFX_CORE
            return type.GetTypeInfo().GetCustomAttributes(typeof(TAttr), inherit)
                .FirstOrDefault() as TAttr;
#else
			return type.GetCustomAttributes(typeof(TAttr), inherit)
				   .FirstOrDefault() as TAttr;
#endif
		}

		/// <summary>
		/// Firsts the attribute.
		/// </summary>
		/// <typeparam name="TAttribute">The type of the t attribute.</typeparam>
		/// <param name="propertyInfo">The property information.</param>
		/// <returns>TAttribute.</returns>
		public static TAttribute FirstAttribute<TAttribute>(this PropertyInfo propertyInfo)
					where TAttribute : Attribute
		{
			return propertyInfo.FirstAttribute<TAttribute>(true);
		}

		/// <summary>
		/// Firsts the attribute.
		/// </summary>
		/// <typeparam name="TAttribute">The type of the t attribute.</typeparam>
		/// <param name="propertyInfo">The property information.</param>
		/// <param name="inherit">The inherit.</param>
		/// <returns>TAttribute.</returns>
		public static TAttribute FirstAttribute<TAttribute>(this PropertyInfo propertyInfo, bool inherit)
					where TAttribute : Attribute
		{
#if NETFX_CORE
            var attrs = propertyInfo.GetCustomAttributes<TAttribute>(inherit);
            return (TAttribute)(attrs.Count() > 0 ? attrs.ElementAt(0) : null);
#else
			var attrs = propertyInfo.GetCustomAttributes(typeof(TAttribute), inherit);
			return (TAttribute) (attrs.Length > 0 ? attrs[0] : null);
#endif
		}

		/// <summary>
		/// Firsts the generic type definition.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Type.</returns>
		public static Type FirstGenericTypeDefinition(this Type type)
		{
			while(type != null) {
				if(type.HasGenericType())
					return type.GenericTypeDefinition();

				type = type.BaseType();
			}

			return null;
		}

		/// <summary>
		/// Determines whether the specified assembly is dynamic.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <returns>System.Boolean.</returns>
		public static bool IsDynamic(this Assembly assembly)
		{
#if MONOTOUCH || WINDOWS_PHONE || NETFX_CORE
			return false;
#else
            try
            {
                var isDyanmic = assembly is System.Reflection.Emit.AssemblyBuilder
                    || string.IsNullOrEmpty(assembly.Location);
                return isDyanmic;
            }
            catch (NotSupportedException)
            {
                //Ignore assembly.Location not supported in a dynamic assembly.
                return true;
            }
#endif
		}

		/// <summary>
		/// Gets the public static method.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="types">The types.</param>
		/// <returns>System.Reflection.MethodInfo.</returns>
		public static MethodInfo GetPublicStaticMethod(this Type type, string methodName, Type[] types = null)
		{
#if NETFX_CORE
            return type.GetRuntimeMethod(methodName, types);
#else
			return types == null
				? type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static)
				: type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static, null, types, null);
#endif
		}

		/// <summary>
		/// Gets the method information.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="types">The types.</param>
		/// <returns>System.Reflection.MethodInfo.</returns>
		public static MethodInfo GetMethodInfo(this Type type, string methodName, Type[] types = null)
		{
#if NETFX_CORE
            return type.GetRuntimeMethods().First(p => p.Name.Equals(methodName));
#else
			return types == null
				? type.GetMethod(methodName)
				: type.GetMethod(methodName, types);
#endif
		}

		/// <summary>
		/// Invokes the method.
		/// </summary>
		/// <param name="fn">The function.</param>
		/// <param name="instance">The instance.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>System.Object.</returns>
		public static object InvokeMethod(this Delegate fn, object instance, object[] parameters = null)
		{
#if NETFX_CORE
            return fn.GetMethodInfo().Invoke(instance, parameters ?? new object[] { });
#else
			return fn.Method.Invoke(instance, parameters ?? new object[] { });
#endif
		}

		/// <summary>
		/// Gets the public static field.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>System.Reflection.FieldInfo.</returns>
		public static FieldInfo GetPublicStaticField(this Type type, string fieldName)
		{
#if NETFX_CORE
            return type.GetRuntimeField(fieldName);
#else
			return type.GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
#endif
		}

		/// <summary>
		/// Makes the delegate.
		/// </summary>
		/// <param name="mi">The mi.</param>
		/// <param name="delegateType">Type of the delegate.</param>
		/// <param name="throwOnBindFailure">The throw on bind failure.</param>
		/// <returns>System.Delegate.</returns>
		public static Delegate MakeDelegate(this MethodInfo mi, Type delegateType, bool throwOnBindFailure = true)
		{
#if NETFX_CORE
            return mi.CreateDelegate(delegateType);
#else
			return Delegate.CreateDelegate(delegateType, mi, throwOnBindFailure);
#endif
		}

		/// <summary>
		/// Generics the type arguments.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Type[].</returns>
		public static Type[] GenericTypeArguments(this Type type)
		{
#if NETFX_CORE
            return type.GenericTypeArguments;
#else
			return type.GetGenericArguments();
#endif
		}

		/// <summary>
		/// Declareds the constructors.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Reflection.ConstructorInfo[].</returns>
		public static ConstructorInfo[] DeclaredConstructors(this Type type)
		{
#if NETFX_CORE
            return type.GetTypeInfo().DeclaredConstructors.ToArray();
#else
			return type.GetConstructors();
#endif
		}

		/// <summary>
		/// Assignables from.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="fromType">From type.</param>
		/// <returns>System.Boolean.</returns>
		public static bool AssignableFrom(this Type type, Type fromType)
		{
#if NETFX_CORE
            return type.GetTypeInfo().IsAssignableFrom(fromType.GetTypeInfo());
#else
			return type.IsAssignableFrom(fromType);
#endif
		}

		/// <summary>
		/// Determines whether [is standard class] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Boolean.</returns>
		public static bool IsStandardClass(this Type type)
		{
#if NETFX_CORE
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsInterface;
#else
			return type.IsClass && !type.IsAbstract && !type.IsInterface;
#endif
		}

		/// <summary>
		/// Determines whether the specified type is abstract.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Boolean.</returns>
		public static bool IsAbstract(this Type type)
		{
#if NETFX_CORE
            return type.GetTypeInfo().IsAbstract;
#else
			return type.IsAbstract;
#endif
		}

		/// <summary>
		/// Gets the property information.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>System.Reflection.PropertyInfo.</returns>
		public static PropertyInfo GetPropertyInfo(this Type type, string propertyName)
		{
#if NETFX_CORE
            return type.GetRuntimeProperty(propertyName);
#else
			return type.GetProperty(propertyName);
#endif
		}

		/// <summary>
		/// Gets the field information.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>System.Reflection.FieldInfo.</returns>
		public static FieldInfo GetFieldInfo(this Type type, string fieldName)
		{
#if NETFX_CORE
            return type.GetRuntimeField(fieldName);
#else
			return type.GetField(fieldName);
#endif
		}

		/// <summary>
		/// Gets the writable fields.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Reflection.FieldInfo[].</returns>
		public static FieldInfo[] GetWritableFields(this Type type)
		{
#if NETFX_CORE
            return type.GetRuntimeFields().Where(p => !p.IsPublic && !p.IsStatic).ToArray();
#else
			return type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField);
#endif
		}

		/// <summary>
		/// Sets the method.
		/// </summary>
		/// <param name="pi">The pi.</param>
		/// <param name="nonPublic">The non public.</param>
		/// <returns>System.Reflection.MethodInfo.</returns>
		public static MethodInfo SetMethod(this PropertyInfo pi, bool nonPublic = true)
		{
#if NETFX_CORE
            return pi.SetMethod;
#else
			return pi.GetSetMethod(nonPublic);
#endif
		}

		/// <summary>
		/// Gets the method information.
		/// </summary>
		/// <param name="pi">The pi.</param>
		/// <param name="nonPublic">The non public.</param>
		/// <returns>System.Reflection.MethodInfo.</returns>
		public static MethodInfo GetMethodInfo(this PropertyInfo pi, bool nonPublic = true)
		{
#if NETFX_CORE
            return pi.GetMethod;
#else
			return pi.GetGetMethod(nonPublic);
#endif
		}

		/// <summary>
		/// Instances the type of the of.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="instance">The instance.</param>
		/// <returns>System.Boolean.</returns>
		public static bool InstanceOfType(this Type type, object instance)
		{
#if NETFX_CORE
            return type.IsInstanceOf(instance.GetType());
#else
			return type.IsInstanceOfType(instance);
#endif
		}

		/// <summary>
		/// Determines whether the specified type is class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Boolean.</returns>
		public static bool IsClass(this Type type)
		{
#if NETFX_CORE
            return type.GetTypeInfo().IsClass;
#else
			return type.IsClass;
#endif
		}

		/// <summary>
		/// Determines whether the specified type is enum.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Boolean.</returns>
		public static bool IsEnum(this Type type)
		{
#if NETFX_CORE
            return type.GetTypeInfo().IsEnum;
#else
			return type.IsEnum;
#endif
		}

		/// <summary>
		/// Determines whether [is enum flags] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Boolean.</returns>
		public static bool IsEnumFlags(this Type type)
		{
#if NETFX_CORE
            return type.GetTypeInfo().IsEnum && type.FirstAttribute<FlagsAttribute>(false) != null;
#else
			return type.IsEnum && type.FirstAttribute<FlagsAttribute>(false) != null;
#endif
		}

		/// <summary>
		/// Determines whether [is underlying enum] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Boolean.</returns>
		public static bool IsUnderlyingEnum(this Type type)
		{
#if NETFX_CORE
            return type.GetTypeInfo().IsEnum;
#else
			return type.IsEnum || type.UnderlyingSystemType.IsEnum;
#endif
		}

		/// <summary>
		/// Gets the method infos.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Reflection.MethodInfo[].</returns>
		public static MethodInfo[] GetMethodInfos(this Type type)
		{
#if NETFX_CORE
            return type.GetRuntimeMethods().ToArray();
#else
			return type.GetMethods();
#endif
		}

		/// <summary>
		/// Gets the property infos.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Reflection.PropertyInfo[].</returns>
		public static PropertyInfo[] GetPropertyInfos(this Type type)
		{
#if NETFX_CORE
            return type.GetRuntimeProperties().ToArray();
#else
			return type.GetProperties();
#endif
		}

#if SILVERLIGHT || NETFX_CORE
		/// <summary>
		/// Converts all.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="converter">The converter.</param>
		/// <returns>System.Collections.Generic.List&lt;U&gt;.</returns>
		public static List<U> ConvertAll<T, U>(this List<T> list, Func<T, U> converter)
		{
			var result = new List<U>();
			foreach(var element in list) {
				result.Add(converter(element));
			}
			return result;
		}
#endif


	}

}