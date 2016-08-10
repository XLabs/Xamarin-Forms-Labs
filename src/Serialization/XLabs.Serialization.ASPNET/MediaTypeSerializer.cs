// ***********************************************************************
// <copyright file="MediaTypeSerializer.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace XLabs.Serialization.AspNet
{
	public class MediaTypeSerializer : MediaTypeFormatter
	{
		private readonly ISerializer _serializer;

		public MediaTypeSerializer(ISerializer serializer, string mediaType, IEnumerable<Encoding> supportedEncodings)
		{
			_serializer = serializer;
			SupportedMediaTypes.Add(new MediaTypeHeaderValue(mediaType));

			foreach (var supportedEncoding in supportedEncodings)
			{
				SupportedEncodings.Add(supportedEncoding);
			}
		}

		/// <summary>
		///     Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can deserialize an object of
		///     the specified type.
		/// </summary>
		/// <returns>
		///     true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can deserialize the type; otherwise,
		///     false.
		/// </returns>
		/// <param name="type">The type to deserialize.</param>
		public override bool CanReadType(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");
			return true;
		}

		/// <summary>
		///     Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can serialize an object of the
		///     specified type.
		/// </summary>
		/// <returns>
		///     true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can serialize the type; otherwise,
		///     false.
		/// </returns>
		/// <param name="type">The type to serialize.</param>
		public override bool CanWriteType(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");
			return true;
		}

		/// <summary>
		///     Asynchronously deserializes an object of the specified type.
		/// </summary>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task" /> whose result will be an object of the given type.
		/// </returns>
		/// <param name="type">The type of the object to deserialize.</param>
		/// <param name="readStream">The <see cref="T:System.IO.Stream" /> to read.</param>
		/// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. It may be null.</param>
		/// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger" /> to log events to.</param>
		/// <exception cref="T:System.NotSupportedException">Derived types need to support reading.</exception>
		public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content,
			IFormatterLogger formatterLogger)
		{
			return Task.Factory.StartNew(() => _serializer.DeserializeFromStream(readStream, type));
		}

		/// <summary>
		///     Asynchronously writes an object of the specified type.
		/// </summary>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task" /> that will perform the write.
		/// </returns>
		/// <param name="type">The type of the object to write.</param>
		/// <param name="value">The object value to write.  It may be null.</param>
		/// <param name="writeStream">The <see cref="T:System.IO.Stream" /> to which to write.</param>
		/// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> if available. It may be null.</param>
		/// <param name="transportContext">The <see cref="T:System.Net.TransportContext" /> if available. It may be null.</param>
		/// <exception cref="T:System.NotSupportedException">Derived types need to support writing.</exception>
		public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
			TransportContext transportContext)
		{
			return Task.Factory.StartNew(() => _serializer.SerializeToStream(value, writeStream));
		}
	}
}