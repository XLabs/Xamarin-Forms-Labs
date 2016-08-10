// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="TextToSpeechService.cs" company="XLabs Team">
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
using System.Linq;
using AVFoundation;

namespace XLabs.Platform.Services
{
	/// <summary>
	/// The text to speech service for iOS.
	/// </summary>
	public class TextToSpeechService : ITextToSpeechService
	{
		const string DEFAULT_LOCALE = "en-US";
		/// <summary>
		/// The speak.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="language">The language.</param>
		public void Speak (string text, string language = DEFAULT_LOCALE)
		{
			var speechSynthesizer = new AVSpeechSynthesizer();

			var voice = AVSpeechSynthesisVoice.FromLanguage (language) ?? AVSpeechSynthesisVoice.FromLanguage (DEFAULT_LOCALE);
		
			var speechUtterance = new AVSpeechUtterance(text)
				                      {
					                      Rate = AVSpeechUtterance.MaximumSpeechRate / 4,
										  Voice = AVSpeechSynthesisVoice.FromLanguage (language),
					                      Volume = 0.5f,
					                      PitchMultiplier = 1.0f
				                      };

			speechSynthesizer.SpeakUtterance(speechUtterance);
		}

		/// <summary>
		/// Get installed languages.
		/// </summary>
		/// <returns>The installed language names.</returns>
		public IEnumerable<string> GetInstalledLanguages()
		{
			return AVSpeechSynthesisVoice.GetSpeechVoices().Select(a => a.Language).Distinct();
		}
	}
}