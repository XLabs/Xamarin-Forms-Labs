using System;
using NUnit.Framework;
using XLabs;

namespace Xlabs.Core.HelpersTests
{
	[TestFixture]
	public class Vector3Tests
	{
		[Test]
		public void Vector3ConstTests ()
		{
			var v3 = new Vector3();
			Assert.IsNotNull(v3); 
		}
		[Test]
		public void Vector3ConstParamTests ()
		{
			var v3 = new Vector3(1,2,3);
			Assert.AreEqual (1, v3.X);
			Assert.AreEqual (2, v3.Y );
			Assert.AreEqual (3, v3.Z);
		}
	}
}

