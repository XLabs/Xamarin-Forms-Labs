using System;
using NUnit.Framework;
using System.IO;
using System.Text;
using XLabs;

namespace Xlabs.Core.HelpersTests
{
	[TestFixture]
	public class BigEndianReaderTests
	{
		public static string testData = "The quick brown fox jumped override the Lazy dog";
		public BigEndianReader getReader()
		{
			MemoryStream stream = new MemoryStream (Encoding.UTF8.GetBytes (BigEndianReaderTests.testData));
			return new BigEndianReader(stream);
		}
		[Test]
		public void ReadBoolean()
		{
			Assert.AreEqual (true, getReader ().ReadBoolean ());
		}

		[Test]
		public void ReadByte()
		{
			Assert.AreEqual (84, getReader ().ReadByte());
		}
		[Test]
		public void ReadBytes()
		{
			Assert.AreEqual (new Byte[]{84,104}, getReader ().ReadBytes(2));
		}
		[Test]
		public void ReadChar()
		{
			Assert.AreEqual (21608, getReader ().ReadChar());
		}
		[Test]
		public void ReadChars()
		{
			Assert.AreEqual (new Char[]{(char)21608,(char)25888}, getReader ().ReadChars(2));
		}
		[Test]
 		public void ReadDecimal()
		{
			Assert.AreEqual (0, getReader ().ReadDecimal());
		}
		[Test]
		public void ReadDouble()
		{
			double d = getReader ().ReadDouble ();
			Assert.AreEqual (4.1685967923796424E+98d, getReader ().ReadDouble());
		}
		[Test]
		public void ReadInt16()
		{
			Assert.AreEqual (21608, getReader ().ReadInt16());
		}
		[Test]
		public void ReadInt32()
		{
			Assert.AreEqual (1416127776, getReader ().ReadInt32());
		}
		[Test]
		public void ReadInt64()
		{
			Assert.AreEqual (6082222486780733795, getReader ().ReadInt64());
		}
		[Test]
		public void ReadSByte()
		{
			Assert.AreEqual (84, getReader ().ReadSByte());
		}
		[Test]
		public void ReadSingle()
		{
			Assert.AreEqual (3.99251603E+12f, getReader ().ReadSingle());
		}
		[Test]
		public void ReadString()
		{
			Assert.AreEqual (BigEndianReaderTests.testData, getReader ().ReadString());
		}
		[Test]
		public void ReadUInt16()
		{
			Assert.AreEqual (21608, getReader ().ReadUInt16());
		}
		[Test]
		public void ReadUInt32()
		{
			Assert.AreEqual (1416127776, getReader ().ReadUInt32());
		}
		[Test]
		public void ReadUInt64()
		{
			Assert.AreEqual (6082222486780733795, getReader ().ReadUInt64());
		}

	}
}

