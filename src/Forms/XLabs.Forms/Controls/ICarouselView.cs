namespace XLabs.Forms.Controls
{
    /// <summary>
    /// An interface carousel views can
    /// implement to receive
    /// lifetime event notifications
    /// </summary>
    /// Element created at 15/11/2014,3:36 PM by Charles
    public interface ICarouselView
    {
        /// <summary>The view is about to be shown</summary>
        /// Element created at 15/11/2014,3:36 PM by Charles
        void Showing();

        /// <summary>The view has been shown</summary>
        /// Element created at 15/11/2014,3:37 PM by Charles
        void Shown();

        /// <summary>The view is about to be hiden</summary>
        /// Element created at 15/11/2014,3:37 PM by Charles
        void Hiding();

        /// <summary>The view has been hiden</summary>
        /// Element created at 15/11/2014,3:37 PM by Charles
        void Hiden();
    }
}