namespace XLabs.Reporting
{
    using System.Diagnostics;

    /// <summary>
    /// Writes reports using Debug.WriteLine
    /// </summary>
    public class DebugReport : IReport
    {
        #region IReport Members

        public void Exception(System.Exception exception)
        {
            Debug.WriteLine(exception);
        }

        #endregion
    }
}