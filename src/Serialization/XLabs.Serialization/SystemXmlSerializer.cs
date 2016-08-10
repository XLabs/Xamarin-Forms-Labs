// ***********************************************************************
// Assembly         : XLabs.Serialization
// Author           : rmarinho
// Created          : 09-08-2015
//
// Last Modified By : rmarinho
// Last Modified On : 09-08-2015
// ***********************************************************************
// <copyright file="SystemXmlSerializer.cs" company="">
//     Copyright © XLabs 2014
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace XLabs.Serialization
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;

	/// <summary>
	/// Class SystemXmlSerializer.
	/// </summary>
	public class SystemXmlSerializer : StreamSerializer, IXmlSerializer
    {
		/// <summary>
		/// Gets the serialization format.
		/// </summary>
		/// <value>Serialization format.</value>
		public override SerializationFormat Format
        {
            get { return SerializationFormat.Xml; }
        }

		/// <summary>
		/// Cleans memory.
		/// </summary>
		public override void Flush()
        {

        }

		/// <summary>
		/// Serializes object to a stream
		/// </summary>
		/// <typeparam name="T">Type of object to serialize.</typeparam>
		/// <param name="obj">Object to serialize</param>
		/// <param name="stream">Stream to serialize to</param>
		public override void Serialize<T>(T obj, Stream stream)
        {
            var serializer = new DataContractSerializer(obj.GetType());
            serializer.WriteObject(stream, obj);
        }

		/// <summary>
		/// Deserializes stream into an object
		/// </summary>
		/// <typeparam name="T">Type of object to serialize to</typeparam>
		/// <param name="stream">Stream to deserialize from</param>
		/// <returns>Object of type T</returns>
		public override T Deserialize<T>(Stream stream)
        {
            return (T) this.Deserialize(stream, typeof (T));
        }

		/// <summary>
		/// Deserializes stream into an object.
		/// </summary>
		/// <param name="stream">Stream to deserialize from.</param>
		/// <param name="type">Type of object to deserialize.</param>
		/// <returns>Deserialized object.</returns>
		public override object Deserialize(Stream stream, Type type)
        {
            var serializer = new DataContractSerializer(type);
            return serializer.ReadObject(stream);
        }

		/// <summary>
		/// Deserializes string into an object.
		/// </summary>
		/// <typeparam name="T">Type of object to serialize to.</typeparam>
		/// <param name="data">Serialized object.</param>
		/// <returns>Object of type T.</returns>
		public override T Deserialize<T>(string data)
        {
            return this.DeserializeFromString<T>(data, Encoding.UTF8);
        }

		/// <summary>
		/// Serializes object to a string.
		/// </summary>
		/// <typeparam name="T">Type of object to serialize.</typeparam>
		/// <param name="obj">Object to serialize.</param>
		/// <returns>Serialized string of the object.</returns>
		public override string Serialize<T>(T obj)
        {
            return this.SerializeToString(obj, Encoding.UTF8);
        }
    }
}
//#endif