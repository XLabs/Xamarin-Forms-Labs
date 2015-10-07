using System;
using NUnit.Framework;
using XLabs;

namespace Xlabs.Core.HelpersTests
{
	[TestFixture]
	public class EventArgs_T_Tests
	{
		public static string testData = "The quick brown fox jumped override the Lazy dog";
		[Test]
		public void EventArgsTests ()
		{
			EventArgs<string> eArgs = new EventArgs<string> (EventArgs_T_Tests.testData);
			Assert.AreEqual ( EventArgs_T_Tests.testData,eArgs.Value);
		}
	}
}

