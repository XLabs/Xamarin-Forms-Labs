using System;
using Windows.Phone.Speech.Synthesis;
using Xamarin.Forms;
using XForms.Toolkit.Services;
using XForms.Toolkit.WP.Services;


[assembly: Dependency(typeof(TextToSpeechService))]
namespace XForms.Toolkit.WP.Services
{
    public class TextToSpeechService : ITextToSpeechService
    {
        SpeechSynthesizer synth;

        public TextToSpeechService()
        {
            synth = new SpeechSynthesizer();
        }


        public async void Speak(string text)
        {
            await synth.SpeakTextAsync(text);
        }
    }
}