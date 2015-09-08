namespace XLabs.Platform.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Windows.Phone.Speech.Synthesis;

	/// <summary>
	/// The text to speech service implements <see cref="ITextToSpeechService" /> for Windows Phone.
	/// </summary>
	public class TextToSpeechService : ITextToSpeechService
	{
		const string DEFAULT_LOCALE = "en-US";
		/// <summary>
		/// The _synth
		/// </summary>
		private readonly SpeechSynthesizer _synth;

		/// <summary>
		/// Initializes a new instance of the <see cref="TextToSpeechService" /> class.
		/// </summary>
		public TextToSpeechService()
		{
			_synth = new SpeechSynthesizer();
		}

		/// <summary>
		/// The speak.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="language">The language.</param>
		public async void Speak (string text, string language = DEFAULT_LOCALE)
		{
			var voice = InstalledVoices.All.FirstOrDefault (c => c.Language == language) ?? InstalledVoices.Default;
			_synth.SetVoice (voice);
			await _synth.SpeakTextAsync (text);
		}

		/// <summary>
		/// Get installed languages.
		/// </summary>
		/// <returns>The installed language names.</returns>
		public IEnumerable<string> GetInstalledLanguages()
		{
			return InstalledVoices.All.Select(a => a.Language).Distinct();
		}
	}
}