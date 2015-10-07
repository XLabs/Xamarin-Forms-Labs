using System;
using NUnit.Framework;
using XLabs.Exceptions;
using System.Collections.Generic;


namespace Xlabs.Core.ExceptionTests
{
	[TestFixture]
	public class PropertyNotFoundExceptionTests
	{
		public class firstparam{}
		[Test]
		public void ConstructorSetsMessage()
		{
			var target = new PropertyNotFoundException(typeof(firstparam),"propertyName",new List<string>{"first","second"},"caller");
			Assert.IsNotNullOrEmpty( target.Message);
		}
		[Test]
		public void ConstructorAssignsInspectedType()
		{
			var target = new PropertyNotFoundException(typeof(firstparam),"propertyName",new List<string>{"first","second"},"caller");
			Assert.AreEqual(target.InspectedType, typeof(firstparam));
		}
		[Test]
		public void ConstructorInspectedTypeName()
		{
			var target = new PropertyNotFoundException(typeof(firstparam),"propertyName",new List<string>{"first","second"},"caller");
					Assert.AreEqual(target.InspectedTypeName, "firstparam");
		}
		[Test]
		public void ConstructorAssignsPropertyName()
		{
			var target = new PropertyNotFoundException(typeof(firstparam),"propertyName",new List<string>{"first","second"},"caller");
			Assert.AreEqual(target.PropertyName, "propertyName");
		}
		[Test]
		public void ConstructorAssignsAvailableProperties()
		{
			var target = new PropertyNotFoundException(typeof(firstparam),"propertyName",new List<string>{"first","second"},"caller");
			Assert.IsNotEmpty (target.AvailableProperties);
		}
	}
}

