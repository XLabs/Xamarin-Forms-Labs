// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="NSDataStream.cs" company="XLabs Team">
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

using System.IO;
using Foundation;

namespace XLabs.Platform.Extensions
{
	/// <summary>
	///     Class NsDataStream.
	/// </summary>
	internal unsafe class NsDataStream : UnmanagedMemoryStream
	{
		/// <summary>
		///     The _data
		/// </summary>
		private readonly NSData _data;

		/// <summary>
		///     Initializes a new instance of the <see cref="NsDataStream" /> class.
		/// </summary>
		/// <param name="data">The data.</param>
		public NsDataStream(NSData data)
			: base((byte*)data.Bytes, (long)data.Length)
		{
			_data = data;
		}

		/// <summary>
		///     Releases the unmanaged resources used by the <see cref="T:System.IO.UnmanagedMemoryStream" /> and optionally
		///     releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		///     true to release both managed and unmanaged resources; false to release only unmanaged
		///     resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_data.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}