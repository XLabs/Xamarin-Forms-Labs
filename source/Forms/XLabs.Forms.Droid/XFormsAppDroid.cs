namespace XLabs.Forms
{
    using Android.OS;

    using Xamarin.Forms;

    using XLabs.Platform.Device;
    using XLabs.Platform.Mvvm;
    using XLabs.Platform.Services;
    using XLabs.Platform.Services.Email;
    using XLabs.Platform.Services.Geolocation;
    using XLabs.Platform.Services.IO;
    using XLabs.Platform.Services.Media;

    /// <summary>
    /// Class XFormsAppDroid.
    /// </summary>
    public class XFormsAppDroid : XFormsApp<XFormsApplicationDroid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XFormsAppDroid"/> class.
        /// </summary>
        public XFormsAppDroid() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XFormsAppDroid"/> class.
        /// </summary>
        /// <param name="app">The application.</param>
        public XFormsAppDroid(XFormsApplicationDroid app) : base(app) { }

        /// <summary>
        /// Raises the back press.
        /// </summary>
        public void RaiseBackPress()
        {
            this.OnBackPress();
        }

        /// <summary>
        /// Called when [initialize].
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="initServices">Should initialize services.</param>
        protected override void OnInit(XFormsApplicationDroid app, bool initServices = true)
        {
            this.AppContext.Start += (o, e) => this.OnStartup();
            this.AppContext.Stop += (o, e) => this.OnClosing();
            this.AppContext.Pause += (o, e) => this.OnSuspended();
            this.AppContext.Resume += (o, e) => this.OnResumed();
            this.AppDataDirectory = Environment.ExternalStorageDirectory.AbsolutePath;

            if (initServices) 
            {
                DependencyService.Register<TextToSpeechService> ();
                DependencyService.Register<Geolocator> ();
                DependencyService.Register<MediaPicker> ();
                DependencyService.Register<SoundService> ();
                DependencyService.Register<EmailService> ();
                DependencyService.Register<FileManager> ();
                DependencyService.Register<AndroidDevice> ();
            }

            base.OnInit(app);
        }
    }
}