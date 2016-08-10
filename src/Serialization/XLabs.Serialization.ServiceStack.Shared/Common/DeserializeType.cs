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

#if !XBOX && !MONOTOUCH && !SILVERLIGHT
using System.Reflection.Emit;
#endif

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ServiceStack.Text.Json;

namespace ServiceStack.Text.Common
{
    internal static class DeserializeType<TSerializer>
        where TSerializer : ITypeSerializer
    {
        private static readonly ITypeSerializer Serializer = JsWriter.GetTypeSerializer<TSerializer>();

        public static ParseStringDelegate GetParseMethod(TypeConfig typeConfig)
        {
            var type = typeConfig.Type;

            if (!type.IsStandardClass()) return null;
            var map = DeserializeTypeRef.GetTypeAccessorMap(typeConfig, Serializer);

            var ctorFn = JsConfig.ModelFactory(type);
            if (map == null) return value => ctorFn();

            return typeof(TSerializer) == typeof(JsonTypeSerializer)
                ? (ParseStringDelegate)(value => DeserializeTypeRefJson.StringToType(type, value, ctorFn, map))
                : value => DeserializeTypeRefJsv.StringToType(type, value, ctorFn, map);
        }

        public static object ObjectStringToType(string strType)
        {
            var type = ExtractType(strType);
            if (type != null)
            {
                var parseFn = Serializer.GetParseFn(type);
                var propertyValue = parseFn(strType);
                return propertyValue;
            }

            if (JsConfig.ConvertObjectTypesIntoStringDictionary && !string.IsNullOrEmpty(strType))
            {
                if (strType[0] == JsWriter.MapStartChar)
                {
                    var dynamicMatch = DeserializeDictionary<TSerializer>.ParseDictionary<string, object>(strType, null, Serializer.UnescapeString, Serializer.UnescapeString);
                    if (dynamicMatch != null && dynamicMatch.Count > 0)
                    {
                        return dynamicMatch;
                    }
                }

                if (strType[0] == JsWriter.ListStartChar)
                {
                    return DeserializeList<List<object>, TSerializer>.Parse(strType);
                }
            }

            return strType;

            //return Serializer.UnescapeString(strType);
        }

        public static Type ExtractType(string strType)
        {
            var typeAttrInObject = Serializer.TypeAttrInObject;
            if (strType != null
                && strType.Length > typeAttrInObject.Length
                && strType.Substring(0, typeAttrInObject.Length) == typeAttrInObject)
            {
                var propIndex = typeAttrInObject.Length;
                var typeName = Serializer.UnescapeSafeString(Serializer.EatValue(strType, ref propIndex));

                var type = JsConfig.TypeFinder.Invoke(typeName);

                if (type == null)
                {
                    Tracer.Instance.WriteWarning("Could not find type: " + typeName);
                    return null;
                }

#if !SILVERLIGHT && !MONOTOUCH
                if (type.IsInterface || type.IsAbstract)
                {
                    return DynamicProxy.GetInstanceFor(type).GetType();
                }
#endif

                return type;
            }
            return null;
        }

        public static object ParseAbstractType<T>(string value)
        {
            if (typeof(T).IsAbstract())
            {
                if (string.IsNullOrEmpty(value)) return null;
                var concreteType = ExtractType(value);
                if (concreteType != null)
                {
                    return Serializer.GetParseFn(concreteType)(value);
                }
                Tracer.Instance.WriteWarning(
                    "Could not deserialize Abstract Type with unknown concrete type: " + typeof(T).FullName);
            }
            return null;
        }

        public static object ParseQuotedPrimitive(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

#if NET40
            Guid guidValue;
            if (Guid.TryParse(value, out guidValue)) return guidValue;
#endif
            if (value.StartsWith(DateTimeSerializer.EscapedWcfJsonPrefix, StringComparison.Ordinal) || value.StartsWith(DateTimeSerializer.WcfJsonPrefix, StringComparison.Ordinal))
            {
                return DateTimeSerializer.ParseWcfJsonDate(value);
            }

            if (JsConfig.DateHandler == JsonDateHandler.ISO8601)
            {
                // check that we have UTC ISO8601 date:
                // YYYY-MM-DDThh:mm:ssZ
                // YYYY-MM-DDThh:mm:ss+02:00
                // YYYY-MM-DDThh:mm:ss-02:00
                if (value.Length > 14 && value[10] == 'T' &&
                    (value.EndsWithInvariant("Z")
                    || value[value.Length - 6] == '+'
                    || value[value.Length - 6] == '-'))
                {
                    return DateTimeSerializer.ParseShortestXsdDateTime(value);
                }
            }

            return Serializer.UnescapeString(value);
        }

        public static object ParsePrimitive(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            bool boolValue;
            if (bool.TryParse(value, out boolValue)) return boolValue;

            decimal decimalValue;
            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimalValue))
            {
                if (!JsConfig.TryToParseNumericType)
                    return decimalValue;

                if (decimalValue == decimal.Truncate(decimalValue))
                {
                    if (decimalValue <= byte.MaxValue && decimalValue >= byte.MinValue) return (byte)decimalValue;
                    if (decimalValue <= sbyte.MaxValue && decimalValue >= sbyte.MinValue) return (sbyte)decimalValue;
                    if (decimalValue <= Int16.MaxValue && decimalValue >= Int16.MinValue) return (Int16)decimalValue;
                    if (decimalValue <= UInt16.MaxValue && decimalValue >= UInt16.MinValue) return (UInt16)decimalValue;
                    if (decimalValue <= Int32.MaxValue && decimalValue >= Int32.MinValue) return (Int32)decimalValue;
                    if (decimalValue <= UInt32.MaxValue && decimalValue >= UInt32.MinValue) return (UInt32)decimalValue;
                    if (decimalValue <= Int64.MaxValue && decimalValue >= Int64.MinValue) return (Int64)decimalValue;
                    if (decimalValue <= UInt64.MaxValue && decimalValue >= UInt64.MinValue) return (UInt64)decimalValue;
                }
                return decimalValue;
            }

            float floatValue;
            if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out floatValue)) return floatValue;

            double doubleValue;
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out doubleValue)) return doubleValue;

            return null;
        }

        internal static object ParsePrimitive(string value, char firstChar)
        {
            if (typeof(TSerializer) == typeof(JsonTypeSerializer))
            {
                return firstChar == JsWriter.QuoteChar
                           ? ParseQuotedPrimitive(value)
                           : ParsePrimitive(value);
            }
            return (ParsePrimitive(value) ?? ParseQuotedPrimitive(value));
        }
    }

    internal class TypeAccessor
    {
        internal ParseStringDelegate GetProperty;
        internal SetPropertyDelegate SetProperty;
        internal Type PropertyType;

        public static Type ExtractType(ITypeSerializer Serializer, string strType)
        {
            var typeAttrInObject = Serializer.TypeAttrInObject;

            if (strType != null
                && strType.Length > typeAttrInObject.Length
                && strType.Substring(0, typeAttrInObject.Length) == typeAttrInObject)
            {
                var propIndex = typeAttrInObject.Length;
                var typeName = Serializer.EatValue(strType, ref propIndex);
                var type = JsConfig.TypeFinder.Invoke(typeName);

                if (type == null)
                    Tracer.Instance.WriteWarning("Could not find type: " + typeName);

                return type;
            }
            return null;
        }

        public static TypeAccessor Create(ITypeSerializer serializer, TypeConfig typeConfig, PropertyInfo propertyInfo)
        {
            return new TypeAccessor
            {
                PropertyType = propertyInfo.PropertyType,
                GetProperty = serializer.GetParseFn(propertyInfo.PropertyType),
                SetProperty = GetSetPropertyMethod(typeConfig, propertyInfo),
            };
        }

        private static SetPropertyDelegate GetSetPropertyMethod(TypeConfig typeConfig, PropertyInfo propertyInfo)
        {
            if (propertyInfo.ReflectedType() != propertyInfo.DeclaringType)
                propertyInfo = propertyInfo.DeclaringType.GetPropertyInfo(propertyInfo.Name);

            if (!propertyInfo.CanWrite && !typeConfig.EnableAnonymousFieldSetterses) return null;

            FieldInfo fieldInfo = null;
            if (!propertyInfo.CanWrite)
            {
                //TODO: What string comparison is used in SST?
                string fieldNameFormat = Env.IsMono ? "<{0}>" : "<{0}>i__Field";
                var fieldName = string.Format(fieldNameFormat, propertyInfo.Name);

                var fieldInfos = typeConfig.Type.GetWritableFields();
                foreach (var f in fieldInfos)
                {
                    if (f.IsInitOnly && f.FieldType == propertyInfo.PropertyType && f.Name == fieldName)
                    {
                        fieldInfo = f;
                        break;
                    }
                }

                if (fieldInfo == null) return null;
            }

#if SILVERLIGHT || MONOTOUCH || XBOX
            if (propertyInfo.CanWrite)
            {
                var setMethodInfo = propertyInfo.SetMethod();
                return (instance, value) => setMethodInfo.Invoke(instance, new[] { value });
            }
            if (fieldInfo == null) return null;
            return (instance, value) => fieldInfo.SetValue(instance, value);
#else
            return propertyInfo.CanWrite
                ? CreateIlPropertySetter(propertyInfo)
                : CreateIlFieldSetter(fieldInfo);
#endif
        }

#if !SILVERLIGHT && !MONOTOUCH && !XBOX

        private static SetPropertyDelegate CreateIlPropertySetter(PropertyInfo propertyInfo)
        {
            var propSetMethod = propertyInfo.GetSetMethod(true);
            if (propSetMethod == null)
                return null;

            var setter = CreateDynamicSetMethod(propertyInfo);

            var generator = setter.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            generator.Emit(OpCodes.Ldarg_1);

            generator.Emit(propertyInfo.PropertyType.IsClass
                ? OpCodes.Castclass
                : OpCodes.Unbox_Any,
                propertyInfo.PropertyType);

            generator.EmitCall(OpCodes.Callvirt, propSetMethod, (Type[])null);
            generator.Emit(OpCodes.Ret);

            return (SetPropertyDelegate)setter.CreateDelegate(typeof(SetPropertyDelegate));
        }

        private static SetPropertyDelegate CreateIlFieldSetter(FieldInfo fieldInfo)
        {
            var setter = CreateDynamicSetMethod(fieldInfo);

            var generator = setter.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, fieldInfo.DeclaringType);
            generator.Emit(OpCodes.Ldarg_1);

            generator.Emit(fieldInfo.FieldType.IsClass
                ? OpCodes.Castclass
                : OpCodes.Unbox_Any,
                fieldInfo.FieldType);

            generator.Emit(OpCodes.Stfld, fieldInfo);
            generator.Emit(OpCodes.Ret);

            return (SetPropertyDelegate)setter.CreateDelegate(typeof(SetPropertyDelegate));
        }

        private static DynamicMethod CreateDynamicSetMethod(MemberInfo memberInfo)
        {
            var args = new[] { typeof(object), typeof(object) };
            var name = string.Format("_{0}{1}_", "Set", memberInfo.Name);
            var returnType = typeof(void);

            return !memberInfo.DeclaringType.IsInterface
                ? new DynamicMethod(name, returnType, args, memberInfo.DeclaringType, true)
                : new DynamicMethod(name, returnType, args, memberInfo.Module, true);
        }
#endif

        internal static SetPropertyDelegate GetSetPropertyMethod(Type type, PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanWrite || propertyInfo.GetIndexParameters().Any()) return null;

#if SILVERLIGHT || MONOTOUCH || XBOX
            var setMethodInfo = propertyInfo.SetMethod();
            return (instance, value) => setMethodInfo.Invoke(instance, new[] { value });
#else
            return CreateIlPropertySetter(propertyInfo);
#endif
        }

        internal static SetPropertyDelegate GetSetFieldMethod(Type type, FieldInfo fieldInfo)
        {

#if SILVERLIGHT || MONOTOUCH || XBOX
            return (instance, value) => fieldInfo.SetValue(instance, value);
#else
            return CreateIlFieldSetter(fieldInfo);
#endif
        }


        public static TypeAccessor Create(ITypeSerializer serializer, TypeConfig typeConfig, FieldInfo fieldInfo)
        {
            return new TypeAccessor
            {
                PropertyType = fieldInfo.FieldType,
                GetProperty = serializer.GetParseFn(fieldInfo.FieldType),
                SetProperty = GetSetFieldMethod(typeConfig, fieldInfo),
            };
            
        }

        private static SetPropertyDelegate GetSetFieldMethod(TypeConfig typeConfig, FieldInfo fieldInfo)
        {
            if (fieldInfo.ReflectedType() != fieldInfo.DeclaringType)
                fieldInfo = fieldInfo.DeclaringType.GetFieldInfo(fieldInfo.Name);

#if SILVERLIGHT || MONOTOUCH || XBOX
            return (instance, value) => fieldInfo.SetValue(instance, value);
#else
            return CreateIlFieldSetter(fieldInfo);
#endif
        }
    }
}
