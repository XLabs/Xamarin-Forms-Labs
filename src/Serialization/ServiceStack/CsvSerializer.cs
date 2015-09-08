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
    public class CsvSerializer
    {
        private static readonly UTF8Encoding UTF8EncodingWithoutBom = new UTF8Encoding(false);

        private static Dictionary<Type, WriteObjectDelegate> WriteFnCache = new Dictionary<Type, WriteObjectDelegate>();

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

        public static string SerializeToCsv<T>(IEnumerable<T> records)
        {
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb, CultureInfo.InvariantCulture))
            {
                writer.WriteCsv(records);
                return sb.ToString();
            }
        }

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

        public static void SerializeToStream<T>(T value, Stream stream)
        {
            if (value == null) return;
            var writer = new StreamWriter(stream, UTF8EncodingWithoutBom);
            CsvSerializer<T>.WriteObject(writer, value);
            writer.Flush();
        }

        public static void SerializeToStream(object obj, Stream stream)
        {
            if (obj == null) return;
            var writer = new StreamWriter(stream, UTF8EncodingWithoutBom);
            var writeFn = GetWriteFn(obj.GetType());
            writeFn(writer, obj);
            writer.Flush();
        }

        public static T DeserializeFromStream<T>(Stream stream)
        {
            throw new NotImplementedException();
        }

        public static object DeserializeFromStream(Type type, Stream stream)
        {
            throw new NotImplementedException();
        }

        public static void WriteLateBoundObject(TextWriter writer, object value)
        {
            if (value == null) return;
            var writeFn = GetWriteFn(value.GetType());
            writeFn(writer, value);
        }
    }

    internal static class CsvSerializer<T>
    {
        private static readonly WriteObjectDelegate CacheFn;

        public static WriteObjectDelegate WriteFn()
        {
            return CacheFn;
        }

        private const string IgnoreResponseStatus = "ResponseStatus";

        private static Func<object, object> valueGetter = null;
        private static WriteObjectDelegate writeElementFn = null;

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

        private static WriteObjectDelegate CreateWriteFn(Type elementType)
        {
            return CreateCsvWriterFn(elementType, "WriteObject");
        }

        private static WriteObjectDelegate CreateWriteRowFn(Type elementType)
        {
            return CreateCsvWriterFn(elementType, "WriteObjectRow");
        }

        private static WriteObjectDelegate CreateCsvWriterFn(Type elementType, string methodName)
        {
            var genericType = typeof(CsvWriter<>).MakeGenericType(elementType);
            var mi = genericType.GetPublicStaticMethod(methodName);
            var writeFn = (WriteObjectDelegate)mi.MakeDelegate(typeof(WriteObjectDelegate));
            return writeFn;
        }

        public static void WriteEnumerableType(TextWriter writer, object obj)
        {
            writeElementFn(writer, obj);
        }

        public static void WriteSelf(TextWriter writer, object obj)
        {
            CsvWriter<T>.WriteRow(writer, (T)obj);
        }

        public static void WriteEnumerableProperty(TextWriter writer, object obj)
        {
            if (obj == null) return; //AOT

            var enumerableProperty = valueGetter(obj);
            writeElementFn(writer, enumerableProperty);
        }

        public static void WriteNonEnumerableType(TextWriter writer, object obj)
        {
            var nonEnumerableType = valueGetter(obj);
            writeElementFn(writer, nonEnumerableType);
        }

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

        public static void WriteObject(TextWriter writer, object value)
        {
            CacheFn(writer, value);
        }
    }
}