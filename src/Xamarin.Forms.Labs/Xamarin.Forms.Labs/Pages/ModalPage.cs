
namespace Xamarin.Forms.Labs.Pages
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A callback based modal page host
    /// usage:
    /// <code>
    /// <![CDATA[
    /// IModal<T> adds a property of type IModalHost<T> to your page class
    /// //Assuming you wanted your modal to return a string you would implement IModal<string> on your page class
    /// When your modal is complete call Host.ModalSuccess(T result) or Host.ModalCancel()
    /// Calling Host.ModalSuccess(T result) will call the function that you supplied as the
    /// success parem to the ModalHost.  Calling Modal.ModalCancel will call the cancel callback(if any).
    /// Suppose you had a modal that needed to return a string....
    /// 
    /// 
    /// class myModalPage : IModal<string>
    /// {
    ///   public IModalHost<string> Host{get;set;}//The Host will be set by the ModalHost<T> class before showing the dialog
    /// 
    ///  .....at some future point your dialog completes and you call Host.ModalSucess(T result) 
    /// 
    /// }
    /// var host = new ModalHost<string,myModalPage>(this.Navigation,new myModalPage(),(x)=>{ ... your success code},()=>{...your cancel code});
    /// host.show();
    /// 
    /// 
    /// ]]>
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type that the dialog is expected to produce</typeparam>
    /// <typeparam name="TPage">The Dialog Page type</typeparam>
    /// Element created at 05/11/2014,9:39 AM by Charles
    public class ModalHost<T,TPage> : IModalHost<T>
        where T:class where TPage: Page, IModal<T>
    {
        private readonly INavigation _navigation;
        private readonly TPage _page;
        private readonly Action _cancelAction;
        private readonly Action<T> _successAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModalHost{T, TPage}"/> class.
        /// </summary>
        /// <param name="navigation">The navigation object from the host page.</param>
        /// <param name="page">The page to display.</param>
        /// <param name="success">The success callback.</param>
        /// <param name="cancel">The cancel callback.</param>
        /// Element created at 06/11/2014,10:32 PM by Charles
        public ModalHost(INavigation navigation, TPage page, Action<T>success,Action cancel=null)
        {
            Contract.Assert(navigation != null,"Navigation object cannot be null");
            Contract.Assert(page != null,"Dialog page cannot be null");
            Contract.Assert(success != null,"success callback cannot be null");
            _successAction = success;
            _cancelAction = cancel;
            _navigation = navigation;
            _page = page;
            _page.Host = this;
        }

        /// <summary>Shows the dialog.</summary>
        /// Element created at 06/11/2014,10:33 PM by Charles
        public async void Show()
        {
            await _navigation.PushModalAsync(_page);

        }
        /// <summary>Closes the dialog and calls the success callback.</summary>
        /// <param name="result">The dialog result</param>
        /// Element created at 05/11/2014,9:46 AM by Charles
        public void ModalSuccess(T result)
        {
            Close(() => { if (_successAction != null) _successAction(result); });
        }

        /// <summary>Closes the dialog and calls the cancel callback(if any)</summary>
        /// Element created at 05/11/2014,9:46 AM by Charles
        public void ModalCancel()
        {
            Close(()=> { if (_cancelAction != null) _cancelAction(); });
        }

        /// <summary>Closes the dialog and calls the supplied action.</summary>
        /// <param name="resultAction">The result action.</param>
        /// Element created at 06/11/2014,10:35 PM by Charles
        private async void Close(Action resultAction)
        {
            await _navigation.PopModalAsync();
            resultAction();
        }
    }

    /// <summary>
    /// Implemented by dialog pages to allow
    /// the ModalHost to inject a Host param
    /// for the dialog page to use
    /// </summary>
    /// <typeparam name="T">The result type for the dialog</typeparam>
    /// Element created at 06/11/2014,10:36 PM by Charles
    public interface IModal<T>
    {
        /// <summary>Gets or sets the host.</summary>
        /// <value>An implmentation of IModalHost</value>
        /// Element created at 06/11/2014,10:37 PM by Charles
        IModalHost<T>  Host { get; set; }
    }

    /// <summary>
    /// Supplies callbacks to modal dialogs
    /// </summary>
    /// <typeparam name="T">The result type of the dialog</typeparam>
    /// Element created at 06/11/2014,10:40 PM by Charles
    public interface IModalHost<in T>
    {
        /// <summary>Calls the suppliced success callback.</summary>
        /// <param name="returnvalue">The resultvalue.</param>
        /// Element created at 06/11/2014,10:41 PM by Charles
        void ModalSuccess(T returnvalue);

        /// <summary>calls the supplied cancel.</summary>
        /// Element created at 06/11/2014,10:41 PM by Charles
        void ModalCancel();
    }
}
