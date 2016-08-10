// ***********************************************************************
// <copyright file="StreamExtensions.cs" company="XLabs">
//     Copyright © ServiceStack 2013 & XLabs
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;

namespace ServiceStack.Text
{
	/// <summary>
	/// Class StreamExtensions.
	/// </summary>
	public static class StreamExtensions
	{
		/// <summary>
		/// Writes to.
		/// </summary>
		/// <param name="inStream">The in stream.</param>
		/// <param name="outStream">The out stream.</param>
		public static void WriteTo(this Stream inStream, Stream outStream)
		{
			var memoryStream = inStream as MemoryStream;
			if (memoryStream != null)
			{
				memoryStream.WriteTo(outStream);
				return;
			}

			var data = new byte[4096];
			int bytesRead;

			while ((bytesRead = inStream.Read(data, 0, data.Length)) > 0)
			{
				outStream.Write(data, 0, bytesRead);
			}
		}

		/// <summary>
		/// Reads the lines.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>IEnumerable&lt;System.String&gt;.</returns>
		/// <exception cref="System.ArgumentNullException">reader</exception>
		public static IEnumerable<string> ReadLines(this StreamReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			string line;
			while ((line = reader.ReadLine()) != null)
			{
				yield return line;
			}
		}

		/// <summary>
		/// @jonskeet: Collection of utility methods which operate on streams.
		/// r285, February 26th 2009: http://www.yoda.arachsys.com/csharp/miscutil/
		/// </summary>
		const int DefaultBufferSize = 8 * 1024;

		/// <summary>
		/// Reads the given stream up to the end, returning the data as a byte
		/// array.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns>System.Byte[].</returns>
		public static byte[] ReadFully(this Stream input)
		{
			return ReadFully(input, DefaultBufferSize);
		}

		/// <summary>
		/// Reads the given stream up to the end, returning the data as a byte
		/// array, using the given buffer size.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="bufferSize">Size of the buffer.</param>
		/// <returns>System.Byte[].</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">bufferSize</exception>
		public static byte[] ReadFully(this Stream input, int bufferSize)
		{
			if (bufferSize < 1)
			{
				throw new ArgumentOutOfRangeException("bufferSize");
			}
			return ReadFully(input, new byte[bufferSize]);
		}

		/// <summary>
		/// Reads the given stream up to the end, returning the data as a byte
		/// array, using the given buffer for transferring data. Note that the
		/// current contents of the buffer is ignored, so the buffer needn't
		/// be cleared beforehand.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="buffer">The buffer.</param>
		/// <returns>System.Byte[].</returns>
		/// <exception cref="System.ArgumentNullException">
		/// buffer
		/// or
		/// input
		/// </exception>
		/// <exception cref="System.ArgumentException">Buffer has length of 0</exception>
		public static byte[] ReadFully(this Stream input, byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (buffer.Length == 0)
			{
				throw new ArgumentException("Buffer has length of 0");
			}
			// We could do all our own work here, but using MemoryStream is easier
			// and likely to be just as efficient.
			using (var tempStream = new MemoryStream())
			{
				CopyTo(input, tempStream, buffer);
				// No need to copy the buffer if it's the right size
#if !NETFX_CORE
				if (tempStream.Length == tempStream.GetBuffer().Length)
				{
					return tempStream.GetBuffer();
				}
#endif
				// Okay, make a copy that's the right size
				return tempStream.ToArray();
			}
		}

		/// <summary>
		/// Copies all the data from one stream into another.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="output">The output.</param>
		public static void CopyTo(this Stream input, Stream output)
		{
			CopyTo(input, output, DefaultBufferSize);
		}

		/// <summary>
		/// Copies all the data from one stream into another, using a buffer
		/// of the given size.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="output">The output.</param>
		/// <param name="bufferSize">Size of the buffer.</param>
		/// <exception cref="System.ArgumentOutOfRangeException">bufferSize</exception>
		public static void CopyTo(this Stream input, Stream output, int bufferSize)
		{
			if (bufferSize < 1)
			{
				throw new ArgumentOutOfRangeException("bufferSize");
			}
			CopyTo(input, output, new byte[bufferSize]);
		}

		/// <summary>
		/// Copies all the data from one stream into another, using the given
		/// buffer for transferring data. Note that the current contents of
		/// the buffer is ignored, so the buffer needn't be cleared beforehand.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="output">The output.</param>
		/// <param name="buffer">The buffer.</param>
		/// <exception cref="System.ArgumentNullException">
		/// buffer
		/// or
		/// input
		/// or
		/// output
		/// </exception>
		/// <exception cref="System.ArgumentException">Buffer has length of 0</exception>
		public static void CopyTo(this Stream input, Stream output, byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (buffer.Length == 0)
			{
				throw new ArgumentException("Buffer has length of 0");
			}
			int read;
			while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
			{
				output.Write(buffer, 0, read);
			}
		}

		/// <summary>
		/// Reads exactly the given number of bytes from the specified stream.
		/// If the end of the stream is reached before the specified amount
		/// of data is read, an exception is thrown.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="bytesToRead">The bytes to read.</param>
		/// <returns>System.Byte[].</returns>
		public static byte[] ReadExactly(this Stream input, int bytesToRead)
		{
			return ReadExactly(input, new byte[bytesToRead]);
		}

		/// <summary>
		/// Reads into a buffer, filling it completely.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="buffer">The buffer.</param>
		/// <returns>System.Byte[].</returns>
		public static byte[] ReadExactly(this Stream input, byte[] buffer)
		{
			return ReadExactly(input, buffer, buffer.Length);
		}

		/// <summary>
		/// Reads exactly the given number of bytes from the specified stream,
		/// into the given buffer, starting at position 0 of the array.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="buffer">The buffer.</param>
		/// <param name="bytesToRead">The bytes to read.</param>
		/// <returns>System.Byte[].</returns>
		public static byte[] ReadExactly(this Stream input, byte[] buffer, int bytesToRead)
		{
			return ReadExactly(input, buffer, 0, bytesToRead);
		}

		/// <summary>
		/// Reads exactly the given number of bytes from the specified stream,
		/// into the given buffer, starting at position 0 of the array.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="buffer">The buffer.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="bytesToRead">The bytes to read.</param>
		/// <returns>System.Byte[].</returns>
		/// <exception cref="System.ArgumentNullException">
		/// input
		/// or
		/// buffer
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// startIndex
		/// or
		/// bytesToRead
		/// </exception>
		public static byte[] ReadExactly(this Stream input, byte[] buffer, int startIndex, int bytesToRead)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}

			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}

			if (startIndex < 0 || startIndex >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}

			if (bytesToRead < 1 || startIndex + bytesToRead > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("bytesToRead");
			}

			return ReadExactlyFast(input, buffer, startIndex, bytesToRead);
		}

		/// <summary>
		/// Same as ReadExactly, but without the argument checks.
		/// </summary>
		/// <param name="fromStream">From stream.</param>
		/// <param name="intoBuffer">The into buffer.</param>
		/// <param name="startAtIndex">The start at index.</param>
		/// <param name="bytesToRead">The bytes to read.</param>
		/// <returns>System.Byte[].</returns>
		/// <exception cref="System.IO.EndOfStreamException"></exception>
		private static byte[] ReadExactlyFast(Stream fromStream, byte[] intoBuffer, int startAtIndex, int bytesToRead)
		{
			var index = 0;
			while (index < bytesToRead)
			{
				var read = fromStream.Read(intoBuffer, startAtIndex + index, bytesToRead - index);
				if (read == 0)
				{
					throw new EndOfStreamException
						(String.Format("End of stream reached with {0} byte{1} left to read.",
						               bytesToRead - index,
						               bytesToRead - index == 1 ? "s" : ""));
				}
				index += read;
			}
			return intoBuffer;
		}
	}
}