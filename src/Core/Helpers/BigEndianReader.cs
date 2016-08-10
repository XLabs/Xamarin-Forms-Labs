// ***********************************************************************
// Assembly         : XLabs.Core
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="BigEndianReader.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.IO;
using System.Text;

namespace XLabs
{
    /// <summary>
    /// Class BigEndianReader.
    /// </summary>
    public class BigEndianReader : BinaryReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BigEndianReader"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="leaveOpen">if set to <c>true</c> [leave open].</param>
        public BigEndianReader(Stream stream, bool leaveOpen = false) 
            : base(stream, Encoding.BigEndianUnicode, leaveOpen)
        {
        }

        /// <summary>
        /// Reads the decimal.
        /// </summary>
        /// <returns>System.Decimal.</returns>
        public override decimal ReadDecimal()
        {
            // see valid decimal bits: https://msdn.microsoft.com/en-us/library/t1de0ya1(v=vs.110).aspx
            var low = ReadInt32();
            var middle = ReadInt32();
            var high = ReadInt32();
            var scale = ReadInt32();// (int)(ReadInt32() & 0x801C0000);
            return new decimal(new[] { low, middle, high, scale });
        }

        /// <summary>
        /// Reads the double.
        /// </summary>
        /// <returns>System.Double.</returns>
        public override double ReadDouble()
        {
            return BitConverter.ToDouble(GetReversedBytes(8), 0);
        }

        /// <summary>
        /// Reads the single.
        /// </summary>
        /// <returns>System.Single.</returns>
        public override float ReadSingle()
        {
            return BitConverter.ToSingle(GetReversedBytes(4), 0);
        }

        /// <summary>
        /// Reads the int64.
        /// </summary>
        /// <returns>System.Int64.</returns>
        public override long ReadInt64()
        {
            return BitConverter.ToInt64(GetReversedBytes(8), 0);
        }

        /// <summary>
        /// Reads the u int64.
        /// </summary>
        /// <returns>System.UInt64.</returns>
        public override ulong ReadUInt64()
        {
            return BitConverter.ToUInt64(GetReversedBytes(8), 0);
        }

        /// <summary>
        /// Reads the int32.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public override int ReadInt32()
        {
            return BitConverter.ToInt32(GetReversedBytes(4), 0);
        }

        /// <summary>
        /// Reads the u int32.
        /// </summary>
        /// <returns>System.UInt32.</returns>
        public override uint ReadUInt32()
        {
            return BitConverter.ToUInt32(GetReversedBytes(4), 0);
        }

        /// <summary>
        /// Reads the int16.
        /// </summary>
        /// <returns>System.Int16.</returns>
        public override short ReadInt16()
        {
            return BitConverter.ToInt16(GetReversedBytes(2), 0);
        }

        /// <summary>
        /// Reads the u int16.
        /// </summary>
        /// <returns>System.UInt16.</returns>
        public override ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(GetReversedBytes(2), 0);
        }

        /// <summary>
        /// Reads the character.
        /// </summary>
        /// <returns>System.Char.</returns>
        public override char ReadChar()
        {
            return BitConverter.ToChar(GetReversedBytes(2), 0);
        }

        /// <summary>
        /// Gets the reversed bytes.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>System.Byte[].</returns>
        private byte[] GetReversedBytes(int count)
        {
            var bytes = this.ReadBytes(count);
            Array.Reverse(bytes);
            return bytes;
        }
    }
}