// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ISoundService.cs" company="XLabs Team">
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
using System.Threading.Tasks;

namespace XLabs.Platform.Services.Media
{
	/// <summary>
	/// Interface ISoundService
	/// Enables playing any type of sound
	/// </summary>
	public interface ISoundService
	{
		/// <summary>
		/// Occurs when [sound file finished].
		/// </summary>
		event EventHandler SoundFileFinished;

		/// <summary>
		/// Plays the asynchronous.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="extension">The extension.</param>
		/// <returns></returns>
		Task<SoundFile> PlayAsync(string filename , string extension = null);

		/// <summary>
		/// Sets the media asynchronous.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <returns></returns>
		Task<SoundFile> SetMediaAsync(string filename);

		/// <summary>
		/// Goes to asynchronous.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <returns></returns>
		Task GoToAsync(double position);

		/// <summary>
		/// Plays this instance.
		/// </summary>
		void Play();

		/// <summary>
		/// Stops this instance.
		/// </summary>
		void Stop();

		/// <summary>
		/// Pauses this instance.
		/// </summary>
		void Pause();

		/// <summary>
		/// Gets or sets the volume.
		/// </summary>
		/// <value>
		/// The volume.
		/// </value>
		double Volume {get;set;}

		/// <summary>
		/// Gets the current time.
		/// </summary>
		/// <value>
		/// The current time.
		/// </value>
		double CurrentTime {get;}

		/// <summary>
		/// Gets a value indicating whether this instance is playing.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is playing; otherwise, <c>false</c>.
		/// </value>
		bool IsPlaying { get;}

		/// <summary>
		/// Gets the current file.
		/// </summary>
		/// <value>
		/// The current file.
		/// </value>
		SoundFile CurrentFile {get;}

	}
}

