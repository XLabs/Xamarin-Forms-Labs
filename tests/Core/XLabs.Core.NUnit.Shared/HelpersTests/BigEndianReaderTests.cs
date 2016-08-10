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
        public static string TestData = "The quick brown fox jumped override the Lazy dog";

        private BigEndianReader GetReader(byte[] data)
        {
            var stream = new MemoryStream(data);
            return new BigEndianReader(stream);
        }

        private BigEndianReader GetReader()
        {
            return GetReader(Encoding.UTF8.GetBytes(TestData));
        }

        [Test]
        public void ReadBoolean()
        {
            Assert.AreEqual(true, GetReader().ReadBoolean());
        }

        [Test]
        public void ReadByte()
        {
            Assert.AreEqual(84, GetReader().ReadByte());
        }

        [Test]
        public void ReadBytes()
        {
            Assert.AreEqual(new Byte[] { 84, 104 }, GetReader().ReadBytes(2));
        }

        [Test]
        public void ReadChar()
        {
            Assert.AreEqual(21608, GetReader().ReadChar());
        }

        [Test]
        public void ReadChars()
        {
            Assert.AreEqual(new Char[] { (char)21608, (char)25888 }, GetReader().ReadChars(2));
        }

        [Test]
        public void ReadDecimal()
        {
            // First 4 bytes of encoded TestData does not represent a valid decimal number
            // see valid decimal bits: https://msdn.microsoft.com/en-us/library/t1de0ya1(v=vs.110).aspx
            // So exception is expected
            Assert.Throws(typeof(ArgumentException), () => { GetReader().ReadDecimal(); });
        }

        [Test]
        public void ReadDouble()
        {
            double d = GetReader().ReadDouble();
            Assert.AreEqual(4.1685967923796424E+98d, GetReader().ReadDouble());
        }

        [Test]
        public void ReadInt16()
        {
            Assert.AreEqual(21608, GetReader().ReadInt16());
        }

        [Test]
        public void ReadInt32()
        {
            Assert.AreEqual(1416127776, GetReader().ReadInt32());
        }

        [Test]
        public void ReadInt64()
        {
            Assert.AreEqual(6082222486780733795, GetReader().ReadInt64());
        }

        [Test]
        public void ReadSByte()
        {
            Assert.AreEqual(84, GetReader().ReadSByte());
        }

        [Test]
        public void ReadSingle()
        {
            Assert.AreEqual(3.99251603E+12f, GetReader().ReadSingle());
        }

        [Test]
        public void ReadString()
        {
            // TestData must be encoded with BigEndianReader's encoding
            var data = Encoding.BigEndianUnicode.GetBytes(TestData);
            var finalData = new byte[data.Length + 1];
            data.CopyTo(finalData, 1);
            finalData[0] = (byte)data.Length; // first byte has to be the length of the data
            Assert.AreEqual(TestData, GetReader(finalData).ReadString());
        }

        [Test]
        public void ReadUInt16()
        {
            Assert.AreEqual(21608, GetReader().ReadUInt16());
        }

        [Test]
        public void ReadUInt32()
        {
            Assert.AreEqual(1416127776, GetReader().ReadUInt32());
        }

        [Test]
        public void ReadUInt64()
        {
            Assert.AreEqual(6082222486780733795, GetReader().ReadUInt64());
        }
    }
}

