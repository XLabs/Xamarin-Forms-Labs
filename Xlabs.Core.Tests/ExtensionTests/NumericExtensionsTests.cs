using System;
using NUnit.Framework;
using XLabs;

namespace Xlabs.Core.ExtensionsTests
{
	[TestFixture]
	public class NumericExtensionsTests
	{
		[Test]
		public void DoubleMinClamp()
		{
			double min = 3;
			double max = 6;
			double value = 2;
			Assert.AreEqual(min,NumericExtensions.Clamp(value,min,max));
		}
		[Test]
		public void DoubleMaxClamp()
		{
			double min = 3;
			double max = 6;
			double value = 9;
			Assert.AreEqual(max,NumericExtensions.Clamp(value,min,max));
		}
		[Test]
		public void DoubleClamp()
		{
			double min = 3;
			double max = 6;
			double value = 5;
			Assert.AreEqual(value,NumericExtensions.Clamp(value,min,max));
		}
		[Test]
		public void IntMinClamp()
		{
			int min = 3;
			int max = 6;
			int value = 2;
			Assert.AreEqual(min,NumericExtensions.Clamp(value,min,max));
		}
		[Test]
		public void IntMaxClamp()
		{
			int min = 3;
			int max = 6;
			int value = 9;
			Assert.AreEqual(max,NumericExtensions.Clamp(value,min,max));
		}
		[Test]
		public void IntClamp()
		{
			int min = 3;
			int max = 6;
			int value = 5;
			Assert.AreEqual(value,NumericExtensions.Clamp(value,min,max));
		}

	}
}

