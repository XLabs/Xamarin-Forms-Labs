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
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using ServiceStack.Text.Json;
using ServiceStack.Text.Reflection;

namespace ServiceStack.Text.Common
{
    internal static class WriteType<T, TSerializer>
        where TSerializer : ITypeSerializer
    {
        private static readonly ITypeSerializer Serializer = JsWriter.GetTypeSerializer<TSerializer>();

        private static readonly WriteObjectDelegate CacheFn;
        internal static TypePropertyWriter[] PropertyWriters;
        private static readonly WriteObjectDelegate WriteTypeInfo;

        private static bool IsIncluded
        {
            get { return (JsConfig.IncludeTypeInfo || JsConfig<T>.IncludeTypeInfo); }
        }
        private static bool IsExcluded
        {
            get { return (JsConfig.ExcludeTypeInfo || JsConfig<T>.ExcludeTypeInfo); }
        }

        static WriteType()
        {
            if (typeof(T) == typeof(Object))
            {
                CacheFn = WriteObjectType;
            }
            else
            {
                CacheFn = Init() ? GetWriteFn() : WriteEmptyType;
            }

            if (IsIncluded)
            {
                WriteTypeInfo = TypeInfoWriter;
            }

            if (typeof(T).IsAbstract())
            {
                WriteTypeInfo = TypeInfoWriter;
                if (!JsConfig.PreferInterfaces || !typeof(T).IsInterface())
                {
                    CacheFn = WriteAbstractProperties;
                }
            }
        }

        public static void TypeInfoWriter(TextWriter writer, object obj)
        {
            TryWriteTypeInfo(writer, obj);
        }

        private static bool ShouldSkipType() { return IsExcluded && !IsIncluded; }

        private static bool TryWriteSelfType(TextWriter writer)
        {
            if (ShouldSkipType()) return false;

            Serializer.WriteRawString(writer, JsConfig.TypeAttr);
            writer.Write(JsWriter.MapKeySeperator);
            Serializer.WriteRawString(writer, JsConfig.TypeWriter(typeof(T)));
            return true;
        }

        private static bool TryWriteTypeInfo(TextWriter writer, object obj)
        {
            if (obj == null || ShouldSkipType()) return false;

            Serializer.WriteRawString(writer, JsConfig.TypeAttr);
            writer.Write(JsWriter.MapKeySeperator);
            Serializer.WriteRawString(writer, JsConfig.TypeWriter(obj.GetType()));
            return true;
        }

        public static WriteObjectDelegate Write
        {
            get { return CacheFn; }
        }

        private static WriteObjectDelegate GetWriteFn()
        {
            return WriteProperties;
        }

        static Func<T, bool> GetShouldSerializeMethod(MemberInfo member)
        {
            var method = member.DeclaringType.GetMethod("ShouldSerialize" + member.Name, BindingFlags.Instance | BindingFlags.Public,
                null, Type.EmptyTypes, null);
            return (method == null || method.ReturnType != typeof(bool)) ? null : (Func<T,bool>)Delegate.CreateDelegate(typeof(Func<T,bool>), method);
        }
        private static bool Init()
        {
            if (!typeof(T).IsClass() && !typeof(T).IsInterface() && !JsConfig.TreatAsRefType(typeof(T))) return false;

            var propertyInfos = TypeConfig<T>.Properties;
            var fieldInfos = JsConfig.IncludePublicFields ? TypeConfig<T>.Fields : new FieldInfo[0];
            var propertyNamesLength = propertyInfos.Length;
            var fieldNamesLength = fieldInfos.Length;
            PropertyWriters = new TypePropertyWriter[propertyNamesLength + fieldNamesLength];

            if (propertyNamesLength + fieldNamesLength == 0 && !JsState.IsWritingDynamic)
            {
                return typeof(T).IsDto();
            }

            // NOTE: very limited support for DataContractSerialization (DCS)
            //	NOT supporting Serializable
            //	support for DCS is intended for (re)Name of properties and Ignore by NOT having a DataMember present
            var isDataContract = typeof(T).IsDto();
            for (var i = 0; i < propertyNamesLength; i++)
            {
                var propertyInfo = propertyInfos[i];

                string propertyName, propertyNameCLSFriendly, propertyNameLowercaseUnderscore, propertyReflectedName;
                int propertyOrder = -1;
                var propertyType = propertyInfo.PropertyType;
                var defaultValue = propertyType.GetDefaultValue();
                bool propertySuppressDefaultConfig = defaultValue != null && propertyType.IsValueType() && JsConfig.HasSerializeFn.Contains(propertyType);
                bool propertySuppressDefaultAttribute = false;
                var shouldSerialize = GetShouldSerializeMethod(propertyInfo);
                if (isDataContract)
                {
                    var dcsDataMember = propertyInfo.GetDataMember();
                    if (dcsDataMember == null) continue;

                    propertyName = dcsDataMember.Name ?? propertyInfo.Name;
                    propertyNameCLSFriendly = dcsDataMember.Name ?? propertyName.ToCamelCase();
                    propertyNameLowercaseUnderscore = dcsDataMember.Name ?? propertyName.ToLowercaseUnderscore();
                    propertyReflectedName = dcsDataMember.Name ?? propertyInfo.ReflectedType.Name;
                    propertyOrder = dcsDataMember.Order;
                    propertySuppressDefaultAttribute = !dcsDataMember.EmitDefaultValue;
                }
                else
                {
                    propertyName = propertyInfo.Name;
                    propertyNameCLSFriendly = propertyName.ToCamelCase();
                    propertyNameLowercaseUnderscore = propertyName.ToLowercaseUnderscore();
                    propertyReflectedName = propertyInfo.ReflectedType.Name;
                }


                PropertyWriters[i] = new TypePropertyWriter
                (
                    propertyName,
                    propertyReflectedName,
                    propertyNameCLSFriendly,
                    propertyNameLowercaseUnderscore,
                    propertyOrder,
                    propertySuppressDefaultConfig,
                    propertySuppressDefaultAttribute,
                    propertyInfo.GetValueGetter<T>(),
                    Serializer.GetWriteFn(propertyType),
                    propertyType.GetDefaultValue(),
                    shouldSerialize
                );
            }

            for (var i = 0; i < fieldNamesLength; i++)
            {
                var fieldInfo = fieldInfos[i];

                string propertyName, propertyNameCLSFriendly, propertyNameLowercaseUnderscore, propertyReflectedName;
                int propertyOrder = -1;
                var propertyType = fieldInfo.FieldType;
                var defaultValue = propertyType.GetDefaultValue();
                bool propertySuppressDefaultConfig = defaultValue != null && propertyType.IsValueType() && JsConfig.HasSerializeFn.Contains(propertyType);
                bool propertySuppressDefaultAttribute = false;
                var shouldSerialize = GetShouldSerializeMethod(fieldInfo);
                if (isDataContract)
                {
                    var dcsDataMember = fieldInfo.GetDataMember();
                    if (dcsDataMember == null) continue;

                    propertyName = dcsDataMember.Name ?? fieldInfo.Name;
                    propertyNameCLSFriendly = dcsDataMember.Name ?? propertyName.ToCamelCase();
                    propertyNameLowercaseUnderscore = dcsDataMember.Name ?? propertyName.ToLowercaseUnderscore();
                    propertyReflectedName = dcsDataMember.Name ?? fieldInfo.ReflectedType.Name;
                    propertyOrder = dcsDataMember.Order;
                    propertySuppressDefaultAttribute = !dcsDataMember.EmitDefaultValue;
                }
                else
                {
                    propertyName = fieldInfo.Name;
                    propertyNameCLSFriendly = propertyName.ToCamelCase();
                    propertyNameLowercaseUnderscore = propertyName.ToLowercaseUnderscore();
                    propertyReflectedName = fieldInfo.ReflectedType.Name;
                }

                PropertyWriters[i + propertyNamesLength] = new TypePropertyWriter
                (
                    propertyName,
                    propertyReflectedName,
                    propertyNameCLSFriendly,
                    propertyNameLowercaseUnderscore,
                    propertyOrder,
                    propertySuppressDefaultConfig,
                    propertySuppressDefaultAttribute,
                    fieldInfo.GetValueGetter<T>(),
                    Serializer.GetWriteFn(propertyType),
                    defaultValue,
                    shouldSerialize
                );
            }
            PropertyWriters = PropertyWriters.OrderBy(x => x.propertyOrder).ToArray();
            return true;
        }

        internal struct TypePropertyWriter
        {
            internal string PropertyName
            {
                get
                {
                    return (JsConfig<T>.EmitCamelCaseNames || JsConfig.EmitCamelCaseNames)
                        ? propertyNameCLSFriendly
                        : (JsConfig<T>.EmitLowercaseUnderscoreNames || JsConfig.EmitLowercaseUnderscoreNames)
                            ? propertyNameLowercaseUnderscore
                            : propertyName;
                }
            }
            internal readonly string propertyName;
            internal readonly int propertyOrder;
            internal readonly bool propertySuppressDefaultConfig;
            internal readonly bool propertySuppressDefaultAttribute;
            internal readonly string propertyReflectedName;
            internal readonly string propertyCombinedNameUpper;
            internal readonly string propertyNameCLSFriendly;
            internal readonly string propertyNameLowercaseUnderscore;
            internal readonly Func<T, object> GetterFn;
            internal readonly WriteObjectDelegate WriteFn;
            internal readonly object DefaultValue;
            internal readonly Func<T, bool> shouldSerialize;

            public TypePropertyWriter(string propertyName, string propertyReflectedName, string propertyNameCLSFriendly, string propertyNameLowercaseUnderscore, int propertyOrder, bool propertySuppressDefaultConfig,bool propertySuppressDefaultAttribute,
                Func<T, object> getterFn, WriteObjectDelegate writeFn, object defaultValue, Func<T, bool> shouldSerialize)
            {
                this.propertyName = propertyName;
                this.propertyOrder = propertyOrder;
                this.propertySuppressDefaultConfig = propertySuppressDefaultConfig;
                this.propertySuppressDefaultAttribute = propertySuppressDefaultAttribute;
                this.propertyReflectedName = propertyReflectedName;
                this.propertyCombinedNameUpper = propertyReflectedName.ToUpper() + "." + propertyName.ToUpper();
                this.propertyNameCLSFriendly = propertyNameCLSFriendly;
                this.propertyNameLowercaseUnderscore = propertyNameLowercaseUnderscore;
                this.GetterFn = getterFn;
                this.WriteFn = writeFn;
                this.DefaultValue = defaultValue;
                this.shouldSerialize = shouldSerialize;
            }
        }

        public static void WriteObjectType(TextWriter writer, object value)
        {
            writer.Write(JsWriter.EmptyMap);
        }

        public static void WriteEmptyType(TextWriter writer, object value)
        {
            if (WriteTypeInfo != null || JsState.IsWritingDynamic)
            {
                writer.Write(JsWriter.MapStartChar);
                if (!(JsConfig.PreferInterfaces && TryWriteSelfType(writer)))
                {
                    TryWriteTypeInfo(writer, value);
                }
                writer.Write(JsWriter.MapEndChar);
                return;
            }
            writer.Write(JsWriter.EmptyMap);
        }

        public static void WriteAbstractProperties(TextWriter writer, object value)
        {
            if (value == null)
            {
                writer.Write(JsWriter.EmptyMap);
                return;
            }
            var valueType = value.GetType();
            if (valueType.IsAbstract())
            {
                WriteProperties(writer, value);
                return;
            }

            var writeFn = Serializer.GetWriteFn(valueType);
            if (!JsConfig<T>.ExcludeTypeInfo) JsState.IsWritingDynamic = true;
            writeFn(writer, value);
            if (!JsConfig<T>.ExcludeTypeInfo) JsState.IsWritingDynamic = false;
        }

        public static void WriteProperties(TextWriter writer, object value)
        {
            if (typeof(TSerializer) == typeof(JsonTypeSerializer) && JsState.WritingKeyCount > 0)
                writer.Write(JsWriter.QuoteChar);

            writer.Write(JsWriter.MapStartChar);

            var i = 0;
            if (WriteTypeInfo != null || JsState.IsWritingDynamic)
            {
                if (JsConfig.PreferInterfaces && TryWriteSelfType(writer)) i++;
                else if (TryWriteTypeInfo(writer, value)) i++;
                JsState.IsWritingDynamic = false;
            }

            if (PropertyWriters != null)
            {
                var len = PropertyWriters.Length;
                var exclude = JsConfig.ExcludePropertyReferences ?? new string[0];
                ConvertToUpper(exclude);
                for (int index = 0; index < len; index++)
                {
                    var propertyWriter = PropertyWriters[index];

                    if (propertyWriter.shouldSerialize != null && !propertyWriter.shouldSerialize((T)value)) continue;

                    var propertyValue = value != null
                        ? propertyWriter.GetterFn((T)value)
                        : null;
                    
                    if (propertyWriter.propertySuppressDefaultAttribute && Equals(propertyWriter.DefaultValue, propertyValue))
                    {
                        continue;
                    }
                    if ((propertyValue == null 
                         || (propertyWriter.propertySuppressDefaultConfig && Equals(propertyWriter.DefaultValue, propertyValue)))
                        && !Serializer.IncludeNullValues
                        )
                    {
                        continue;
                    }

                    if (exclude.Any() && exclude.Contains(propertyWriter.propertyCombinedNameUpper)) continue;

                    if (i++ > 0)
                        writer.Write(JsWriter.ItemSeperator);

                    Serializer.WritePropertyName(writer, propertyWriter.PropertyName);
                    writer.Write(JsWriter.MapKeySeperator);

                    if (typeof(TSerializer) == typeof(JsonTypeSerializer)) JsState.IsWritingValue = true;
                    if (propertyValue == null)
                    {
                        writer.Write(JsonUtils.Null);
                    }
                    else
                    {
                        propertyWriter.WriteFn(writer, propertyValue);
                    }
                    if (typeof(TSerializer) == typeof(JsonTypeSerializer)) JsState.IsWritingValue = false;
                }
            }

            writer.Write(JsWriter.MapEndChar);

            if (typeof(TSerializer) == typeof(JsonTypeSerializer) && JsState.WritingKeyCount > 0)
                writer.Write(JsWriter.QuoteChar);
        }

        private static readonly char[] ArrayBrackets = new[] { '[', ']' };

        public static void WriteQueryString(TextWriter writer, object value)
        {
            try
            {
                JsState.QueryStringMode = true;
                var i = 0;
                foreach (var propertyWriter in PropertyWriters)
                {
                    var propertyValue = propertyWriter.GetterFn((T)value);
                    if (propertyValue == null) continue;

                    if (i++ > 0)
                        writer.Write('&');

                    Serializer.WritePropertyName(writer, propertyWriter.PropertyName);
                    writer.Write('=');

                    var isEnumerable = propertyValue != null
                        && !(propertyValue is string)
                        && !(propertyValue.GetType().IsValueType())
                        && propertyValue.GetType().HasInterface(typeof(IEnumerable));

                    if (!isEnumerable)
                    {
                        propertyWriter.WriteFn(writer, propertyValue);
                    }
                    else
                    {                        
                        //Trim brackets in top-level lists in QueryStrings, e.g: ?a=[1,2,3] => ?a=1,2,3
                        using (var ms = new MemoryStream())
                        using (var enumerableWriter = new StreamWriter(ms))
                        {
                            propertyWriter.WriteFn(enumerableWriter, propertyValue); 
                            enumerableWriter.Flush();
                            var output = ms.ToArray().FromUtf8Bytes();
                            output = output.Trim(ArrayBrackets);
                            writer.Write(output);
                        }
                    }
                }
            }
            finally
            {
                JsState.QueryStringMode = false;
            }
        }

        private static void ConvertToUpper(string[] strArray)
        {
            for (var i = 0; i < strArray.Length; ++i)
            {
                if (strArray[i] != null)
                    strArray[i] = strArray[i].ToUpper();
            }
        }
    }
}
