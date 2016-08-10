// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IAudioStream.cs" company="XLabs Team">
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
using System.Threading.Tasks;

namespace XLabs.Platform.Services.Media
{
	/// <summary>
	/// Interface IAudioStream
	/// </summary>
	public interface IAudioStream
	{
		/// <summary>
		/// Occurs when new audio has been streamed.
		/// </summary>
		event EventHandler<EventArgs<byte[]>> OnBroadcast;

		/// <summary>
		/// Gets the sample rate.
		/// </summary>
		/// <value>The sample rate in hertz.</value>
		int SampleRate { get; }

		/// <summary>
		/// Gets the channel count.
		/// </summary>
		/// <value>The channel count.</value>
		int ChannelCount { get; }

		/// <summary>
		/// Gets bits per sample.
		/// </summary>
		/// <value>The bits per sample.</value>
		int BitsPerSample { get; }

		/// <summary>
		/// Gets the average data transfer rate
		/// </summary>
		/// <value>The average data transfer rate in bytes per second.</value>
		//int AverageBytesPerSecond { get; }

		IEnumerable<int> SupportedSampleRates { get; }

		/// <summary>
		/// Starts the specified sample rate.
		/// </summary>
		/// <param name="sampleRate">The sample rate.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		Task<bool> Start(int sampleRate);

		/// <summary>
		/// Stops this instance.
		/// </summary>
		/// <returns>Task.</returns>
		Task Stop();
	}
}
