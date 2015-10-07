using System;
using NUnit.Framework;
using XLabs.Helpers;
using System.Collections.Generic;

namespace Xlabs.Core.HelpersTests
{
	/// <summary>
	/// Grouped list source is just some extended type aware constructors for Observable Collection.
	/// </summary>

	[TestFixture]
	public class GroupedListSourceTests
	{
		class dataClass{}
		[Test]
		public void GroupedListSourceEmptyConstructorTests ()
		{
			GroupedListSource<string,dataClass> list = new GroupedListSource<string,dataClass>();
			var myDataClass = new dataClass ();
			list.Add (new ListGroup<string, dataClass> (){ myDataClass, new dataClass () });
			Assert.Contains (myDataClass, list[0]);
		}
		[Test]
		public void GroupedListSourceConstructorTests ()
		{
			var myDataClass = new dataClass ();
			GroupedListSource<string,dataClass> list = new GroupedListSource<string,dataClass>(
				new List<ListGroup<string, dataClass>>{new ListGroup<string, dataClass> (){ myDataClass, new dataClass () }});
			Assert.Contains (myDataClass, list[0]);
		}
	}
}

