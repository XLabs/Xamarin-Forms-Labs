using System;
using NUnit.Framework;
using XLabs.Exceptions;

namespace Xlabs.Core.ExceptionTests
{
	[TestFixture]
	public class InvalidVisualObjectExceptionTests
	{
		public class FirstParam	{}
		[Test]
		public void ConstructorSetsMessage()
		{
			var target = new InvalidVisualObjectException(typeof(FirstParam),"name");
			Assert.IsNotNullOrEmpty( target.Message);
		}

	}
}

