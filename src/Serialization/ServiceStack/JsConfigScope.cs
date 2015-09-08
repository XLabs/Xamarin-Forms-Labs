using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace ServiceStack.Text
{
    public sealed class JsConfigScope : IDisposable
    {
        bool disposed;
        JsConfigScope parent;

        [ThreadStatic]
        private static JsConfigScope head;

        internal JsConfigScope()
        {
#if !SILVERLIGHT
            Thread.BeginThreadAffinity();
#endif
            parent = head;
            head = this;
        }

        internal static JsConfigScope Current
        {
            get
            {
                return head;
            }
        }

        public static void DisposeCurrent()
        {
            if (head != null)
            {
                head.Dispose();
            }
        }

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

        public bool? ConvertObjectTypesIntoStringDictionary { get; set; }
        public bool? TryToParsePrimitiveTypeValues { get; set; }
		public bool? TryToParseNumericType { get; set; }
        public bool? IncludeNullValues { get; set; }
        public bool? TreatEnumAsInteger { get; set; }
        public bool? ExcludeTypeInfo { get; set; }
        public bool? IncludeTypeInfo { get; set; }
        public string TypeAttr { get; set; }
        internal string JsonTypeAttrInObject { get; set; }
        internal string JsvTypeAttrInObject { get; set; }
        public Func<Type, string> TypeWriter { get; set; }
        public Func<string, Type> TypeFinder { get; set; }
        public JsonDateHandler? DateHandler { get; set; }
        public JsonTimeSpanHandler? TimeSpanHandler { get; set; }
        public bool? EmitCamelCaseNames { get; set; }
        public bool? EmitLowercaseUnderscoreNames { get; set; }
        public bool? ThrowOnDeserializationError { get; set; }
        public bool? AlwaysUseUtc { get; set; }
        public bool? AssumeUtc { get; set; }
        public bool? AppendUtcOffset { get; set; }
        public bool? EscapeUnicode { get; set; }
        public bool? PreferInterfaces { get; set; }
        public bool? IncludePublicFields { get; set; }
        public int? MaxDepth { get; set; }
        public EmptyCtorFactoryDelegate ModelFactory { get; set; }
        public string[] ExcludePropertyReferences { get; set; }
    }
}
