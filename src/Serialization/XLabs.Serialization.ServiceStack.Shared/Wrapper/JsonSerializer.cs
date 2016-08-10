// ***********************************************************************
// <copyright file="JsonSerializer.cs" company="XLabs">
//     Copyright © ServiceStack 2013 & XLabs
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Diagnostics.CodeAnalysis;

namespace XLabs.Serialization.ServiceStack
{
    using global::ServiceStack.Text;

    using JsConfig = global::ServiceStack.Text.JsConfig;
    using JsonDateHandler = global::ServiceStack.Text.JsonDateHandler;
    using Serializer = global::ServiceStack.Text.JsonSerializer;

	/// <summary>
	/// JSON serializer using ServiceStack.Text library.
	/// </summary>
	/// <remarks>ServiceStack.Text copyright information.
	/// Copyright (c) 2007-2011, Demis Bellot, ServiceStack.
	/// http://www.servicestack.net
	/// All rights reserved.
	/// https://github.com/ServiceStack/ServiceStack.Text/blob/master/LICENSE</remarks>
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1629:DocumentationTextMustEndWithAPeriod", Justification = "Reviewed. Suppression is OK here."), SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1644:DocumentationHeadersMustNotContainBlankLines", Justification = "Reviewed. Suppression is OK here."), SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class JsonSerializer : StringSerializer, IJsonSerializer
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="JsonSerializer" /> class.
		/// </summary>
		public JsonSerializer()
        {
            JsConfig.EmitCamelCaseNames = true;
            JsConfig.DateHandler = JsonDateHandler.ISO8601;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonSerializer" /> class.
		/// </summary>
		/// <param name="emitCamelCaseNames">Emit camelcase names</param>
		/// <param name="accurateDateTime">Set to true if you need DateTime's to not lose accuracy</param>
		public JsonSerializer(bool emitCamelCaseNames, bool accurateDateTime)
        {
            JsConfig.EmitCamelCaseNames = emitCamelCaseNames;

            if (accurateDateTime)
            {
                JsConfig.DateHandler = JsonDateHandler.ISO8601;
            }
        }

		/// <summary>
		/// Sets the serialize delegate.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="serializerDelegate">The serializer delegate.</param>
		public static void SetSerializeDelegate<T>(System.Func<T,string> serializerDelegate)
        {
            JsConfig<T>.SerializeFn = serializerDelegate;
        }

		/// <summary>
		/// Sets the deserialize delegate.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="deserializerDelegate">The deserializer delegate.</param>
		public static void SetDeserializeDelegate<T>(System.Func<string, T> deserializerDelegate)
        {
            JsConfig<T>.DeSerializeFn = deserializerDelegate;
        }

		#region ISerializer Members

		/// <summary>
		/// Gets the format.
		/// </summary>
		/// <value>The format.</value>
		public override SerializationFormat Format
        {
            get { return SerializationFormat.Json; }
        }

		/// <summary>
		/// Flushes this instance.
		/// </summary>
		public override void Flush()
        {
            JsConfig.Reset();
        }

		#endregion

		#region IStringSerializer Members

		/// <summary>
		/// Serializes the specified object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The object.</param>
		/// <returns>System.String.</returns>
		public override string Serialize<T>(T obj)
        {
            return Serializer.SerializeToString(obj);
        }

		/// <summary>
		/// Deserializes the specified data.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data">The data.</param>
		/// <returns>T.</returns>
		public override T Deserialize<T>(string data)
        {
            return Serializer.DeserializeFromString<T>(data);
        }

		/// <summary>
		/// Deserializes the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="type">The type.</param>
		/// <returns>System.Object.</returns>
		public override object Deserialize(string data, System.Type type)
        {
            return Serializer.DeserializeFromString(data, type);
        }

        #endregion
    }
}
