// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="WaveRecorder.cs" company="XLabs Team">
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
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace XLabs.Platform.Services.Media
{
	/// <summary>
	/// Class WaveRecorder.
	/// </summary>
	public class WaveRecorder
	{
		/// <summary>
		/// The bits per sample
		/// </summary>
		private int _bitsPerSample;

		/// <summary>
		/// The byte count
		/// </summary>
		private int _byteCount;

		/// <summary>
		/// The channel count
		/// </summary>
		private int _channelCount;

		/// <summary>
		/// The sample rate
		/// </summary>
		private int _sampleRate;

		/// <summary>
		/// The stream
		/// </summary>
		private IAudioStream _stream;

		//private StreamWriter streamWriter;
		/// <summary>
		/// The writer
		/// </summary>
		private BinaryWriter _writer;

		/// <summary>
		/// Finalizes an instance of the <see cref="WaveRecorder"/> class.
		/// </summary>
		~WaveRecorder()
		{
			StopRecorder().Wait();
		}

		/// <summary>
		/// Starts the recorder.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="fileStream">The file stream.</param>
		/// <param name="sampleRate">The sample rate.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public async Task<bool> StartRecorder(IAudioStream stream, Stream fileStream, int sampleRate)
		{
			if (_stream != null || stream == null)
			{
				return false;
			}

			_stream = stream;

			try
			{
				_writer = new BinaryWriter(fileStream, Encoding.UTF8);
			}
			catch (Exception)
			{
				return false;
			}

			_byteCount = 0;
			_stream.OnBroadcast += OnStreamBroadcast;

			var result = await _stream.Start(sampleRate);
			if (result)
			{
				_sampleRate = sampleRate;
				_bitsPerSample = stream.BitsPerSample;
				_channelCount = stream.ChannelCount;
			}
			return result;
		}

		/// <summary>
		/// Stops the recorder.
		/// </summary>
		/// <returns>Task.</returns>
		public async Task StopRecorder()
		{
			if (_stream != null)
			{
				_stream.OnBroadcast -= OnStreamBroadcast;
				await _stream.Stop();
			}
			if (_writer != null && _writer.BaseStream.CanWrite)
			{
				WriteHeader();
				_writer.Dispose();
				_writer = null;
				_sampleRate = _bitsPerSample = _channelCount = -1;
			}

			_stream = null;
		}

		/// <summary>
		/// Called when [stream broadcast].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="eventArgs">The event arguments.</param>
		private void OnStreamBroadcast(object sender, EventArgs<byte[]> eventArgs)
		{
			_writer.Write(eventArgs.Value);
			_byteCount += eventArgs.Value.Length;
		}

		/// <summary>
		/// Writes the header.
		/// </summary>
		private void WriteHeader()
		{
			_writer.Seek(0, SeekOrigin.Begin);
			// chunk ID
			_writer.Write('R');
			_writer.Write('I');
			_writer.Write('F');
			_writer.Write('F');

			_writer.Write(_byteCount + 36);
			_writer.Write('W');
			_writer.Write('A');
			_writer.Write('V');
			_writer.Write('E');

			_writer.Write('f');
			_writer.Write('m');
			_writer.Write('t');
			_writer.Write(' ');

			_writer.Write(16);
			_writer.Write((short)1);

			_writer.Write((short)_channelCount);
			_writer.Write(_sampleRate);
			_writer.Write(_sampleRate * 2);
			_writer.Write((short)2);
			_writer.Write((short)_bitsPerSample);
			_writer.Write('d');
			_writer.Write('a');
			_writer.Write('t');
			_writer.Write('a');
			_writer.Write(_byteCount);
		}
	}
}