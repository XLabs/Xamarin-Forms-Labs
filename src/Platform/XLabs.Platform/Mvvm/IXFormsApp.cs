using System;
using System.Threading.Tasks;

namespace XLabs.Platform.Mvvm
{
    using Enums;

    /// <summary>
    /// Interface IXFormsApp
    /// </summary>
    public interface IXFormsApp
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value><c>true</c> if this instance is initialized; otherwise, <c>false</c>.</value>
        bool IsInitialized { get; }

        /// <summary>
        /// Gets or sets the application data directory.
        /// </summary>
        /// <value>The application data directory</value>
        string AppDataDirectory { get; set; }

        Orientation Orientation { get; }

        [Obsolete]
        Func<Task<bool>> BackPressDelegate
        {
            get;
            set;
        }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Gets or sets the initialize.
        /// </summary>
        /// <value>The initialize.</value>
        EventHandler<EventArgs> Initialize { get; set; }

        /// <summary>
        /// Gets or sets the on startup.
        /// </summary>
        /// <value>The on startup.</value>
        EventHandler<EventArgs> Startup { get; set; }

        /// <summary>
        /// Gets or sets the on close.
        /// </summary>
        /// <value>The on close.</value>
        EventHandler<EventArgs> Closing { get; set; }

        /// <summary>
        /// Gets or sets the on suspend.
        /// </summary>
        /// <value>The on suspend.</value>
        EventHandler<EventArgs> Suspended { get; set; }

        /// <summary>
        /// Gets or sets the on resume.
        /// </summary>
        /// <value>The on resume.</value>
        EventHandler<EventArgs> Resumed { get; set; }

        /// <summary>
        /// Gets or sets the on error.
        /// </summary>
        /// <value>The on error.</value>
        EventHandler<EventArgs> Error { get; set; }

        /// <summary>
        /// Gets or sets the on rotation.
        /// </summary>
        /// <value>The on rotation.</value>
        EventHandler<EventArgs<Orientation>> Rotation { get; set; }

        EventHandler<EventArgs> BackPress { get; set; } 

        #endregion Event Handlers

        #region Methods

        #endregion Methods
    }

    /// <summary>
    /// Interface IXFormsApp
    /// </summary>
    /// <typeparam name="TApp">The type of the t application.</typeparam>
    public interface IXFormsApp<TApp> : IXFormsApp
    {
        #region Properties

        /// <summary>
        /// Gets or sets the application context.
        /// </summary>
        /// <value>The application context.</value>
        TApp AppContext { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initializes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="initServices">Should initialize services.</param>
        void Init(TApp context,bool initServices = true);

        #endregion Methods
    }
}
