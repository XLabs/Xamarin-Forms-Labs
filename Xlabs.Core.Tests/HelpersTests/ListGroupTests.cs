using System;
using NUnit.Framework;
using XLabs.Helpers;
using System.Collections.Generic;

namespace Xlabs.Core.HelpersTests
{
	[TestFixture]
	public class ListGroupTests
	{
		class dataClass{}
		[Test]
		public void ListGroupEmptyConst()
		{
			var list = new ListGroup<String,dataClass> ();
			Assert.IsNotNull (list);
		}
		[Test]
		public void ListGroupConstInitList()
		{
			var someClass = new dataClass ();
			var list = new ListGroup<String,dataClass> (new List<dataClass>{someClass});
			Assert.Contains(someClass,list);
		}
		[Test]
		public void ListGroupConstInitListAndKey()
		{
			var someClass = new dataClass ();
			var list = new ListGroup<String,dataClass>("MyKey",new List<dataClass>{someClass});
			Assert.AreEqual ("MyKey",list.Key );
		}
	}
}

