using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace XLabs.Platform.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Windows.Media.SpeechSynthesis;

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
		/// Gets the global media element.
		/// </summary>
		/// <value>The global media element.</value>
		/// <exception cref="ArgumentNullException">GlobalMedia is missing</exception>
		public static MediaElement GlobalMediaElement
		{
			get
			{
				if (Application.Current.Resources.ContainsKey("GlobalMedia"))
				{
					return Application.Current.Resources["GlobalMedia"] as MediaElement;
				}

				throw new ArgumentNullException("Pre-requisite for use: Add a new MediaElement called 'GlobalMedia' instance to the System.Windows.Application.Current.Resources dictionary. Do not replace this instance at any point.");
			}
		}

		/// <summary>
		/// The speak.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="language">The language.</param>
		public async void Speak (string text, string language = DEFAULT_LOCALE)
		{
	
			var voice = SpeechSynthesizer.AllVoices.FirstOrDefault (c => c.Language == language) ?? SpeechSynthesizer.DefaultVoice;

			_synth.Voice = voice;

			var voiceStream = await _synth.SynthesizeTextToStreamAsync(text);

			GlobalMediaElement.SetSource(voiceStream, voiceStream.ContentType);

			GlobalMediaElement.Play();
		}

		/// <summary>
		/// Get installed languages.
		/// </summary>
		/// <returns>The installed language names.</returns>
		public IEnumerable<string> GetInstalledLanguages()
		{
			return SpeechSynthesizer.AllVoices.Select(a => a.Language).Distinct();
		}
	}
}