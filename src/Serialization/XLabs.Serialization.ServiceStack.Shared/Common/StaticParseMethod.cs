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
	/// <summary>
	/// Delegate ParseDelegate
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.Object.</returns>
	internal delegate object ParseDelegate(string value);

	/// <summary>
	/// Class ParseMethodUtilities.
	/// </summary>
	internal static class ParseMethodUtilities
    {
		/// <summary>
		/// Gets the parse function.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="parseMethod">The parse method.</param>
		/// <returns>ParseStringDelegate.</returns>
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

	/// <summary>
	/// Class StaticParseMethod.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class StaticParseMethod<T>
    {
		/// <summary>
		/// The parse method
		/// </summary>
		const string ParseMethod = "Parse";

		/// <summary>
		/// The cache function
		/// </summary>
		private static readonly ParseStringDelegate CacheFn;

		/// <summary>
		/// Gets the parse.
		/// </summary>
		/// <value>The parse.</value>
		public static ParseStringDelegate Parse
        {
            get { return CacheFn; }
        }

		/// <summary>
		/// Initializes static members of the <see cref="StaticParseMethod{T}"/> class.
		/// </summary>
		static StaticParseMethod()
        {
            CacheFn = ParseMethodUtilities.GetParseFn<T>(ParseMethod);
        }

    }

	/// <summary>
	/// Class StaticParseRefTypeMethod.
	/// </summary>
	/// <typeparam name="TSerializer">The type of the t serializer.</typeparam>
	/// <typeparam name="T"></typeparam>
	internal static class StaticParseRefTypeMethod<TSerializer, T>
        where TSerializer : ITypeSerializer
    {
		/// <summary>
		/// The parse method
		/// </summary>
		static readonly string ParseMethod = typeof(TSerializer) == typeof(JsvTypeSerializer)
            ? "ParseJsv"
            : "ParseJson";

		/// <summary>
		/// The cache function
		/// </summary>
		private static readonly ParseStringDelegate CacheFn;

		/// <summary>
		/// Gets the parse.
		/// </summary>
		/// <value>The parse.</value>
		public static ParseStringDelegate Parse
        {
            get { return CacheFn; }
        }

		/// <summary>
		/// Initializes static members of the <see cref="StaticParseRefTypeMethod{TSerializer, T}"/> class.
		/// </summary>
		static StaticParseRefTypeMethod()
        {
            CacheFn = ParseMethodUtilities.GetParseFn<T>(ParseMethod);
        }
    }

}