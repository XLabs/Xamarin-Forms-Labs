// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
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
using Android.App;
using Android.Content;
using Android.Speech.Tts;
using Java.Lang;
using Java.Util;

namespace XLabs.Platform.Services
{
    /// <summary>
    ///     The text to speech service implements <see cref="ITextToSpeechService" /> for Android.
    /// </summary>
    public class TextToSpeechService : Object, ITextToSpeechService, TextToSpeech.IOnInitListener
    {
        const string DefaultLocale = "en";
        private TextToSpeech _speaker;

        private string _toSpeak;

        private static Context Context
        {
            get { return Application.Context; }
        }


        #region IOnInitListener implementation

        /// <summary>
        ///     Implementation for <see cref="TextToSpeech.IOnInitListener.OnInit" />.
        /// </summary>
        /// <param name="status">
        ///     The status.
        /// </param>
        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                var p = new Dictionary<string, string>();

#pragma warning disable CS0618 // Type or member is obsolete
                _speaker.Speak(_toSpeak, QueueMode.Flush, p);
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        #endregion

        /// <summary>
        /// The speak.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="language">The language.</param>
        public void Speak (string text, string language = DefaultLocale)
        {
            _toSpeak = text;
            if (_speaker == null)
            {
                _speaker = new TextToSpeech(Context, this);

                var lang = GetInstalledLanguages().Where(c => c == language).DefaultIfEmpty(DefaultLocale).First();
                var locale = new Locale (lang);
                _speaker.SetLanguage (locale);
            }
            else
            {
                var p = new Dictionary<string, string>();

#pragma warning disable CS0618 // Type or member is obsolete
                _speaker.Speak(_toSpeak, QueueMode.Flush, p);
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        /// <summary>
        ///     Get installed languages.
        /// </summary>
        /// <returns>
        ///     The installed language names.
        /// </returns>
        public IEnumerable<string> GetInstalledLanguages()
        {
            return Locale.GetAvailableLocales().Select(a => a.Language).Distinct();
        }
    }
}