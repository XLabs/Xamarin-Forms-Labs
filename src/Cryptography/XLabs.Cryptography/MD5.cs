// ***********************************************************************
// Assembly         : XLabs.Cryptography
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="MD5.cs" company="XLabs Team">
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

namespace XLabs.Cryptography
{
	/// <summary>
	/// Summary description for MD5.
	/// </summary>
	public class MD5 : IDisposable
	{
		/// <summary>
		/// Creates the specified hash name.
		/// </summary>
		/// <param name="hashName">Name of the hash.</param>
		/// <returns>MD5.</returns>
		/// <exception cref="System.NotSupportedException"></exception>
		static public MD5 Create(string hashName)
		{
			if (hashName == "MD5")
				return new MD5();
			else
				throw new NotSupportedException();
		}

		/// <summary>
		/// Gets the MD5 string.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>System.String.</returns>
		static public string GetMd5String(String source)
		{
			var md = Create();
			var hash = md.ComputeHash(Encoding.UTF8.GetBytes(source));

			var sb = new StringBuilder();
			foreach (byte b in hash) sb.Append(b.ToString("x2"));
			return sb.ToString();
		}

		/// <summary>
		/// Creates this instance.
		/// </summary>
		/// <returns>MD5.</returns>
		static public MD5 Create()
		{
			return new MD5();
		}

		#region base implementation of the MD5
		#region constants
		/// <summary>
		/// The S11
		/// </summary>
		private const byte S11 = 7;
		/// <summary>
		/// The S12
		/// </summary>
		private const byte S12 = 12;
		/// <summary>
		/// The S13
		/// </summary>
		private const byte S13 = 17;
		/// <summary>
		/// The S14
		/// </summary>
		private const byte S14 = 22;
		/// <summary>
		/// The S21
		/// </summary>
		private const byte S21 = 5;
		/// <summary>
		/// The S22
		/// </summary>
		private const byte S22 = 9;
		/// <summary>
		/// The S23
		/// </summary>
		private const byte S23 = 14;
		/// <summary>
		/// The S24
		/// </summary>
		private const byte S24 = 20;
		/// <summary>
		/// The S31
		/// </summary>
		private const byte S31 = 4;
		/// <summary>
		/// The S32
		/// </summary>
		private const byte S32 = 11;
		/// <summary>
		/// The S33
		/// </summary>
		private const byte S33 = 16;
		/// <summary>
		/// The S34
		/// </summary>
		private const byte S34 = 23;
		/// <summary>
		/// The S41
		/// </summary>
		private const byte S41 = 6;
		/// <summary>
		/// The S42
		/// </summary>
		private const byte S42 = 10;
		/// <summary>
		/// The S43
		/// </summary>
		private const byte S43 = 15;
		/// <summary>
		/// The S44
		/// </summary>
		private const byte S44 = 21;
		/// <summary>
		/// The padding
		/// </summary>
		static private readonly byte[] Padding = {
			0x80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
		};
		#endregion

		#region F, G, H and I are basic MD5 functions.
		/// <summary>
		/// fs the specified x.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="z">The z.</param>
		/// <returns>System.UInt32.</returns>
		static private uint F(uint x, uint y, uint z)
		{
			return (((x) & (y)) | ((~x) & (z)));
		}
		/// <summary>
		/// gs the specified x.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="z">The z.</param>
		/// <returns>System.UInt32.</returns>
		static private uint G(uint x, uint y, uint z)
		{
			return (((x) & (z)) | ((y) & (~z)));
		}
		/// <summary>
		/// hes the specified x.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="z">The z.</param>
		/// <returns>System.UInt32.</returns>
		static private uint H(uint x, uint y, uint z)
		{
			return ((x) ^ (y) ^ (z));
		}
		/// <summary>
		/// is the specified x.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="z">The z.</param>
		/// <returns>System.UInt32.</returns>
		static private uint I(uint x, uint y, uint z)
		{
			return ((y) ^ ((x) | (~z)));
		}
		#endregion

		#region rotates x left n bits.
		/// <summary>
		/// rotates x left n bits.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="n">The n.</param>
		/// <returns>System.UInt32.</returns>
		static private uint ROTATE_LEFT(uint x, byte n)
		{
			return (((x) << (n)) | ((x) >> (32 - (n))));
		}
		#endregion

		#region FF, GG, HH, and II transformations
		/// <summary>
		/// Ffs the specified a.
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		/// <param name="c">The c.</param>
		/// <param name="d">The d.</param>
		/// <param name="x">The x.</param>
		/// <param name="s">The s.</param>
		/// <param name="ac">The ac.</param>
		/// FF, GG, HH, and II transformations
		/// for rounds 1, 2, 3, and 4.
		/// Rotation is separate from addition to prevent recomputation.
		static private void FF(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
		{
			(a) += F((b), (c), (d)) + (x) + (uint)(ac);
			(a) = ROTATE_LEFT((a), (s));
			(a) += (b);
		}
		/// <summary>
		/// Ggs the specified a.
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		/// <param name="c">The c.</param>
		/// <param name="d">The d.</param>
		/// <param name="x">The x.</param>
		/// <param name="s">The s.</param>
		/// <param name="ac">The ac.</param>
		static private void GG(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
		{
			(a) += G((b), (c), (d)) + (x) + (uint)(ac);
			(a) = ROTATE_LEFT((a), (s));
			(a) += (b);
		}
		/// <summary>
		/// Hhes the specified a.
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		/// <param name="c">The c.</param>
		/// <param name="d">The d.</param>
		/// <param name="x">The x.</param>
		/// <param name="s">The s.</param>
		/// <param name="ac">The ac.</param>
		static private void HH(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
		{
			(a) += H((b), (c), (d)) + (x) + (uint)(ac);
			(a) = ROTATE_LEFT((a), (s));
			(a) += (b);
		}
		/// <summary>
		/// Iis the specified a.
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		/// <param name="c">The c.</param>
		/// <param name="d">The d.</param>
		/// <param name="x">The x.</param>
		/// <param name="s">The s.</param>
		/// <param name="ac">The ac.</param>
		static private void II(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
		{
			(a) += I((b), (c), (d)) + (x) + (uint)(ac);
			(a) = ROTATE_LEFT((a), (s));
			(a) += (b);
		}
		#endregion

		#region context info
		/// <summary>
		/// state (ABCD)
		/// </summary>
		readonly uint[] state = new uint[4];

		/// <summary>
		/// number of bits, modulo 2^64 (lsb first)
		/// </summary>
		readonly uint[] count = new uint[2];

		/// <summary>
		/// input buffer
		/// </summary>
		readonly byte[] buffer = new byte[64];
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="MD5"/> class.
		/// </summary>
		internal MD5()
		{
			Initialize();
		}

		/// <summary>
		/// MD5 initialization. Begins an MD5 operation, writing a new context.
		/// </summary>
		/// <remarks>The RFC named it "MD5Init"</remarks>
		public virtual void Initialize()
		{
			this.count[0] = this.count[1] = 0;

			// Load magic initialization constants.
			this.state[0] = 0x67452301;
			this.state[1] = 0xefcdab89;
			this.state[2] = 0x98badcfe;
			this.state[3] = 0x10325476;
		}

		/// <summary>
		/// MD5 block update operation. Continues an MD5 message-digest
		/// operation, processing another message block, and updating the
		/// context.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="count">The count.</param>
		/// <remarks>The RFC Named it MD5Update</remarks>
		protected virtual void HashCore(byte[] input, int offset, int count)
		{
			int i;
			int index;
			int partLen;

			// Compute number of bytes mod 64
			index = (int)((this.count[0] >> 3) & 0x3F);

			// Update number of bits
			if ((this.count[0] += (uint)((uint)count << 3)) < ((uint)count << 3))
				this.count[1]++;
			this.count[1] += ((uint)count >> 29);

			partLen = 64 - index;

			// Transform as many times as possible.
			if (count >= partLen)
			{
				Buffer.BlockCopy(input, offset, this.buffer, index, partLen);
				Transform(this.buffer, 0);

				for (i = partLen; i + 63 < count; i += 64)
					Transform(input, offset + i);

				index = 0;
			}
			else
				i = 0;

			// Buffer remaining input 
			Buffer.BlockCopy(input, offset + i, this.buffer, index, count - i);
		}

		/// <summary>
		/// MD5 finalization. Ends an MD5 message-digest operation, writing the
		/// the message digest and zeroizing the context.
		/// </summary>
		/// <returns>message digest</returns>
		/// <remarks>The RFC named it MD5Final</remarks>
		protected virtual byte[] HashFinal()
		{
			byte[] digest = new byte[16];
			byte[] bits = new byte[8];
			int index, padLen;

			// Save number of bits
			Encode(bits, 0, this.count, 0, 8);

			// Pad out to 56 mod 64.
			index = (int)((uint)(this.count[0] >> 3) & 0x3f);
			padLen = (index < 56) ? (56 - index) : (120 - index);
			HashCore(Padding, 0, padLen);

			// Append length (before padding)
			HashCore(bits, 0, 8);

			// Store state in digest 
			Encode(digest, 0, this.state, 0, 16);

			// Zeroize sensitive information.
			this.count[0] = this.count[1] = 0;
			this.state[0] = 0;
			this.state[1] = 0;
			this.state[2] = 0;
			this.state[3] = 0;

			// initialize again, to be ready to use
			Initialize();

			return digest;
		}

		/// <summary>
		/// MD5 basic transformation. Transforms state based on 64 bytes block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="offset">The offset.</param>
		private void Transform(byte[] block, int offset)
		{
			uint a = this.state[0], b = this.state[1], c = this.state[2], d = this.state[3];
			uint[] x = new uint[16];
			Decode(x, 0, block, offset, 64);

			// Round 1
			FF(ref a, b, c, d, x[0], S11, 0xd76aa478); /* 1 */
			FF(ref d, a, b, c, x[1], S12, 0xe8c7b756); /* 2 */
			FF(ref c, d, a, b, x[2], S13, 0x242070db); /* 3 */
			FF(ref b, c, d, a, x[3], S14, 0xc1bdceee); /* 4 */
			FF(ref a, b, c, d, x[4], S11, 0xf57c0faf); /* 5 */
			FF(ref d, a, b, c, x[5], S12, 0x4787c62a); /* 6 */
			FF(ref c, d, a, b, x[6], S13, 0xa8304613); /* 7 */
			FF(ref b, c, d, a, x[7], S14, 0xfd469501); /* 8 */
			FF(ref a, b, c, d, x[8], S11, 0x698098d8); /* 9 */
			FF(ref d, a, b, c, x[9], S12, 0x8b44f7af); /* 10 */
			FF(ref c, d, a, b, x[10], S13, 0xffff5bb1); /* 11 */
			FF(ref b, c, d, a, x[11], S14, 0x895cd7be); /* 12 */
			FF(ref a, b, c, d, x[12], S11, 0x6b901122); /* 13 */
			FF(ref d, a, b, c, x[13], S12, 0xfd987193); /* 14 */
			FF(ref c, d, a, b, x[14], S13, 0xa679438e); /* 15 */
			FF(ref b, c, d, a, x[15], S14, 0x49b40821); /* 16 */

			// Round 2
			GG(ref a, b, c, d, x[1], S21, 0xf61e2562); /* 17 */
			GG(ref d, a, b, c, x[6], S22, 0xc040b340); /* 18 */
			GG(ref c, d, a, b, x[11], S23, 0x265e5a51); /* 19 */
			GG(ref b, c, d, a, x[0], S24, 0xe9b6c7aa); /* 20 */
			GG(ref a, b, c, d, x[5], S21, 0xd62f105d); /* 21 */
			GG(ref d, a, b, c, x[10], S22, 0x2441453); /* 22 */
			GG(ref c, d, a, b, x[15], S23, 0xd8a1e681); /* 23 */
			GG(ref b, c, d, a, x[4], S24, 0xe7d3fbc8); /* 24 */
			GG(ref a, b, c, d, x[9], S21, 0x21e1cde6); /* 25 */
			GG(ref d, a, b, c, x[14], S22, 0xc33707d6); /* 26 */
			GG(ref c, d, a, b, x[3], S23, 0xf4d50d87); /* 27 */
			GG(ref b, c, d, a, x[8], S24, 0x455a14ed); /* 28 */
			GG(ref a, b, c, d, x[13], S21, 0xa9e3e905); /* 29 */
			GG(ref d, a, b, c, x[2], S22, 0xfcefa3f8); /* 30 */
			GG(ref c, d, a, b, x[7], S23, 0x676f02d9); /* 31 */
			GG(ref b, c, d, a, x[12], S24, 0x8d2a4c8a); /* 32 */

			// Round 3
			HH(ref a, b, c, d, x[5], S31, 0xfffa3942); /* 33 */
			HH(ref d, a, b, c, x[8], S32, 0x8771f681); /* 34 */
			HH(ref c, d, a, b, x[11], S33, 0x6d9d6122); /* 35 */
			HH(ref b, c, d, a, x[14], S34, 0xfde5380c); /* 36 */
			HH(ref a, b, c, d, x[1], S31, 0xa4beea44); /* 37 */
			HH(ref d, a, b, c, x[4], S32, 0x4bdecfa9); /* 38 */
			HH(ref c, d, a, b, x[7], S33, 0xf6bb4b60); /* 39 */
			HH(ref b, c, d, a, x[10], S34, 0xbebfbc70); /* 40 */
			HH(ref a, b, c, d, x[13], S31, 0x289b7ec6); /* 41 */
			HH(ref d, a, b, c, x[0], S32, 0xeaa127fa); /* 42 */
			HH(ref c, d, a, b, x[3], S33, 0xd4ef3085); /* 43 */
			HH(ref b, c, d, a, x[6], S34, 0x4881d05); /* 44 */
			HH(ref a, b, c, d, x[9], S31, 0xd9d4d039); /* 45 */
			HH(ref d, a, b, c, x[12], S32, 0xe6db99e5); /* 46 */
			HH(ref c, d, a, b, x[15], S33, 0x1fa27cf8); /* 47 */
			HH(ref b, c, d, a, x[2], S34, 0xc4ac5665); /* 48 */

			// Round 4
			II(ref a, b, c, d, x[0], S41, 0xf4292244); /* 49 */
			II(ref d, a, b, c, x[7], S42, 0x432aff97); /* 50 */
			II(ref c, d, a, b, x[14], S43, 0xab9423a7); /* 51 */
			II(ref b, c, d, a, x[5], S44, 0xfc93a039); /* 52 */
			II(ref a, b, c, d, x[12], S41, 0x655b59c3); /* 53 */
			II(ref d, a, b, c, x[3], S42, 0x8f0ccc92); /* 54 */
			II(ref c, d, a, b, x[10], S43, 0xffeff47d); /* 55 */
			II(ref b, c, d, a, x[1], S44, 0x85845dd1); /* 56 */
			II(ref a, b, c, d, x[8], S41, 0x6fa87e4f); /* 57 */
			II(ref d, a, b, c, x[15], S42, 0xfe2ce6e0); /* 58 */
			II(ref c, d, a, b, x[6], S43, 0xa3014314); /* 59 */
			II(ref b, c, d, a, x[13], S44, 0x4e0811a1); /* 60 */
			II(ref a, b, c, d, x[4], S41, 0xf7537e82); /* 61 */
			II(ref d, a, b, c, x[11], S42, 0xbd3af235); /* 62 */
			II(ref c, d, a, b, x[2], S43, 0x2ad7d2bb); /* 63 */
			II(ref b, c, d, a, x[9], S44, 0xeb86d391); /* 64 */

			this.state[0] += a;
			this.state[1] += b;
			this.state[2] += c;
			this.state[3] += d;

			// Zeroize sensitive information.
			for (int i = 0; i < x.Length; i++)
				x[i] = 0;
		}

		/// <summary>
		/// Encodes input (uint) into output (byte). Assumes len is
		/// multiple of 4.
		/// </summary>
		/// <param name="output">The output.</param>
		/// <param name="outputOffset">The output offset.</param>
		/// <param name="input">The input.</param>
		/// <param name="inputOffset">The input offset.</param>
		/// <param name="count">The count.</param>
		private static void Encode(byte[] output, int outputOffset, uint[] input, int inputOffset, int count)
		{
			int i, j;
			int end = outputOffset + count;
			for (i = inputOffset, j = outputOffset; j < end; i++, j += 4)
			{
				output[j] = (byte)(input[i] & 0xff);
				output[j + 1] = (byte)((input[i] >> 8) & 0xff);
				output[j + 2] = (byte)((input[i] >> 16) & 0xff);
				output[j + 3] = (byte)((input[i] >> 24) & 0xff);
			}
		}

		/// <summary>
		/// Decodes input (byte) into output (uint). Assumes len is
		/// a multiple of 4.
		/// </summary>
		/// <param name="output">The output.</param>
		/// <param name="outputOffset">The output offset.</param>
		/// <param name="input">The input.</param>
		/// <param name="inputOffset">The input offset.</param>
		/// <param name="count">The count.</param>
		static private void Decode(uint[] output, int outputOffset, byte[] input, int inputOffset, int count)
		{
			int i, j;
			int end = inputOffset + count;
			for (i = outputOffset, j = inputOffset; j < end; i++, j += 4)
				output[i] = input[j] | (((uint)input[j + 1]) << 8) | (((uint)input[j + 2]) << 16) | (((uint)input[j + 3]) << 24);
		}
		#endregion

		#region expose the same interface as the regular MD5 object

		/// <summary>
		/// The hash value
		/// </summary>
		protected byte[] HashValue;
		/// <summary>
		/// The state
		/// </summary>
		protected int State;
		/// <summary>
		/// Gets a value indicating whether this instance can reuse transform.
		/// </summary>
		/// <value><c>true</c> if this instance can reuse transform; otherwise, <c>false</c>.</value>
		public virtual bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance can transform multiple blocks.
		/// </summary>
		/// <value><c>true</c> if this instance can transform multiple blocks; otherwise, <c>false</c>.</value>
		public virtual bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Gets the hash.
		/// </summary>
		/// <value>The hash.</value>
		/// <exception cref="System.InvalidOperationException"></exception>
		public virtual byte[] Hash
		{
			get
			{
				if (this.State != 0)
					throw new InvalidOperationException();
				return (byte[])this.HashValue.Clone();
			}
		}
		/// <summary>
		/// Gets the size of the hash.
		/// </summary>
		/// <value>The size of the hash.</value>
		public virtual int HashSize
		{
			get
			{
				return this.HashSizeValue;
			}
		}
		/// <summary>
		/// The hash size value
		/// </summary>
		protected int HashSizeValue = 128;

		/// <summary>
		/// Gets the size of the input block.
		/// </summary>
		/// <value>The size of the input block.</value>
		public virtual int InputBlockSize
		{
			get
			{
				return 1;
			}
		}
		/// <summary>
		/// Gets the size of the output block.
		/// </summary>
		/// <value>The size of the output block.</value>
		public virtual int OutputBlockSize
		{
			get
			{
				return 1;
			}
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			Dispose(true);
		}

		/// <summary>
		/// Computes the hash.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <returns>System.Byte[].</returns>
		public byte[] ComputeHash(byte[] buffer)
		{
			return ComputeHash(buffer, 0, buffer.Length);
		}
		/// <summary>
		/// Computes the hash.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="count">The count.</param>
		/// <returns>System.Byte[].</returns>
		public byte[] ComputeHash(byte[] buffer, int offset, int count)
		{
			Initialize();
			HashCore(buffer, offset, count);
			this.HashValue = HashFinal();
			return (byte[])this.HashValue.Clone();
		}

		/// <summary>
		/// Computes the hash.
		/// </summary>
		/// <param name="inputStream">The input stream.</param>
		/// <returns>System.Byte[].</returns>
		public byte[] ComputeHash(Stream inputStream)
		{
			Initialize();
			int count;
			byte[] buffer = new byte[4096];
			while (0 < (count = inputStream.Read(buffer, 0, 4096)))
			{
				HashCore(buffer, 0, count);
			}
			this.HashValue = HashFinal();
			return (byte[])this.HashValue.Clone();
		}

		/// <summary>
		/// Transforms the block.
		/// </summary>
		/// <param name="inputBuffer">The input buffer.</param>
		/// <param name="inputOffset">The input offset.</param>
		/// <param name="inputCount">The input count.</param>
		/// <param name="outputBuffer">The output buffer.</param>
		/// <param name="outputOffset">The output offset.</param>
		/// <returns>System.Int32.</returns>
		/// <exception cref="System.ArgumentNullException">inputBuffer</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// inputOffset
		/// or
		/// inputOffset
		/// </exception>
		/// <exception cref="System.ArgumentException">inputCount</exception>
		public int TransformBlock(
			byte[] inputBuffer,
			int inputOffset,
			int inputCount,
			byte[] outputBuffer,
			int outputOffset
		)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset");
			}
			if ((inputCount < 0) || (inputCount > inputBuffer.Length))
			{
				throw new ArgumentException("inputCount");
			}
			if ((inputBuffer.Length - inputCount) < inputOffset)
			{
				throw new ArgumentOutOfRangeException("inputOffset");
			}
			if (this.State == 0)
			{
				Initialize();
				this.State = 1;
			}

			HashCore(inputBuffer, inputOffset, inputCount);
			if ((inputBuffer != outputBuffer) || (inputOffset != outputOffset))
			{
				Buffer.BlockCopy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
			}
			return inputCount;
		}
		/// <summary>
		/// Transforms the final block.
		/// </summary>
		/// <param name="inputBuffer">The input buffer.</param>
		/// <param name="inputOffset">The input offset.</param>
		/// <param name="inputCount">The input count.</param>
		/// <returns>System.Byte[].</returns>
		/// <exception cref="System.ArgumentNullException">inputBuffer</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// inputOffset
		/// or
		/// inputOffset
		/// </exception>
		/// <exception cref="System.ArgumentException">inputCount</exception>
		public byte[] TransformFinalBlock(
			byte[] inputBuffer,
			int inputOffset,
			int inputCount
		)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset");
			}
			if ((inputCount < 0) || (inputCount > inputBuffer.Length))
			{
				throw new ArgumentException("inputCount");
			}
			if ((inputBuffer.Length - inputCount) < inputOffset)
			{
				throw new ArgumentOutOfRangeException("inputOffset");
			}
			if (this.State == 0)
			{
				Initialize();
			}
			HashCore(inputBuffer, inputOffset, inputCount);
			this.HashValue = HashFinal();
			byte[] buffer = new byte[inputCount];
			Buffer.BlockCopy(inputBuffer, inputOffset, buffer, 0, inputCount);
			this.State = 0;
			return buffer;
		}
		#endregion

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
				Initialize();
		}
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}
	}
}