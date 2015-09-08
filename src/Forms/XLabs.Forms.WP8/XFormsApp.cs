namespace XLabs.Forms
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Windows.Storage;

    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;
    using Platform.Device;
    using Platform.Services;
    using Platform.Services.Email;
    using Platform.Services.Geolocation;
    using Platform.Services.IO;
    using Platform.Services.Media;
    using Xamarin.Forms;
    using XLabs.Platform;
    using XLabs.Platform.Mvvm;
    using Application = System.Windows.Application;

    /// <summary>
    ///     The XLabs Windows Phone Application.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "Reviewed. Suppression is OK here.")]
    public class XFormsAppWP : XFormsApp<Application>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="XFormsAppWP" /> class.
        /// </summary>
        public XFormsAppWP()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XFormsAppWP" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        public XFormsAppWP(Application application)
            : base(application)
        {
        }

        /// <summary>
        ///     Raises the back press.
        /// </summary>
        public void RaiseBackPress()
        {
            OnBackPress();
        }

        /// <summary>
        ///     Sets the orientation.
        /// </summary>
        /// <param name="orientation">The orientation.</param>
        public void SetOrientation(PageOrientation orientation)
        {
            Orientation = orientation.ToOrientation();
        }

        /// <summary>
        ///     Initializes the specified context.
        /// </summary>
        /// <param name="app">The native application.</param>
        /// <param name="initServices">Should initialize services.</param>
        protected override void OnInit(Application app,bool initServices = true)
        {
            app.Startup += (o, e) => OnStartup();
            app.Exit += (o, e) => OnClosing();
            app.UnhandledException += (o, e) => OnError(e.ExceptionObject);
            AppDataDirectory = ApplicationData.Current.LocalFolder.Path;

            foreach (var svc in app.ApplicationLifetimeObjects.OfType<PhoneApplicationService>())
            {
                svc.Activated += (o, e) => OnResumed();
                svc.Deactivated += (o, e) => OnSuspended();
            }

            if (initServices) 
            {
                DependencyService.Register<TextToSpeechService>();
                DependencyService.Register<Geolocator>();
                DependencyService.Register<MediaPicker>();
                DependencyService.Register<SoundService>();
                DependencyService.Register<EmailService>();
                DependencyService.Register<FileManager>();
                DependencyService.Register<WindowsPhoneDevice>();
            }

            base.OnInit(app);
        }
    }
}