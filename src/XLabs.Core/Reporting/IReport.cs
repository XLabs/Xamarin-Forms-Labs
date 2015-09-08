namespace XLabs.Reporting
{
    using System;

    /// <summary>
    /// Interface for reporting events.
    /// </summary>
    public interface IReport
    {
        /// <summary>
        /// Report an exception.
        /// </summary>
        /// <param name="exception">Exception that happened.</param>
        void Exception(Exception exception);
    }
}