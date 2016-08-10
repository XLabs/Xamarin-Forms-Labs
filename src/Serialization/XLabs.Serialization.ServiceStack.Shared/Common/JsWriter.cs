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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

using ServiceStack.Text.Json;
using ServiceStack.Text.Jsv;

namespace ServiceStack.Text.Common
{
	/// <summary>
	/// Class JsWriter.
	/// </summary>
	public static class JsWriter
    {
		/// <summary>
		/// The type attribute
		/// </summary>
		public const string TypeAttr = "__type";

		/// <summary>
		/// The map start character
		/// </summary>
		public const char MapStartChar = '{';
		/// <summary>
		/// The map key seperator
		/// </summary>
		public const char MapKeySeperator = ':';
		/// <summary>
		/// The item seperator
		/// </summary>
		public const char ItemSeperator = ',';
		/// <summary>
		/// The map end character
		/// </summary>
		public const char MapEndChar = '}';
		/// <summary>
		/// The map null value
		/// </summary>
		public const string MapNullValue = "\"\"";
		/// <summary>
		/// The empty map
		/// </summary>
		public const string EmptyMap = "{}";

		/// <summary>
		/// The list start character
		/// </summary>
		public const char ListStartChar = '[';
		/// <summary>
		/// The list end character
		/// </summary>
		public const char ListEndChar = ']';
		/// <summary>
		/// The return character
		/// </summary>
		public const char ReturnChar = '\r';
		/// <summary>
		/// The line feed character
		/// </summary>
		public const char LineFeedChar = '\n';

		/// <summary>
		/// The quote character
		/// </summary>
		public const char QuoteChar = '"';
		/// <summary>
		/// The quote string
		/// </summary>
		public const string QuoteString = "\"";
		/// <summary>
		/// The escaped quote string
		/// </summary>
		public const string EscapedQuoteString = "\\\"";
		/// <summary>
		/// The item seperator string
		/// </summary>
		public const string ItemSeperatorString = ",";
		/// <summary>
		/// The map key seperator string
		/// </summary>
		public const string MapKeySeperatorString = ":";

		/// <summary>
		/// The CSV chars
		/// </summary>
		public static readonly char[] CsvChars = new[] { ItemSeperator, QuoteChar };
		/// <summary>
		/// The escape chars
		/// </summary>
		public static readonly char[] EscapeChars = new[] { QuoteChar, MapKeySeperator, ItemSeperator, MapStartChar, MapEndChar, ListStartChar, ListEndChar, ReturnChar, LineFeedChar };

		/// <summary>
		/// The length from largest character
		/// </summary>
		private const int LengthFromLargestChar = '}' + 1;
		/// <summary>
		/// The escape character flags
		/// </summary>
		private static readonly bool[] EscapeCharFlags = new bool[LengthFromLargestChar];

		/// <summary>
		/// Initializes static members of the <see cref="JsWriter"/> class.
		/// </summary>
		static JsWriter()
        {
            foreach (var escapeChar in EscapeChars)
            {
                EscapeCharFlags[escapeChar] = true;
            }
            var loadConfig = JsConfig.EmitCamelCaseNames; //force load
        }

		/// <summary>
		/// Writes the dynamic.
		/// </summary>
		/// <param name="callback">The callback.</param>
		public static void WriteDynamic(Action callback)
        {
            JsState.IsWritingDynamic = true;
            try
            {
                callback();
            }
            finally
            {
                JsState.IsWritingDynamic = false;
            }
        }

		/// <summary>
		/// micro optimizations: using flags instead of value.IndexOfAny(EscapeChars)
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if [has any escape chars] [the specified value]; otherwise, <c>false</c>.</returns>
		public static bool HasAnyEscapeChars(string value)
        {
            var len = value.Length;
            for (var i = 0; i < len; i++)
            {
                var c = value[i];
                if (c >= LengthFromLargestChar || !EscapeCharFlags[c]) continue;
                return true;
            }
            return false;
        }

		/// <summary>
		/// Writes the item seperator if ran once.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="ranOnce">if set to <c>true</c> [ran once].</param>
		internal static void WriteItemSeperatorIfRanOnce(TextWriter writer, ref bool ranOnce)
        {
            if (ranOnce)
                writer.Write(ItemSeperator);
            else
                ranOnce = true;
        }

		/// <summary>
		/// Shoulds the use default to string method.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		internal static bool ShouldUseDefaultToStringMethod(Type type)
        {
            return type == typeof(byte) || type == typeof(byte?)
                || type == typeof(short) || type == typeof(short?)
                || type == typeof(ushort) || type == typeof(ushort?)
                || type == typeof(int) || type == typeof(int?)
                || type == typeof(uint) || type == typeof(uint?)
                || type == typeof(long) || type == typeof(long?)
                || type == typeof(ulong) || type == typeof(ulong?)
                || type == typeof(bool) || type == typeof(bool?)
                || type == typeof(DateTime) || type == typeof(DateTime?)
                || type == typeof(Guid) || type == typeof(Guid?)
                || type == typeof(float) || type == typeof(float?)
                || type == typeof(double) || type == typeof(double?)
                || type == typeof(decimal) || type == typeof(decimal?);
        }

		/// <summary>
		/// Gets the type serializer.
		/// </summary>
		/// <typeparam name="TSerializer">The type of the t serializer.</typeparam>
		/// <returns>ITypeSerializer.</returns>
		/// <exception cref="System.NotSupportedException"></exception>
		internal static ITypeSerializer GetTypeSerializer<TSerializer>()
        {
            if (typeof(TSerializer) == typeof(JsvTypeSerializer))
                return JsvTypeSerializer.Instance;

            if (typeof(TSerializer) == typeof(JsonTypeSerializer))
                return JsonTypeSerializer.Instance;

            throw new NotSupportedException(typeof(TSerializer).Name);
        }

#if NETFX_CORE
        private static readonly Type[] knownTypes = new Type[] {
                typeof(bool), typeof(char), typeof(sbyte), typeof(byte),
                typeof(short), typeof(ushort), typeof(int), typeof(uint),
                typeof(long), typeof(ulong), typeof(float), typeof(double),
                typeof(decimal), typeof(string),
                typeof(DateTime), typeof(TimeSpan), typeof(Guid), typeof(Uri),
                typeof(byte[]), typeof(System.Type)};

        private static readonly ServiceStackTypeCode[] knownCodes = new ServiceStackTypeCode[] {
            ServiceStackTypeCode.Boolean, ServiceStackTypeCode.Char, ServiceStackTypeCode.SByte, ServiceStackTypeCode.Byte,
            ServiceStackTypeCode.Int16, ServiceStackTypeCode.UInt16, ServiceStackTypeCode.Int32, ServiceStackTypeCode.UInt32,
            ServiceStackTypeCode.Int64, ServiceStackTypeCode.UInt64, ServiceStackTypeCode.Single, ServiceStackTypeCode.Double,
            ServiceStackTypeCode.Decimal, ServiceStackTypeCode.String,
            ServiceStackTypeCode.DateTime, ServiceStackTypeCode.TimeSpan, ServiceStackTypeCode.Guid, ServiceStackTypeCode.Uri,
            ServiceStackTypeCode.ByteArray, ServiceStackTypeCode.Type
        };

        internal enum ServiceStackTypeCode
        {
            Empty = 0,
            Unknown = 1, // maps to TypeCode.Object
            Boolean = 3,
            Char = 4,
            SByte = 5,
            Byte = 6,
            Int16 = 7,
            UInt16 = 8,
            Int32 = 9,
            UInt32 = 10,
            Int64 = 11,
            UInt64 = 12,
            Single = 13,
            Double = 14,
            Decimal = 15,
            DateTime = 16,
            String = 18,

            // additions
            TimeSpan = 100,
            ByteArray = 101,
            Guid = 102,
            Uri = 103,
            Type = 104
        }

        private static ServiceStackTypeCode GetTypeCode(Type type)
        {
            int idx = Array.IndexOf<Type>(knownTypes, type);
            if (idx >= 0) return knownCodes[idx];
            return type == null ? ServiceStackTypeCode.Empty : ServiceStackTypeCode.Unknown;
        }
#endif

		/// <summary>
		/// Writes the enum flags.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="enumFlagValue">The enum flag value.</param>
		public static void WriteEnumFlags(TextWriter writer, object enumFlagValue)
        {
            if (enumFlagValue == null) return;
#if NETFX_CORE
            var typeCode = GetTypeCode(Enum.GetUnderlyingType(enumFlagValue.GetType()));

            switch (typeCode)
            {
                case ServiceStackTypeCode.Byte:
                    writer.Write((byte)enumFlagValue);
                    break;
                case ServiceStackTypeCode.Int16:
                    writer.Write((short)enumFlagValue);
                    break;
                case ServiceStackTypeCode.UInt16:
                    writer.Write((ushort)enumFlagValue);
                    break;
                case ServiceStackTypeCode.Int32:
                    writer.Write((int)enumFlagValue);
                    break;
                case ServiceStackTypeCode.UInt32:
                    writer.Write((uint)enumFlagValue);
                    break;
                case ServiceStackTypeCode.Int64:
                    writer.Write((long)enumFlagValue);
                    break;
                case ServiceStackTypeCode.UInt64:
                    writer.Write((ulong)enumFlagValue);
                    break;
                default:
                    writer.Write((int)enumFlagValue);
                    break;
            }
#else
            var typeCode = Type.GetTypeCode(Enum.GetUnderlyingType(enumFlagValue.GetType()));

            switch (typeCode)
            {
                case TypeCode.SByte:
                    writer.Write((sbyte) enumFlagValue);
                    break;
                case TypeCode.Byte:
                    writer.Write((byte)enumFlagValue);
                    break;
                case TypeCode.Int16:
                    writer.Write((short)enumFlagValue);
                    break;
                case TypeCode.UInt16:
                    writer.Write((ushort)enumFlagValue);
                    break;
                case TypeCode.Int32:
                    writer.Write((int)enumFlagValue);
                    break;
                case TypeCode.UInt32:
                    writer.Write((uint)enumFlagValue);
                    break;
                case TypeCode.Int64:
                    writer.Write((long)enumFlagValue);
                    break;
                case TypeCode.UInt64:
                    writer.Write((ulong)enumFlagValue);
                    break;
                default:
                    writer.Write((int)enumFlagValue);
                    break;
            }
#endif
        }
    }

	/// <summary>
	/// Class JsWriter.
	/// </summary>
	/// <typeparam name="TSerializer">The type of the t serializer.</typeparam>
	internal class JsWriter<TSerializer>
        where TSerializer : ITypeSerializer
    {
		/// <summary>
		/// The serializer
		/// </summary>
		private static readonly ITypeSerializer Serializer = JsWriter.GetTypeSerializer<TSerializer>();

		/// <summary>
		/// Initializes a new instance of the <see cref="JsWriter{TSerializer}"/> class.
		/// </summary>
		public JsWriter()
        {
            this.SpecialTypes = new Dictionary<Type, WriteObjectDelegate>
        	{
        		{ typeof(Uri), Serializer.WriteObjectString },
        		{ typeof(Type), WriteType },
        		{ typeof(Exception), Serializer.WriteException },
#if !MONOTOUCH && !SILVERLIGHT && !XBOX  && !ANDROID
                { typeof(System.Data.Linq.Binary), Serializer.WriteLinqBinary },
#endif
        	};
        }

		/// <summary>
		/// Gets the value type to string method.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>WriteObjectDelegate.</returns>
		public WriteObjectDelegate GetValueTypeToStringMethod(Type type)
        {
            if (type == typeof(char) || type == typeof(char?))
                return Serializer.WriteChar;
            if (type == typeof(int) || type == typeof(int?))
                return Serializer.WriteInt32;
            if (type == typeof(long) || type == typeof(long?))
                return Serializer.WriteInt64;
            if (type == typeof(ulong) || type == typeof(ulong?))
                return Serializer.WriteUInt64;
            if (type == typeof(uint) || type == typeof(uint?))
                return Serializer.WriteUInt32;

            if (type == typeof(byte) || type == typeof(byte?))
                return Serializer.WriteByte;

            if (type == typeof(short) || type == typeof(short?))
                return Serializer.WriteInt16;
            if (type == typeof(ushort) || type == typeof(ushort?))
                return Serializer.WriteUInt16;

            if (type == typeof(bool) || type == typeof(bool?))
                return Serializer.WriteBool;

            if (type == typeof(DateTime))
                return Serializer.WriteDateTime;

            if (type == typeof(DateTime?))
                return Serializer.WriteNullableDateTime;

            if (type == typeof(DateTimeOffset))
                return Serializer.WriteDateTimeOffset;

            if (type == typeof(DateTimeOffset?))
                return Serializer.WriteNullableDateTimeOffset;

            if (type == typeof(TimeSpan))
                return Serializer.WriteTimeSpan;

            if (type == typeof(TimeSpan?))
                return Serializer.WriteNullableTimeSpan;

            if (type == typeof(Guid))
                return Serializer.WriteGuid;

            if (type == typeof(Guid?))
                return Serializer.WriteNullableGuid;

            if (type == typeof(float) || type == typeof(float?))
                return Serializer.WriteFloat;

            if (type == typeof(double) || type == typeof(double?))
                return Serializer.WriteDouble;

            if (type == typeof(decimal) || type == typeof(decimal?))
                return Serializer.WriteDecimal;

            if (type.IsUnderlyingEnum())
                return type.FirstAttribute<FlagsAttribute>(false) != null
                    ? (WriteObjectDelegate)Serializer.WriteEnumFlags
                    : Serializer.WriteEnum;

            Type nullableType;
            if ((nullableType = Nullable.GetUnderlyingType(type)) != null && nullableType.IsEnum())
                return nullableType.FirstAttribute<FlagsAttribute>(false) != null
                    ? (WriteObjectDelegate)Serializer.WriteEnumFlags
                    : Serializer.WriteEnum;

            if (type.HasInterface(typeof (IFormattable)))
                return Serializer.WriteFormattableObjectString;

            return Serializer.WriteObjectString;
        }

		/// <summary>
		/// Gets the write function.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>WriteObjectDelegate.</returns>
		internal WriteObjectDelegate GetWriteFn<T>()
        {
            if (typeof(T) == typeof(string))
            {
                return Serializer.WriteObjectString;
            }

            var onSerializingFn = JsConfig<T>.OnSerializingFn;
            if (onSerializingFn != null)
            {
                return (w, x) => GetCoreWriteFn<T>()(w, onSerializingFn((T)x));
            }

            if (JsConfig<T>.HasSerializeFn)
            {
                return JsConfig<T>.WriteFn<TSerializer>;
            }

            return GetCoreWriteFn<T>();
        }

		/// <summary>
		/// Gets the core write function.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>WriteObjectDelegate.</returns>
		private WriteObjectDelegate GetCoreWriteFn<T>()
        {
            if ((typeof(T).IsValueType() && !JsConfig.TreatAsRefType(typeof(T))) || JsConfig<T>.HasSerializeFn)
            {
                return JsConfig<T>.HasSerializeFn
                    ? JsConfig<T>.WriteFn<TSerializer>
                    : GetValueTypeToStringMethod(typeof(T));
            }

            var specialWriteFn = GetSpecialWriteFn(typeof(T));
            if (specialWriteFn != null)
            {
                return specialWriteFn;
            }

            if (typeof(T).IsArray)
            {
                if (typeof(T) == typeof(byte[]))
                    return (w, x) => WriteLists.WriteBytes(Serializer, w, x);

                if (typeof(T) == typeof(string[]))
                    return (w, x) => WriteLists.WriteStringArray(Serializer, w, x);

                if (typeof(T) == typeof(int[]))
                    return WriteListsOfElements<int, TSerializer>.WriteGenericArrayValueType;
                if (typeof(T) == typeof(long[]))
                    return WriteListsOfElements<long, TSerializer>.WriteGenericArrayValueType;

                var elementType = typeof(T).GetElementType();
                var writeFn = WriteListsOfElements<TSerializer>.GetGenericWriteArray(elementType);
                return writeFn;
            }

            if (typeof(T).HasGenericType() ||
                typeof(T).HasInterface(typeof(IDictionary<string, object>))) // is ExpandoObject?
            {
                if (typeof(T).IsOrHasGenericInterfaceTypeOf(typeof(IList<>)))
                    return WriteLists<T, TSerializer>.Write;

                var mapInterface = typeof(T).GetTypeWithGenericTypeDefinitionOf(typeof(IDictionary<,>));
                if (mapInterface != null)
                {
                    var mapTypeArgs = mapInterface.GenericTypeArguments();
                    var writeFn = WriteDictionary<TSerializer>.GetWriteGenericDictionary(
                        mapTypeArgs[0], mapTypeArgs[1]);

                    var keyWriteFn = Serializer.GetWriteFn(mapTypeArgs[0]);
                    var valueWriteFn = typeof(T) == typeof(JsonObject)
                        ? JsonObject.WriteValue
                        : Serializer.GetWriteFn(mapTypeArgs[1]);

                    return (w, x) => writeFn(w, x, keyWriteFn, valueWriteFn);
                }
            }

            var enumerableInterface = typeof(T).GetTypeWithGenericTypeDefinitionOf(typeof(IEnumerable<>));
            if (enumerableInterface != null)
            {
                var elementType = enumerableInterface.GenericTypeArguments()[0];
                var writeFn = WriteListsOfElements<TSerializer>.GetGenericWriteEnumerable(elementType);
                return writeFn;
            }

            var isDictionary = typeof(T) != typeof(IEnumerable) && typeof(T) != typeof(ICollection)
                && (typeof(T).AssignableFrom(typeof(IDictionary)) || typeof(T).HasInterface(typeof(IDictionary)));
            if (isDictionary)
            {
                return WriteDictionary<TSerializer>.WriteIDictionary;
            }

            var isEnumerable = typeof(T).AssignableFrom(typeof(IEnumerable))
                || typeof(T).HasInterface(typeof(IEnumerable));
            if (isEnumerable)
            {
                return WriteListsOfElements<TSerializer>.WriteIEnumerable;
            }

            if (typeof(T).IsClass() || typeof(T).IsInterface() || JsConfig.TreatAsRefType(typeof(T)))
            {
                var typeToStringMethod = WriteType<T, TSerializer>.Write;
                if (typeToStringMethod != null)
                {
                    return typeToStringMethod;
                }
            }

            return Serializer.WriteBuiltIn;
        }

		/// <summary>
		/// The special types
		/// </summary>
		public Dictionary<Type, WriteObjectDelegate> SpecialTypes;

		/// <summary>
		/// Gets the special write function.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>WriteObjectDelegate.</returns>
		public WriteObjectDelegate GetSpecialWriteFn(Type type)
        {
            WriteObjectDelegate writeFn = null;
            if (SpecialTypes.TryGetValue(type, out writeFn))
                return writeFn;

            if (type.InstanceOfType(typeof(Type)))
                return WriteType;

            if (type.IsInstanceOf(typeof(Exception)))
                return Serializer.WriteException;

            return null;
        }

		/// <summary>
		/// Writes the type.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="value">The value.</param>
		public void WriteType(TextWriter writer, object value)
        {
            Serializer.WriteRawString(writer, JsConfig.TypeWriter((Type)value));
        }

    }
}
