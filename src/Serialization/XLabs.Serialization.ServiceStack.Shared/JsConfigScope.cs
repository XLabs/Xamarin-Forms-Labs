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
using System.Text;
using System.Threading;
using System.Diagnostics;


namespace ServiceStack.Text
{
	/// <summary>
	/// Class JsConfigScope. This class cannot be inherited.
	/// </summary>
	public sealed class JsConfigScope : IDisposable
    {
		/// <summary>
		/// The disposed
		/// </summary>
		bool disposed;
		/// <summary>
		/// The parent
		/// </summary>
		JsConfigScope parent;

		/// <summary>
		/// The head
		/// </summary>
		[ThreadStatic]
        private static JsConfigScope head;

		/// <summary>
		/// Initializes a new instance of the <see cref="JsConfigScope"/> class.
		/// </summary>
		internal JsConfigScope()
        {
#if !SILVERLIGHT
            Thread.BeginThreadAffinity();
#endif
            parent = head;
            head = this;
        }

		/// <summary>
		/// Gets the current.
		/// </summary>
		/// <value>The current.</value>
		internal static JsConfigScope Current
        {
            get
            {
                return head;
            }
        }

		/// <summary>
		/// Disposes the current.
		/// </summary>
		public static void DisposeCurrent()
        {
            if (head != null)
            {
                head.Dispose();
            }
        }

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;

                Debug.Assert(this == head, "Disposed out of order.");

                head = parent;
#if !SILVERLIGHT
                Thread.EndThreadAffinity();
#endif
            }
        }

		/// <summary>
		/// Gets or sets a value indicating whether [convert object types into string dictionary].
		/// </summary>
		/// <value><c>null</c> if [convert object types into string dictionary] contains no value, <c>true</c> if [convert object types into string dictionary]; otherwise, <c>false</c>.</value>
		public bool? ConvertObjectTypesIntoStringDictionary { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [try to parse primitive type values].
		/// </summary>
		/// <value><c>null</c> if [try to parse primitive type values] contains no value, <c>true</c> if [try to parse primitive type values]; otherwise, <c>false</c>.</value>
		public bool? TryToParsePrimitiveTypeValues { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [try to parse numeric type].
		/// </summary>
		/// <value><c>null</c> if [try to parse numeric type] contains no value, <c>true</c> if [try to parse numeric type]; otherwise, <c>false</c>.</value>
		public bool? TryToParseNumericType { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [include null values].
		/// </summary>
		/// <value><c>null</c> if [include null values] contains no value, <c>true</c> if [include null values]; otherwise, <c>false</c>.</value>
		public bool? IncludeNullValues { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [treat enum as integer].
		/// </summary>
		/// <value><c>null</c> if [treat enum as integer] contains no value, <c>true</c> if [treat enum as integer]; otherwise, <c>false</c>.</value>
		public bool? TreatEnumAsInteger { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [exclude type information].
		/// </summary>
		/// <value><c>null</c> if [exclude type information] contains no value, <c>true</c> if [exclude type information]; otherwise, <c>false</c>.</value>
		public bool? ExcludeTypeInfo { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [include type information].
		/// </summary>
		/// <value><c>null</c> if [include type information] contains no value, <c>true</c> if [include type information]; otherwise, <c>false</c>.</value>
		public bool? IncludeTypeInfo { get; set; }
		/// <summary>
		/// Gets or sets the type attribute.
		/// </summary>
		/// <value>The type attribute.</value>
		public string TypeAttr { get; set; }
		/// <summary>
		/// Gets or sets the json type attribute in object.
		/// </summary>
		/// <value>The json type attribute in object.</value>
		internal string JsonTypeAttrInObject { get; set; }
		/// <summary>
		/// Gets or sets the JSV type attribute in object.
		/// </summary>
		/// <value>The JSV type attribute in object.</value>
		internal string JsvTypeAttrInObject { get; set; }
		/// <summary>
		/// Gets or sets the type writer.
		/// </summary>
		/// <value>The type writer.</value>
		public Func<Type, string> TypeWriter { get; set; }
		/// <summary>
		/// Gets or sets the type finder.
		/// </summary>
		/// <value>The type finder.</value>
		public Func<string, Type> TypeFinder { get; set; }
		/// <summary>
		/// Gets or sets the date handler.
		/// </summary>
		/// <value>The date handler.</value>
		public JsonDateHandler? DateHandler { get; set; }
		/// <summary>
		/// Gets or sets the time span handler.
		/// </summary>
		/// <value>The time span handler.</value>
		public JsonTimeSpanHandler? TimeSpanHandler { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [emit camel case names].
		/// </summary>
		/// <value><c>null</c> if [emit camel case names] contains no value, <c>true</c> if [emit camel case names]; otherwise, <c>false</c>.</value>
		public bool? EmitCamelCaseNames { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [emit lowercase underscore names].
		/// </summary>
		/// <value><c>null</c> if [emit lowercase underscore names] contains no value, <c>true</c> if [emit lowercase underscore names]; otherwise, <c>false</c>.</value>
		public bool? EmitLowercaseUnderscoreNames { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [throw on deserialization error].
		/// </summary>
		/// <value><c>null</c> if [throw on deserialization error] contains no value, <c>true</c> if [throw on deserialization error]; otherwise, <c>false</c>.</value>
		public bool? ThrowOnDeserializationError { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [always use UTC].
		/// </summary>
		/// <value><c>null</c> if [always use UTC] contains no value, <c>true</c> if [always use UTC]; otherwise, <c>false</c>.</value>
		public bool? AlwaysUseUtc { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [assume UTC].
		/// </summary>
		/// <value><c>null</c> if [assume UTC] contains no value, <c>true</c> if [assume UTC]; otherwise, <c>false</c>.</value>
		public bool? AssumeUtc { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [append UTC offset].
		/// </summary>
		/// <value><c>null</c> if [append UTC offset] contains no value, <c>true</c> if [append UTC offset]; otherwise, <c>false</c>.</value>
		public bool? AppendUtcOffset { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [escape unicode].
		/// </summary>
		/// <value><c>null</c> if [escape unicode] contains no value, <c>true</c> if [escape unicode]; otherwise, <c>false</c>.</value>
		public bool? EscapeUnicode { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [prefer interfaces].
		/// </summary>
		/// <value><c>null</c> if [prefer interfaces] contains no value, <c>true</c> if [prefer interfaces]; otherwise, <c>false</c>.</value>
		public bool? PreferInterfaces { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [include public fields].
		/// </summary>
		/// <value><c>null</c> if [include public fields] contains no value, <c>true</c> if [include public fields]; otherwise, <c>false</c>.</value>
		public bool? IncludePublicFields { get; set; }
		/// <summary>
		/// Gets or sets the maximum depth.
		/// </summary>
		/// <value>The maximum depth.</value>
		public int? MaxDepth { get; set; }
		/// <summary>
		/// Gets or sets the model factory.
		/// </summary>
		/// <value>The model factory.</value>
		public EmptyCtorFactoryDelegate ModelFactory { get; set; }
		/// <summary>
		/// Gets or sets the exclude property references.
		/// </summary>
		/// <value>The exclude property references.</value>
		public string[] ExcludePropertyReferences { get; set; }
    }
}
