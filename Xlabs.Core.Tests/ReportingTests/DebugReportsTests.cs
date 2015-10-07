using System;
using NUnit.Framework;
using XLabs.Reporting;

namespace Xlabs.Core.ReportingTests
{
	/// <summary>
	/// Apears to be a mock likely should be rmoved
	/// </summary>
	[TestFixture]
	public class DebugReportsTests
	{
		//Needs DI to test properly
		[Test]
		public void ExceptionClassTests()
		{
			var debugReport = new DebugReport ();
			debugReport.Exception(new Exception("Should appead in output"));
			//If you got this far it passed.
			Assert.Pass();
		}

	}
}

