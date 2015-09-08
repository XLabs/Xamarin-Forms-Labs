//
// https://github.com/ServiceStack/ServiceStack.Text
// ServiceStack.Text: .NET C# POCO JSON, JSV and CSV Text Serializers.
//
// Authors:
//   Demis Bellot (demis.bellot@gmail.com)
//
// Copyright 2012 ServiceStack Ltd.
//
// Licensed under the same terms of ServiceStack: new BSD license.
//

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
    public delegate EmptyCtorDelegate EmptyCtorFactoryDelegate(Type type);
    public delegate object EmptyCtorDelegate();

    public static class ReflectionExtensions
    {
        private static Dictionary<Type, object> DefaultValueTypes = new Dictionary<Type, object>();

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

        public static bool IsOrHasGenericInterfaceTypeOf(this Type type, Type genericTypeDefinition)
        {
            return (type.GetTypeWithGenericTypeDefinitionOf(genericTypeDefinition) != null)
                || (type == genericTypeDefinition);
        }

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

        public static bool HasInterface(this Type type, Type interfaceType)
        {
            foreach (var t in type.GetTypeInterfaces())
            {
                if (t == interfaceType)
                    return true;
            }
            return false;
        }

        public static bool AllHaveInterfacesOfType(
            this Type assignableFromType, params Type[] types)
        {
            foreach (var type in types)
            {
                if (assignableFromType.GetTypeWithInterfaceOf(type) == null) return false;
            }
            return true;
        }

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

        public static bool AreAllStringOrValueTypes(params Type[] types)
        {
            foreach (var type in types)
            {
                if (!(type == typeof(string) || type.IsValueType())) return false;
            }
            return true;
        }

        static Dictionary<Type, EmptyCtorDelegate> ConstructorMethods = new Dictionary<Type, EmptyCtorDelegate>();
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

        static Dictionary<string, EmptyCtorDelegate> TypeNamesMap = new Dictionary<string, EmptyCtorDelegate>();
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

        private static class TypeMeta<T>
        {
            public static readonly EmptyCtorDelegate EmptyCtorFn;
            static TypeMeta()
            {
                EmptyCtorFn = GetConstructorMethodToCache(typeof(T));
            }
        }

        public static object CreateInstance<T>()
        {
            return TypeMeta<T>.EmptyCtorFn();
        }

        public static object CreateInstance(this Type type)
        {
            var ctorFn = GetConstructorMethod(type);
            return ctorFn();
        }

        public static object CreateInstance(string typeName)
        {
            var ctorFn = GetConstructorMethod(typeName);
            return ctorFn();
        }

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

        const string DataContract = "DataContractAttribute";
        const string DataMember = "DataMemberAttribute";
        const string IgnoreDataMember = "IgnoreDataMemberAttribute";

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

        public static FieldInfo[] GetSerializableFields(this Type type)
        {
            if (type.IsDto()) {
                return new FieldInfo[0];
            }
            
            var publicFields = type.GetPublicFields();

            // else return those properties that are not decorated with IgnoreDataMember
            return publicFields.Where(prop => !prop.CustomAttributes(false).Any(attr => attr.GetType().Name == IgnoreDataMember)).ToArray();
        }

        public static bool HasAttr<T>(this Type type) where T : Attribute
        {
            return type.HasAttribute<T>();
        }

#if !SILVERLIGHT && !MONOTOUCH 
        static readonly Dictionary<Type, FastMember.TypeAccessor> typeAccessorMap 
            = new Dictionary<Type, FastMember.TypeAccessor>();
#endif

        public static DataContractAttribute GetDataContract(this Type type)
        {
            var dataContract = type.FirstAttribute<DataContractAttribute>();

#if !SILVERLIGHT && !MONOTOUCH && !XBOX
            if (dataContract == null && Env.IsMono)
                return type.GetWeakDataContract();
#endif
            return dataContract;
        }

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

    public static class PlatformExtensions //Because WinRT is a POS
    {
        public static bool IsInterface(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().IsInterface;
#else
            return type.IsInterface;
#endif
        }

        public static bool IsArray(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().IsArray;
#else
            return type.IsArray;
#endif
        }

        public static bool IsValueType(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().IsValueType;
#else
            return type.IsValueType;
#endif
        }

        public static bool IsGeneric(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().IsGenericType;
#else
            return type.IsGenericType;
#endif
        }

        public static Type BaseType(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().BaseType;
#else
            return type.BaseType;
#endif
        }

        public static Type ReflectedType(this PropertyInfo pi)
        {
#if NETFX_CORE
            return pi.PropertyType;
#else
            return pi.ReflectedType;
#endif
        }

        public static Type ReflectedType(this FieldInfo fi)
        {
#if NETFX_CORE
            return fi.FieldType;
#else
            return fi.ReflectedType;
#endif
        }

        public static Type GenericTypeDefinition(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().GetGenericTypeDefinition();
#else
            return type.GetGenericTypeDefinition();
#endif
        }

        public static Type[] GetTypeInterfaces(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().ImplementedInterfaces.ToArray();
#else
            return type.GetInterfaces();
#endif
        }

        public static Type[] GetTypeGenericArguments(this Type type)
        {
#if NETFX_CORE
            return type.GenericTypeArguments;
#else
            return type.GetGenericArguments();
#endif
        }

        public static ConstructorInfo GetEmptyConstructor(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c => c.GetParameters().Count() == 0);
#else
            return type.GetConstructor(Type.EmptyTypes);
#endif
        }

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

        public static PropertyInfo[] Properties(this Type type)
        {
#if NETFX_CORE 
            return type.GetRuntimeProperties().ToArray();
#else
            return type.GetProperties();
#endif
        }

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

        public static bool HasAttribute<T>(this Type type, bool inherit = true) where T : Attribute
        {
            return type.CustomAttributes(inherit).Any(x => x.GetType() == typeof(T));
        }

        public static IEnumerable<T> AttributesOfType<T>(this Type type, bool inherit = true) where T : Attribute
        {
#if NETFX_CORE
            return type.GetTypeInfo().GetCustomAttributes<T>(inherit);
#else
            return type.GetCustomAttributes(inherit).OfType<T>();
#endif
        }

        const string DataContract = "DataContractAttribute";
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

        public static MethodInfo PropertyGetMethod(this PropertyInfo pi, bool nonPublic = false)
        {
#if NETFX_CORE
            return pi.GetMethod;
#else
            return pi.GetGetMethod(false);
#endif
        }

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

        public static PropertyInfo[] AllProperties(this Type type)
        {
#if NETFX_CORE
            return type.GetRuntimeProperties().ToArray();
#else
            return type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
#endif
        }

        public static object[] CustomAttributes(this PropertyInfo propertyInfo, bool inherit = true)
        {
#if NETFX_CORE
            return propertyInfo.GetCustomAttributes(inherit).ToArray();
#else
            return propertyInfo.GetCustomAttributes(inherit);
#endif
        }

        public static object[] CustomAttributes(this PropertyInfo propertyInfo, Type attrType, bool inherit = true)
        {
#if NETFX_CORE
            return propertyInfo.GetCustomAttributes(inherit).Where(x => x.GetType() == attrType).ToArray();
#else
            return propertyInfo.GetCustomAttributes(attrType, inherit);
#endif
        }

        public static object[] CustomAttributes(this FieldInfo fieldInfo, bool inherit = true)
        {
#if NETFX_CORE
            return fieldInfo.GetCustomAttributes(inherit).ToArray();
#else
            return fieldInfo.GetCustomAttributes(inherit);
#endif
        }

        public static object[] CustomAttributes(this FieldInfo fieldInfo, Type attrType, bool inherit = true)
        {
#if NETFX_CORE
            return fieldInfo.GetCustomAttributes(inherit).Where(x => x.GetType() == attrType).ToArray();
#else
            return fieldInfo.GetCustomAttributes(attrType, inherit);
#endif
        }

        public static object[] CustomAttributes(this Type type, bool inherit = true)
        {
#if NETFX_CORE
            return type.GetTypeInfo().GetCustomAttributes(inherit).ToArray();
#else
            return type.GetCustomAttributes(inherit);
#endif
        }

        public static object[] CustomAttributes(this Type type, Type attrType, bool inherit = true)
        {
#if NETFX_CORE
            return type.GetTypeInfo().GetCustomAttributes(inherit).Where(x => x.GetType() == attrType).ToArray();
#else
            return type.GetCustomAttributes(attrType, inherit);
#endif
        }

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
        
        public static TAttribute FirstAttribute<TAttribute>(this PropertyInfo propertyInfo)
            where TAttribute : Attribute
        {
            return propertyInfo.FirstAttribute<TAttribute>(true);
        }

        public static TAttribute FirstAttribute<TAttribute>(this PropertyInfo propertyInfo, bool inherit)
            where TAttribute : Attribute
        {
#if NETFX_CORE
            var attrs = propertyInfo.GetCustomAttributes<TAttribute>(inherit);
            return (TAttribute)(attrs.Count() > 0 ? attrs.ElementAt(0) : null);
#else
            var attrs = propertyInfo.GetCustomAttributes(typeof(TAttribute), inherit);
            return (TAttribute)(attrs.Length > 0 ? attrs[0] : null);
#endif
        }

        public static Type FirstGenericTypeDefinition(this Type type)
        {
            while (type != null)
            {
                if (type.HasGenericType())
                    return type.GenericTypeDefinition();

                type = type.BaseType();
            }

            return null;
        }

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

        public static object InvokeMethod(this Delegate fn, object instance, object[] parameters = null)
        {
#if NETFX_CORE
            return fn.GetMethodInfo().Invoke(instance, parameters ?? new object[] { });
#else
            return fn.Method.Invoke(instance, parameters ?? new object[] { });
#endif
        }

        public static FieldInfo GetPublicStaticField(this Type type, string fieldName)
        {
#if NETFX_CORE
            return type.GetRuntimeField(fieldName);
#else
            return type.GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
#endif
        }

        public static Delegate MakeDelegate(this MethodInfo mi, Type delegateType, bool throwOnBindFailure=true)
        {
#if NETFX_CORE
            return mi.CreateDelegate(delegateType);
#else
            return Delegate.CreateDelegate(delegateType, mi, throwOnBindFailure);
#endif
        }

        public static Type[] GenericTypeArguments(this Type type)
        {
#if NETFX_CORE
            return type.GenericTypeArguments;
#else
            return type.GetGenericArguments();
#endif
        }

        public static ConstructorInfo[] DeclaredConstructors(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().DeclaredConstructors.ToArray();
#else
            return type.GetConstructors();
#endif
        }

        public static bool AssignableFrom(this Type type, Type fromType)
        {
#if NETFX_CORE
            return type.GetTypeInfo().IsAssignableFrom(fromType.GetTypeInfo());
#else
            return type.IsAssignableFrom(fromType);
#endif
        }
        
        public static bool IsStandardClass(this Type type)
        {
#if NETFX_CORE
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsInterface;
#else
            return type.IsClass && !type.IsAbstract && !type.IsInterface;
#endif
        }

        public static bool IsAbstract(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().IsAbstract;
#else
            return type.IsAbstract;
#endif
        }

        public static PropertyInfo GetPropertyInfo(this Type type, string propertyName)
        {
#if NETFX_CORE
            return type.GetRuntimeProperty(propertyName);
#else
            return type.GetProperty(propertyName);
#endif
        }

        public static FieldInfo GetFieldInfo(this Type type, string fieldName)
        {
#if NETFX_CORE
            return type.GetRuntimeField(fieldName);
#else
            return type.GetField(fieldName);
#endif
        }

        public static FieldInfo[] GetWritableFields(this Type type)
        {
#if NETFX_CORE
            return type.GetRuntimeFields().Where(p => !p.IsPublic && !p.IsStatic).ToArray();
#else
            return type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField);
#endif
        }

        public static MethodInfo SetMethod(this PropertyInfo pi, bool nonPublic = true)
        {
#if NETFX_CORE
            return pi.SetMethod;
#else
            return pi.GetSetMethod(nonPublic);
#endif
        }

        public static MethodInfo GetMethodInfo(this PropertyInfo pi, bool nonPublic = true)
        {
#if NETFX_CORE
            return pi.GetMethod;
#else
            return pi.GetGetMethod(nonPublic);
#endif
        }

        public static bool InstanceOfType(this Type type, object instance)
        {
#if NETFX_CORE
            return type.IsInstanceOf(instance.GetType());
#else
            return type.IsInstanceOfType(instance);
#endif
        }
        
        public static bool IsClass(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().IsClass;
#else
            return type.IsClass;
#endif
        }

        public static bool IsEnum(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum;
#endif
        }

        public static bool IsEnumFlags(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().IsEnum && type.FirstAttribute<FlagsAttribute>(false) != null;
#else
            return type.IsEnum && type.FirstAttribute<FlagsAttribute>(false) != null;
#endif
        }

        public static bool IsUnderlyingEnum(this Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum || type.UnderlyingSystemType.IsEnum;
#endif
        }

        public static MethodInfo[] GetMethodInfos(this Type type)
        {
#if NETFX_CORE
            return type.GetRuntimeMethods().ToArray();
#else
            return type.GetMethods();
#endif
        }

        public static PropertyInfo[] GetPropertyInfos(this Type type)
        {
#if NETFX_CORE
            return type.GetRuntimeProperties().ToArray();
#else
            return type.GetProperties();
#endif
        }

#if SILVERLIGHT || NETFX_CORE
        public static List<U> ConvertAll<T, U>(this List<T> list, Func<T, U> converter)
        {
            var result = new List<U>();
            foreach (var element in list)
            {
                result.Add(converter(element));
            }
            return result;
        }
#endif
 
    
    }

}