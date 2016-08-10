// ***********************************************************************
// Assembly         : XLabs.Serialization
// Author           : rmarinho
// Created          : 09-08-2015
//
// Last Modified By : rmarinho
// Last Modified On : 09-08-2015
// ***********************************************************************
// <copyright file="StringSerializer.cs" company="">
//     Copyright © XLabs 2014
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Diagnostics.CodeAnalysis;

namespace XLabs.Serialization
{
	/// <summary>
	/// The string serializer base class.
	/// </summary>
	[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1124:DoNotUseRegions", Justification = "Reviewed. Suppression is OK here.")]
    public abstract class StringSerializer : ISerializer
    {
		#region ISerializer Members
		/// <summary>
		/// Gets the serialization format.
		/// </summary>
		/// <value>Serialization format.</value>
		public abstract SerializationFormat Format { get; }

		/// <summary>
		/// Cleans memory.
		/// </summary>
		public abstract void Flush();
		#endregion

		#region IByteSerializer Members

		/// <summary>
		/// Serializes object to a byte array.
		/// </summary>
		/// <typeparam name="T">Type of object to serialize to.</typeparam>
		/// <param name="obj">Object to serialize.</param>
		/// <returns>Serialized byte[] of the object.</returns>
		public byte[] SerializeToBytes<T>(T obj)
        {
            return (this as IStringSerializer).GetSerializedBytes(obj);
        }

		/// <summary>
		/// Deserializes byte array into an object.
		/// </summary>
		/// <typeparam name="T">Type of object to serialize to.</typeparam>
		/// <param name="data">Serialized object as byte buffer.</param>
		/// <returns>Object of type T.</returns>
		public T Deserialize<T>(byte[] data)
        {
            return (this as IStringSerializer).DeserializeFromBytes<T>(data);
        }

		/// <summary>
		/// Deserializes byte array into an object.
		/// </summary>
		/// <param name="data">Serialized object as byte buffer.</param>
		/// <param name="type">Type of object to deserialize.</param>
		/// <returns>Deserialized object.</returns>
		public object Deserialize(byte[] data, System.Type type)
        {
            return (this as IStringSerializer).DeserializeFromBytes(data, type);
        }
		#endregion

		#region IStreamSerializer Members
		/// <summary>
		/// Serializes object to a stream.
		/// </summary>
		/// <typeparam name="T">Type of object to serialize.</typeparam>
		/// <param name="obj">Object to serialize.</param>
		/// <param name="stream">Stream to serialize to.</param>
		public void Serialize<T>(T obj, System.IO.Stream stream)
        {
            this.SerializeToStream(obj, stream);
        }

		/// <summary>
		/// Deserializes stream into an object.
		/// </summary>
		/// <typeparam name="T">Type of object to serialize to.</typeparam>
		/// <param name="stream">Stream to deserialize from.</param>
		/// <returns>Object of type T.</returns>
		public T Deserialize<T>(System.IO.Stream stream)
        {
            return this.DeserializeFromStream<T>(stream);
        }

		/// <summary>
		/// Deserializes stream into an object.
		/// </summary>
		/// <param name="stream">Stream to deserialize from.</param>
		/// <param name="type">Type of object to deserialize.</param>
		/// <returns>Deserialized object.</returns>
		public object Deserialize(System.IO.Stream stream, System.Type type)
        {
            return this.DeserializeFromStream(stream, type);
        }
		#endregion

		#region IStringSerializer Members
		/// <summary>
		/// Serializes object to a string.
		/// </summary>
		/// <typeparam name="T">Type of object to serialize.</typeparam>
		/// <param name="obj">Object to serialize.</param>
		/// <returns>Serialized string of the object.</returns>
		public abstract string Serialize<T>(T obj);

		/// <summary>
		/// Deserializes string into an object.
		/// </summary>
		/// <typeparam name="T">Type of object to serialize to.</typeparam>
		/// <param name="data">Serialized object.</param>
		/// <returns>Object of type T.</returns>
		public abstract T Deserialize<T>(string data);

		/// <summary>
		/// Deserializes string into an object.
		/// </summary>
		/// <param name="data">Serialized object.</param>
		/// <param name="type">Type of object to deserialize.</param>
		/// <returns>Object of type T.</returns>
		public abstract object Deserialize(string data, System.Type type);
        #endregion

        #region IStreamSerializer Members




        #endregion

        #region IStringSerializer Members




        #endregion
    }
}
