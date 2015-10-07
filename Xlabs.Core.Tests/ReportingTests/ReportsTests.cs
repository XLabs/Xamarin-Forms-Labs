using System;
using NUnit.Framework;
using XLabs.Reporting;

namespace Xlabs.Core.ReportingTests
{
	[TestFixture]
	public class ReportsTests
	{
		class reporter:IReport
		{
			public bool ExceptionCalled{ get; set; }
			#region IReport implementation
			public void Exception (Exception exception)
			{
				ExceptionCalled = true;
			}
			#endregion
			
		}
		[Test]
		public void AddTest()
		{
			var report = new reporter();
			Report.Add(report);
			Assert.Pass();
		}
		[Test]
		public void RemoveTest()
		{
			var report = new reporter();
			Report.Add(report);
			Report.Remove(report);
			Assert.Pass();
		}
		[Test]
		public void ExceptionTest()
		{
			var report = new reporter();
			Report.Add(report);
			Report.Exception (new Exception ("test"));
			Assert.IsTrue (report.ExceptionCalled);
		}

	}
}

