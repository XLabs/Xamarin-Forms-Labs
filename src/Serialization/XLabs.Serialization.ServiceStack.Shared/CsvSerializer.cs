// ***********************************************************************
// <copyright file="CsvSerializer.cs" company="XLabs">
//     Copyright © ServiceStack 2013 & XLabs
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using ServiceStack.Text.Common;
using ServiceStack.Text.Jsv;
using ServiceStack.Text.Reflection;

namespace ServiceStack.Text
{
	/// <summary>
	/// Class CsvSerializer.
	/// </summary>
	public class CsvSerializer
    {
		/// <summary>
		/// The ut f8 encoding without bom
		/// </summary>
		private static readonly UTF8Encoding UTF8EncodingWithoutBom = new UTF8Encoding(false);

		/// <summary>
		/// The write function cache
		/// </summary>
		private static Dictionary<Type, WriteObjectDelegate> WriteFnCache = new Dictionary<Type, WriteObjectDelegate>();

		/// <summary>
		/// Gets the write function.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>WriteObjectDelegate.</returns>
		internal static WriteObjectDelegate GetWriteFn(Type type)
        {
            try
            {
                WriteObjectDelegate writeFn;
                if (WriteFnCache.TryGetValue(type, out writeFn)) return writeFn;

                var genericType = typeof(CsvSerializer<>).MakeGenericType(type);
                var mi = genericType.GetPublicStaticMethod("WriteFn");
                var writeFactoryFn = (Func<WriteObjectDelegate>)mi.MakeDelegate(
                    typeof(Func<WriteObjectDelegate>));

                writeFn = writeFactoryFn();

                Dictionary<Type, WriteObjectDelegate> snapshot, newCache;
                do
                {
                    snapshot = WriteFnCache;
                    newCache = new Dictionary<Type, WriteObjectDelegate>(WriteFnCache);
                    newCache[type] = writeFn;

                } while (!ReferenceEquals(
                    Interlocked.CompareExchange(ref WriteFnCache, newCache, snapshot), snapshot));
                
                return writeFn;
            }
            catch (Exception ex)
            {
                Tracer.Instance.WriteError(ex);
                throw;
            }
        }

		/// <summary>
		/// Serializes to CSV.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="records">The records.</param>
		/// <returns>System.String.</returns>
		public static string SerializeToCsv<T>(IEnumerable<T> records)
        {
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb, CultureInfo.InvariantCulture))
            {
                writer.WriteCsv(records);
                return sb.ToString();
            }
        }

		/// <summary>
		/// Serializes to string.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns>System.String.</returns>
		public static string SerializeToString<T>(T value)
        {
            if (value == null) return null;
            if (typeof(T) == typeof(string)) return value as string;

            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb, CultureInfo.InvariantCulture))
            {
                CsvSerializer<T>.WriteObject(writer, value);
            }
            return sb.ToString();
        }

		/// <summary>
		/// Serializes to writer.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <param name="writer">The writer.</param>
		public static void SerializeToWriter<T>(T value, TextWriter writer)
        {
            if (value == null) return;
            if (typeof(T) == typeof(string))
            {
                writer.Write(value);
                return;
            }
            CsvSerializer<T>.WriteObject(writer, value);
        }

		/// <summary>
		/// Serializes to stream.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <param name="stream">The stream.</param>
		public static void SerializeToStream<T>(T value, Stream stream)
        {
            if (value == null) return;
            var writer = new StreamWriter(stream, UTF8EncodingWithoutBom);
            CsvSerializer<T>.WriteObject(writer, value);
            writer.Flush();
        }

		/// <summary>
		/// Serializes to stream.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="stream">The stream.</param>
		public static void SerializeToStream(object obj, Stream stream)
        {
            if (obj == null) return;
            var writer = new StreamWriter(stream, UTF8EncodingWithoutBom);
            var writeFn = GetWriteFn(obj.GetType());
            writeFn(writer, obj);
            writer.Flush();
        }

		/// <summary>
		/// Deserializes from stream.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream">The stream.</param>
		/// <returns>T.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public static T DeserializeFromStream<T>(Stream stream)
        {
            throw new NotImplementedException();
        }

		/// <summary>
		/// Deserializes from stream.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>System.Object.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public static object DeserializeFromStream(Type type, Stream stream)
        {
            throw new NotImplementedException();
        }

		/// <summary>
		/// Writes the late bound object.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="value">The value.</param>
		public static void WriteLateBoundObject(TextWriter writer, object value)
        {
            if (value == null) return;
            var writeFn = GetWriteFn(value.GetType());
            writeFn(writer, value);
        }
    }

	/// <summary>
	/// Class CsvSerializer.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal static class CsvSerializer<T>
    {
		/// <summary>
		/// The cache function
		/// </summary>
		private static readonly WriteObjectDelegate CacheFn;

		/// <summary>
		/// Writes the function.
		/// </summary>
		/// <returns>WriteObjectDelegate.</returns>
		public static WriteObjectDelegate WriteFn()
        {
            return CacheFn;
        }

		/// <summary>
		/// The ignore response status
		/// </summary>
		private const string IgnoreResponseStatus = "ResponseStatus";

		/// <summary>
		/// The value getter
		/// </summary>
		private static Func<object, object> valueGetter = null;
		/// <summary>
		/// The write element function
		/// </summary>
		private static WriteObjectDelegate writeElementFn = null;

		/// <summary>
		/// Gets the write function.
		/// </summary>
		/// <returns>WriteObjectDelegate.</returns>
		private static WriteObjectDelegate GetWriteFn()
        {
            PropertyInfo firstCandidate = null;
            Type bestCandidateEnumerableType = null;
            PropertyInfo bestCandidate = null;

            if (typeof(T).IsValueType())
            {
                return JsvWriter<T>.WriteObject;
            }

            //If type is an enumerable property itself write that
            bestCandidateEnumerableType = typeof(T).GetTypeWithGenericTypeDefinitionOf(typeof(IEnumerable<>));
            if (bestCandidateEnumerableType != null)
            {
                var elementType = bestCandidateEnumerableType.GenericTypeArguments()[0];
                writeElementFn = CreateWriteFn(elementType);

                return WriteEnumerableType;
            }

            //Look for best candidate property if DTO
            if (typeof(T).IsDto() || typeof(T).HasAttr<CsvAttribute>())
            {
                var properties = TypeConfig<T>.Properties;
                foreach (var propertyInfo in properties)
                {
                    if (propertyInfo.Name == IgnoreResponseStatus) continue;

                    if (propertyInfo.PropertyType == typeof(string)
                        || propertyInfo.PropertyType.IsValueType()
                        || propertyInfo.PropertyType == typeof(byte[])) continue;

                    if (firstCandidate == null)
                    {
                        firstCandidate = propertyInfo;
                    }

                    var enumProperty = propertyInfo.PropertyType
                        .GetTypeWithGenericTypeDefinitionOf(typeof(IEnumerable<>));

                    if (enumProperty != null)
                    {
                        bestCandidateEnumerableType = enumProperty;
                        bestCandidate = propertyInfo;
                        break;
                    }
                }
            }

            //If is not DTO or no candidates exist, write self
            var noCandidatesExist = bestCandidate == null && firstCandidate == null;
            if (noCandidatesExist)
            {
                return WriteSelf;
            }

            //If is DTO and has an enumerable property serialize that
            if (bestCandidateEnumerableType != null)
            {
                valueGetter = bestCandidate.GetValueGetter(typeof(T));
                var elementType = bestCandidateEnumerableType.GenericTypeArguments()[0];
                writeElementFn = CreateWriteFn(elementType);

                return WriteEnumerableProperty;
            }

            //If is DTO and has non-enumerable, reference type property serialize that
            valueGetter = firstCandidate.GetValueGetter(typeof(T));
            writeElementFn = CreateWriteRowFn(firstCandidate.PropertyType);

            return WriteNonEnumerableType;
        }

		/// <summary>
		/// Creates the write function.
		/// </summary>
		/// <param name="elementType">Type of the element.</param>
		/// <returns>WriteObjectDelegate.</returns>
		private static WriteObjectDelegate CreateWriteFn(Type elementType)
        {
            return CreateCsvWriterFn(elementType, "WriteObject");
        }

		/// <summary>
		/// Creates the write row function.
		/// </summary>
		/// <param name="elementType">Type of the element.</param>
		/// <returns>WriteObjectDelegate.</returns>
		private static WriteObjectDelegate CreateWriteRowFn(Type elementType)
        {
            return CreateCsvWriterFn(elementType, "WriteObjectRow");
        }

		/// <summary>
		/// Creates the CSV writer function.
		/// </summary>
		/// <param name="elementType">Type of the element.</param>
		/// <param name="methodName">Name of the method.</param>
		/// <returns>WriteObjectDelegate.</returns>
		private static WriteObjectDelegate CreateCsvWriterFn(Type elementType, string methodName)
        {
            var genericType = typeof(CsvWriter<>).MakeGenericType(elementType);
            var mi = genericType.GetPublicStaticMethod(methodName);
            var writeFn = (WriteObjectDelegate)mi.MakeDelegate(typeof(WriteObjectDelegate));
            return writeFn;
        }

		/// <summary>
		/// Writes the type of the enumerable.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="obj">The object.</param>
		public static void WriteEnumerableType(TextWriter writer, object obj)
        {
            writeElementFn(writer, obj);
        }

		/// <summary>
		/// Writes the self.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="obj">The object.</param>
		public static void WriteSelf(TextWriter writer, object obj)
        {
            CsvWriter<T>.WriteRow(writer, (T)obj);
        }

		/// <summary>
		/// Writes the enumerable property.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="obj">The object.</param>
		public static void WriteEnumerableProperty(TextWriter writer, object obj)
        {
            if (obj == null) return; //AOT

            var enumerableProperty = valueGetter(obj);
            writeElementFn(writer, enumerableProperty);
        }

		/// <summary>
		/// Writes the type of the non enumerable.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="obj">The object.</param>
		public static void WriteNonEnumerableType(TextWriter writer, object obj)
        {
            var nonEnumerableType = valueGetter(obj);
            writeElementFn(writer, nonEnumerableType);
        }

		/// <summary>
		/// Initializes static members of the <see cref="CsvSerializer{T}"/> class.
		/// </summary>
		static CsvSerializer()
        {
            if (typeof(T) == typeof(object))
            {
                CacheFn = CsvSerializer.WriteLateBoundObject;
            }
            else
            {
                CacheFn = GetWriteFn();
            }
        }

		/// <summary>
		/// Writes the object.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="value">The value.</param>
		public static void WriteObject(TextWriter writer, object value)
        {
            CacheFn(writer, value);
        }
    }
}