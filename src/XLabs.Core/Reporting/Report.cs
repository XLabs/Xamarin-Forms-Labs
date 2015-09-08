namespace XLabs.Reporting
{
    using System.Collections.Generic;

    //public class Report : IReport
    public static class Report
    {
        private static readonly List<IReport> reporters;
 
        static Report()
        {
            reporters = new List<IReport>(); 
        }

        public static void Add(IReport report)
        {
            reporters.Add(report);
        }

        public static void Remove(IReport report)
        {
            reporters.Remove(report);
        }

        #region IReport calls

        public static void Exception(System.Exception exception)
        {
            foreach (var reporter in reporters)
            {
                reporter.Exception(exception);
            }
        }

        #endregion
    }
}