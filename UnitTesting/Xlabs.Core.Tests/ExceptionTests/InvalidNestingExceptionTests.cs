using System;
using NUnit.Framework;
using XLabs.Exceptions;
using System.Collections.Generic;

namespace Xlabs.Core.ExceptionTests
{
	[TestFixture]
	public class InvalidNestingExceptionTests
	{
		public class FirstParam	{}
		public class SecondParam	{}

		[Test]
		public void ConstructorSetsMessage()
		{
			var target = new InvalidNestingException (typeof(FirstParam),typeof(SecondParam),new List<string>{"first","second"});
			Assert.IsNotNullOrEmpty( target.Message);
		}

		[Test]
		public void ConstructorAssignsNestedType()
		{
			var target = new InvalidNestingException (typeof(FirstParam),typeof(SecondParam),new List<string>{"first","second"});
			Assert.IsTrue(target.NestedType==typeof(FirstParam));
		}
		[Test]
		public void ConstructorAssignsNestedName()
		{
			var target = new InvalidNestingException (typeof(FirstParam),typeof(SecondParam),new List<string>{"first","second"});
			Assert.IsTrue(target.NestedName=="FirstParam");
		}

		[Test]
		public void ConstructorAssignsExpectedContainer()
		{
			var target = new InvalidNestingException (typeof(FirstParam),typeof(SecondParam),new List<string>{"first","second"});
			Assert.IsTrue(target.ExpectedContainer==typeof(SecondParam));
		}
		[Test]
		public void ConstructorAssignsExpectedContainerName()
		{
			var target = new InvalidNestingException (typeof(FirstParam),typeof(SecondParam),new List<string>{"first","second"});
			Assert.IsTrue(target.ExpectedContainerName=="SecondParam");
		}

		[Test]
		public void ConstructorAssignsSearchPath()
		{
			var target = new InvalidNestingException (typeof(FirstParam),typeof(SecondParam),new List<string>{"first","second"});
			Assert.IsNotEmpty(target.SearchPath);
		}
	}
}

