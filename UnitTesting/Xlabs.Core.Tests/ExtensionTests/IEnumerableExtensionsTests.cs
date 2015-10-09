using System;
using NUnit.Framework;
using System.Drawing;
using XLabs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace Xlabs.Core.ExtensionsTests
{
	[TestFixture]
	public class IEnumerableExtensionsTests
	{
		[Test]
		public void ConvertToReadonly()
		{
			IEnumerable<Color> myList = new List<Color>(){ Color.AliceBlue, Color.AntiqueWhite };
			IReadOnlyCollection<Color> myReadOnly = IEnumerableExtensions.ToReadOnlyCollection(myList);
			Assert.AreEqual (((List<Color>)myList).Count, myReadOnly.Count);
		}
		class dataClass
		{
			public string Name{ get; set; }
		}
		[Test]
		public void FluentForEach()
		{
			
			IEnumerable<dataClass> theOriginalData=new List<dataClass>{new dataClass(){Name="a"},new dataClass(){Name="b"},new dataClass(){Name="c"}};
			IEnumerable<dataClass> thechangedData= IEnumerableExtensions.ForEach<dataClass>(theOriginalData,delegate(dataClass obj) {obj.Name="New Name";});
			foreach(dataClass dc in thechangedData)
			{
				Assert.AreEqual("New Name",dc.Name);
			}
		}
	}
}

