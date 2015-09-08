namespace XLabs
{
    using System;
    using System.IO;
    using System.Text;

    public class BigEndianReader : BinaryReader
    {
        public BigEndianReader(Stream stream, bool leaveOpen = false) 
            : base(stream, Encoding.BigEndianUnicode, leaveOpen)
        {
        }

        public override decimal ReadDecimal()
        {
            return new Decimal(new[] { ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32() });
        }

        public override double ReadDouble()
        {
            return BitConverter.ToDouble(GetReversedBytes(8), 0);
        }

        public override float ReadSingle()
        {
            return BitConverter.ToSingle(GetReversedBytes(4), 0);
        }

        public override long ReadInt64()
        {
            return BitConverter.ToInt64(GetReversedBytes(8), 0);
        }

        public override ulong ReadUInt64()
        {
            return BitConverter.ToUInt64(GetReversedBytes(8), 0);
        }

        public override int ReadInt32()
        {
            return BitConverter.ToInt32(GetReversedBytes(4), 0);
        }

        public override uint ReadUInt32()
        {
            return BitConverter.ToUInt32(GetReversedBytes(4), 0);
        }

        public override short ReadInt16()
        {
            return BitConverter.ToInt16(GetReversedBytes(2), 0);
        }

        public override ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(GetReversedBytes(2), 0);
        }

        public override char ReadChar()
        {
            return BitConverter.ToChar(GetReversedBytes(2), 0);
        }

        private byte[] GetReversedBytes(int count)
        {
            var bytes = this.ReadBytes(count);
            Array.Reverse(bytes);
            return bytes;
        }
    }
}