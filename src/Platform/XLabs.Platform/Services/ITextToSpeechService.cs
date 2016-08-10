// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ITextToSpeechService.cs" company="XLabs Team">
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

using System.Collections.Generic;

namespace XLabs.Platform.Services
{
	/// <summary>
	/// The Text To Speech Service interface.
	/// </summary>
	public interface ITextToSpeechService
	{
		/// <summary>
		/// The speak method.
		/// </summary>
		/// <param name="text">
		/// The text to speak.
		/// </param>
		/// <param name="language">
		/// The language.
		/// </param>
		void Speak(string text, string language = "en-US");

		/// <summary>
		/// Get installed languages.
		/// </summary>
		/// <returns>
		/// The installed language names.
		/// </returns>
		IEnumerable<string> GetInstalledLanguages();
	}
}
