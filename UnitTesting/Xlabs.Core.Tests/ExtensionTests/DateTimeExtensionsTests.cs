using System;
using NUnit.Framework;
using XLabs;

namespace Xlabs.Core.ExtensionsTests
{

	//Basically a pointless set of tests but completed for the code coverage.
	[TestFixture]
	public class DateTimeExtensionsTests
	{
		[Test]
		public void SinceUnixTimeTestDate()
		{
			var baseTime = new DateTime (1970, 1, 2);
			Assert.AreEqual (1.0d,DateTimeExtensions.SinceUnixTime (baseTime).TotalDays);
		}
		[Test]
		public void SinceUnixTimeTestOffset()
		{
			var baseOffset = new DateTimeOffset(1970,1,2,0,0,0,new TimeSpan(0));
			Assert.AreEqual ((baseOffset-new DateTime (1970, 1, 1)),DateTimeExtensions.SinceUnixTime (baseOffset));
		}
		[Test]
		public void SinceUnixTimeTestNullableDateTime()
		{
			var baseDT = new DateTime?(new DateTime(1970,1,2));
			Assert.AreEqual (1.0d,DateTimeExtensions.SinceUnixTime (baseDT).Value.TotalDays);
		}
		[Test]
		public void SinceUnixTimeTestNullableOffset()
		{
			var baseOffset = new DateTimeOffset?(new DateTimeOffset(1970,1,2,0,0,0,new TimeSpan(0)));
			Assert.AreEqual (baseOffset-new DateTime (1970, 1, 1),DateTimeExtensions.SinceUnixTime (baseOffset));
		}
		[Test]
		public void NullableFullMillisecondsWithNull()
		{
			Assert.IsNull(DateTimeExtensions.FullMilliseconds(null));
		}	
		[Test]
		public void NullableFullMillisecondsWithTimeSpan()
		{
			Assert.AreEqual(1000,DateTimeExtensions.FullMilliseconds(new TimeSpan?(new TimeSpan(0,0,1))));
		}	
	}
}

