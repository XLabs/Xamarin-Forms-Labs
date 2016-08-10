// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="UIViewExtensions.cs" company="XLabs Team">
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
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;

namespace XLabs.Platform.Extensions
{
	/// <summary>
	/// Class UiViewExtensions.
	/// </summary>
	public static class UiViewExtensions
	{
		/// <summary>
		/// Streams to PNG.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>Task.</returns>
		public static async Task StreamToPng(this UIView view, Stream stream)
		{
			var bytes = view.ToNativeImage().AsPNG().ToArray();

			await stream.WriteAsync(bytes, 0, bytes.Length);
		}

		/// <summary>
		/// Takes an image of the view.
		/// </summary>
		/// <param name="view">View to process to image.</param>
		/// <returns>A native image of type <see cref="UIImage" />.</returns>
		public static UIImage ToNativeImage(this UIView view)
		{
			UIGraphics.BeginImageContext(view.Bounds.Size);

			view.Layer.RenderInContext(UIGraphics.GetCurrentContext());

			var image = UIGraphics.GetImageFromCurrentImageContext();

			UIGraphics.EndImageContext();

			return image;
		}

		/// <summary>
		/// To the bitmap context.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns>CGBitmapContext.</returns>
		public static CGBitmapContext ToBitmapContext(this CGSize size)
		{
			using (var colorSpace = CGColorSpace.CreateDeviceRGB())
			{
				var pixelsWide = (int)size.Width;
				var pixelsHigh = (int)size.Height;

				var bitmapBytesPerRow = (pixelsWide * 4); // 1
				var bitmapByteCount = (bitmapBytesPerRow * pixelsHigh);

				var bitmapData = new byte[bitmapByteCount];

				return new CGBitmapContext(
					bitmapData,
					pixelsWide,
					pixelsHigh,
					8,
					// bits per component
					bitmapBytesPerRow,
					colorSpace,
					CGImageAlphaInfo.PremultipliedLast);
			}
		}
	}
}