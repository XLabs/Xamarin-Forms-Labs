namespace XLabs.Forms
{
    using System.Diagnostics.CodeAnalysis;
    using Windows.Storage;
    using Windows.UI.Xaml;
    using Platform.Mvvm;

    /// <summary>
    ///     The XLabs Windows Universal Application.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "Reviewed. Suppression is OK here.")]
    public class XFormsAppWin : XFormsApp<Application>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="XFormsAppWP" /> class.
        /// </summary>
        public XFormsAppWin()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XFormsAppWP" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        public XFormsAppWin(Application application)
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

        public void RaiseStartup()
        {
            OnStartup();
        }

        public void RaiseClosing()
        {
            OnClosing();
        }

        /// <summary>
        ///     Sets the orientation.
        /// </summary>
        /// <param name="orientation">The orientation.</param>
        //public void SetOrientation(PageOrientation orientation)
        //{
        //    Orientation = orientation.ToOrientation();
        //}

        /// <summary>
        ///     Initializes the specified context.
        /// </summary>
        /// <param name="app">The native application.</param>
        /// <param name="initServices">Should initialize services.</param>
        protected override void OnInit(Application app,bool initServices = true)
        {
            
            //AppContext.Startup += (o, e) => OnStartup();
            //AppContext.Exit += (o, e) => OnClosing();
            app.UnhandledException += (o, e) => OnError(e.Exception);
            AppDataDirectory = ApplicationData.Current.LocalFolder.Path;

            app.Resuming += (o, e) => OnResumed();
            app.Suspending += (o, e) => OnSuspended();

            if (initServices) 
            {
                //TODO : REGISTER SERVICES
            }

            base.OnInit(app);
        }
    }
}