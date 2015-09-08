using Android.App;
using Android.Content;

namespace XLabs.Platform.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Android.Speech.Tts;

    using Java.Lang;
    using Java.Util;

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
                _speaker.Speak(_toSpeak, QueueMode.Flush, p);
            }
        }

        #endregion

        /// <summary>
        ///     The speak.
        /// </summary>
        /// <param name="text">
        ///     The text.
        /// </param>
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
                _speaker.Speak(_toSpeak, QueueMode.Flush, p);
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