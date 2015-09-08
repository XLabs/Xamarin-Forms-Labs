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
using System.Reflection;
using System.Linq;
using ServiceStack.Text.Jsv;

namespace ServiceStack.Text.Common
{
    internal delegate object ParseDelegate(string value);

    internal static class ParseMethodUtilities
    {
        public static ParseStringDelegate GetParseFn<T>(string parseMethod)
        {
            // Get the static Parse(string) method on the type supplied
            var parseMethodInfo = typeof(T).GetPublicStaticMethod(parseMethod, new[] { typeof(string) });
            if (parseMethodInfo == null) 
                return null;

            ParseDelegate parseDelegate = null;
            try
            {
                if (parseMethodInfo.ReturnType != typeof(T))
                {
                    parseDelegate = (ParseDelegate)parseMethodInfo.MakeDelegate(typeof(ParseDelegate), false);
                }
                if (parseDelegate == null)
                {
                    //Try wrapping strongly-typed return with wrapper fn.
                    var typedParseDelegate = (Func<string, T>)parseMethodInfo.MakeDelegate(typeof(Func<string, T>));
                    parseDelegate = x => typedParseDelegate(x);
                }
            }
            catch (ArgumentException)
            {
                Tracer.Instance.WriteDebug("Nonstandard Parse method on type {0}", typeof(T));
            }

            if (parseDelegate != null)
                return value => parseDelegate(value.FromCsvField());

            return null;
        }
    }

    public static class StaticParseMethod<T>
    {
        const string ParseMethod = "Parse";

        private static readonly ParseStringDelegate CacheFn;

        public static ParseStringDelegate Parse
        {
            get { return CacheFn; }
        }

        static StaticParseMethod()
        {
            CacheFn = ParseMethodUtilities.GetParseFn<T>(ParseMethod);
        }

    }

    internal static class StaticParseRefTypeMethod<TSerializer, T>
        where TSerializer : ITypeSerializer
    {
        static readonly string ParseMethod = typeof(TSerializer) == typeof(JsvTypeSerializer)
            ? "ParseJsv"
            : "ParseJson";

        private static readonly ParseStringDelegate CacheFn;

        public static ParseStringDelegate Parse
        {
            get { return CacheFn; }
        }

        static StaticParseRefTypeMethod()
        {
            CacheFn = ParseMethodUtilities.GetParseFn<T>(ParseMethod);
        }
    }

}