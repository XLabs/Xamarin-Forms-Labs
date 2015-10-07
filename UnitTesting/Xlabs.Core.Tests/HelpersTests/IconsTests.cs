using System;
using NUnit.Framework;
using XLabs;

namespace Xlabs.Core.HelpersTests
{
	/// <summary>
	/// Icons is a glorified enum. Not much to test so a list of asserts to verify no unintended changes slipped in were enough.
	/// </summary>
	[TestFixture]
	public class IconsTests
	{
		[Test]
		public void Tests ()
		{
			Assert.AreEqual (61757, (int)Icons.Anchor[0]);
			Assert.AreEqual (61881, (int)Icons.Car[0]);
			Assert.AreEqual (61452, (int)Icons.Check[0]);
			Assert.AreEqual (61634, (int)Icons.Cloud[0]);
			Assert.AreEqual (61465, (int)Icons.Download[0]);
			Assert.AreEqual (61664, (int)Icons.Envelope[0]);
			Assert.AreEqual (61582, (int)Icons.ExternalLink[0]);
			Assert.AreEqual (61594, (int)Icons.Facebook[0]);
			Assert.AreEqual (61570, (int)Icons.FacebookSquare[0]);
			Assert.AreEqual (61573, (int)Icons.Gears[0]);
			Assert.AreEqual (61595, (int)Icons.Github[0]);
			Assert.AreEqual (61856, (int)Icons.Google[0]);
			Assert.AreEqual (61653, (int)Icons.GooglePlus[0]);
			Assert.AreEqual (61475, (int)Icons.Lock[0]);
			Assert.AreEqual (61504, (int)Icons.Pencil[0]);
			Assert.AreEqual (61639, (int)Icons.Save[0]);
			Assert.AreEqual (61912, (int)Icons.Send[0]);
			Assert.AreEqual (61579, (int)Icons.SignOut[0]);
			Assert.AreEqual (61765, (int)Icons.Ticket[0]);
			Assert.AreEqual (61944, (int)Icons.TrashCan[0]);
			Assert.AreEqual (61825, (int)Icons.Trello[0]);
			Assert.AreEqual (61593, (int)Icons.Twitter[0]);
			Assert.AreEqual (61666, (int)Icons.Undo[0]);
			Assert.AreEqual (61596, (int)Icons.Unlock[0]);
			Assert.AreEqual (61447, (int)Icons.User[0]);
			Assert.AreEqual (61553, (int)Icons.Warning[0]);
			Assert.AreEqual (61818, (int)Icons.Windows[0]);
		}
	}
}

