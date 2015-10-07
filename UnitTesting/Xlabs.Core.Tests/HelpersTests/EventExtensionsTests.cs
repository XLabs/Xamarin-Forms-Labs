using System;
using NUnit.Framework;
using XLabs;

namespace Xlabs.Core.HelpersTests
{
	[TestFixture]
	public class EventExtensionsTests
	{
		class dataClass{
			public bool Fired{get;set;}

		}
		dataClass Invoke1Value=new dataClass();
		dataClass TryInvoke2Value=new dataClass();
		[Test]
		public void Invoke1Tests ()
		{
			EventExtensions.Invoke<dataClass> (
				new EventHandler<EventArgs<dataClass>> (
					(object sender, EventArgs<dataClass> e) => ((dataClass)e.Value).Fired = true), this, Invoke1Value);
			Assert.IsTrue (Invoke1Value.Fired);
		}
		[Test]
		public void TryInvokeTests ()
		{
			bool result =true;
			result= EventExtensions.TryInvoke <EventArgs<dataClass>> (null, this, new EventArgs<dataClass>(Invoke1Value));
			Assert.IsFalse(result);
		}
		[Test]
		public void TryInvoke2Tests ()
		{
			bool result =false;
			result= EventExtensions.TryInvoke <EventArgs<dataClass>> (new EventHandler<EventArgs<dataClass>> (
				(object sender, EventArgs<dataClass> e) => ((dataClass)e.Value).Fired = true), this, new EventArgs<dataClass>(TryInvoke2Value));
			Assert.IsTrue(result);
			Assert.IsTrue(TryInvoke2Value.Fired);
		}

	}
}


