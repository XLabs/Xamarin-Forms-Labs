using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace ServiceStack.Text.Common
{
    internal static class DeserializeSpecializedCollections<T, TSerializer>
        where TSerializer : ITypeSerializer
    {
        private readonly static ParseStringDelegate CacheFn;

        static DeserializeSpecializedCollections()
        {
            CacheFn = GetParseFn();
        }

        public static ParseStringDelegate Parse
        {
            get { return CacheFn; }
        }

        public static ParseStringDelegate GetParseFn()
        {
            if (typeof(T).HasAnyTypeDefinitionsOf(typeof(Queue<>)))
            {
                if (typeof(T) == typeof(Queue<string>))
                    return ParseStringQueue;

                if (typeof(T) == typeof(Queue<int>))
                    return ParseIntQueue;

                return GetGenericQueueParseFn();
            }

            if (typeof(T).HasAnyTypeDefinitionsOf(typeof(Stack<>)))
            {
                if (typeof(T) == typeof(Stack<string>))
                    return ParseStringStack;

                if (typeof(T) == typeof(Stack<int>))
                    return ParseIntStack;

                return GetGenericStackParseFn();
            }

#if !SILVERLIGHT
            if (typeof(T) == typeof(StringCollection))
            {
                return ParseStringCollection<TSerializer>;
            }
#endif
            if (typeof (T) == typeof (IEnumerable) || typeof(T) == typeof(ICollection))
            {
                return GetEnumerableParseFn();
            }

            return GetGenericEnumerableParseFn();
        }

        public static Queue<string> ParseStringQueue(string value)
        {
            var parse = (IEnumerable<string>)DeserializeList<List<string>, TSerializer>.Parse(value);
            return new Queue<string>(parse);
        }

        public static Queue<int> ParseIntQueue(string value)
        {
            var parse = (IEnumerable<int>)DeserializeList<List<int>, TSerializer>.Parse(value);
            return new Queue<int>(parse);
        }

#if !SILVERLIGHT
        public static StringCollection ParseStringCollection<TS>(string value) where TS : ITypeSerializer
        {
            if ((value = DeserializeListWithElements<TS>.StripList(value)) == null) return null;
            return value == String.Empty
                   ? new StringCollection()
                   : ToStringCollection(DeserializeListWithElements<TSerializer>.ParseStringList(value));
        }

        public static StringCollection ToStringCollection(List<string> items)
        {
            var to = new StringCollection();
            foreach (var item in items)
            {
                to.Add(item);
            }
            return to;
        }
#endif

        internal static ParseStringDelegate GetGenericQueueParseFn()
        {
            var enumerableInterface = typeof(T).GetTypeWithGenericInterfaceOf(typeof(IEnumerable<>));
            var elementType = enumerableInterface.GenericTypeArguments()[0];
            var genericType = typeof(SpecializedQueueElements<>).MakeGenericType(elementType);
            var mi = genericType.GetPublicStaticMethod("ConvertToQueue");
            var convertToQueue = (ConvertObjectDelegate)mi.MakeDelegate(typeof(ConvertObjectDelegate));

            var parseFn = DeserializeEnumerable<T, TSerializer>.GetParseFn();

            return x => convertToQueue(parseFn(x));
        }

        public static Stack<string> ParseStringStack(string value)
        {
            var parse = (IEnumerable<string>)DeserializeList<List<string>, TSerializer>.Parse(value);
            return new Stack<string>(parse);
        }

        public static Stack<int> ParseIntStack(string value)
        {
            var parse = (IEnumerable<int>)DeserializeList<List<int>, TSerializer>.Parse(value);
            return new Stack<int>(parse);
        }

        internal static ParseStringDelegate GetGenericStackParseFn()
        {
            var enumerableInterface = typeof(T).GetTypeWithGenericInterfaceOf(typeof(IEnumerable<>));
            
            var elementType = enumerableInterface.GenericTypeArguments()[0];
            var genericType = typeof(SpecializedQueueElements<>).MakeGenericType(elementType);
            var mi = genericType.GetPublicStaticMethod("ConvertToStack");
            var convertToQueue = (ConvertObjectDelegate)mi.MakeDelegate(typeof(ConvertObjectDelegate));

            var parseFn = DeserializeEnumerable<T, TSerializer>.GetParseFn();

            return x => convertToQueue(parseFn(x));
        }

        public static ParseStringDelegate GetEnumerableParseFn()
        {
            return DeserializeListWithElements<TSerializer>.ParseStringList;
        }

        public static ParseStringDelegate GetGenericEnumerableParseFn()
        {
            var enumerableInterface = typeof(T).GetTypeWithGenericInterfaceOf(typeof(IEnumerable<>));
            if (enumerableInterface == null) return null; 
            var elementType = enumerableInterface.GenericTypeArguments()[0];
            var genericType = typeof(SpecializedEnumerableElements<,>).MakeGenericType(typeof(T), elementType);
            var fi = genericType.GetPublicStaticField("ConvertFn");

            var convertFn = fi.GetValue(null) as ConvertObjectDelegate;
            if (convertFn == null) return null;

            var parseFn = DeserializeEnumerable<T, TSerializer>.GetParseFn();

            return x => convertFn(parseFn(x));
        }
    }

    internal class SpecializedQueueElements<T>
    {
        public static Queue<T> ConvertToQueue(object enumerable)
        {
            if (enumerable == null) return null;
            return new Queue<T>((IEnumerable<T>)enumerable);
        }

        public static Stack<T> ConvertToStack(object enumerable)
        {
            if (enumerable == null) return null;
            return new Stack<T>((IEnumerable<T>)enumerable);
        }
    }

    internal class SpecializedEnumerableElements<TCollection, T>
    {
        public static ConvertObjectDelegate ConvertFn;

        static SpecializedEnumerableElements()
        {
            foreach (var ctorInfo in typeof(TCollection).DeclaredConstructors())
            {
                var ctorParams = ctorInfo.GetParameters();
                if (ctorParams.Length != 1) continue;
                var ctorParam = ctorParams[0];

                if (typeof(IEnumerable).AssignableFrom(ctorParam.ParameterType)
                    || ctorParam.ParameterType.IsOrHasGenericInterfaceTypeOf(typeof(IEnumerable<>)))
                {
                    ConvertFn = fromObject =>
                    {
                        var to = Activator.CreateInstance(typeof(TCollection), fromObject);
                        return to;
                    };
                    return;
                }
            }

            if (typeof(TCollection).IsOrHasGenericInterfaceTypeOf(typeof(ICollection<>)))
            {
                ConvertFn = ConvertFromCollection;
            }
        }

        public static object Convert(object enumerable)
        {
            return ConvertFn(enumerable);
        }

        public static object ConvertFromCollection(object enumerable)
        {
            var to = (ICollection<T>)typeof(TCollection).CreateInstance();
            var from = (IEnumerable<T>)enumerable;
            foreach (var item in from)
            {
                to.Add(item);
            }
            return to;
        }
    }
}
