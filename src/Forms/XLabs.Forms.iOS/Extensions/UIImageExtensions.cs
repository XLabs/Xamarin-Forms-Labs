// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="UIImageExtensions.cs" company="XLabs Team">
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
using System.Threading;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Platform.Extensions;

namespace XLabs.Forms.Extensions
{
	/// <summary>
	/// Class UiImageExtensions.
	/// </summary>
	public static class UiImageExtensions
	{
		/// <summary>
		/// Adds the text.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="text">The text.</param>
		/// <param name="point">The point.</param>
		/// <param name="font">The font.</param>
		/// <param name="color">The color.</param>
		/// <returns>Task&lt;ImageSource&gt;.</returns>
		public static async Task<ImageSource> AddText(
			this StreamImageSource source,
			string text,
			CGPoint point,
			Font font,
			Color color)
		{
			var token = new CancellationTokenSource();
			var stream = await source.Stream(token.Token);
			var image = UIImage.LoadFromData(NSData.FromStream(stream));

			var bytes = image.AddText(text, point, font.ToUIFont(), color.ToUIColor()).AsPNG().ToArray();

			return ImageSource.FromStream(() => new MemoryStream(bytes));
		}

		/// <summary>
		/// Adds the text.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="text">The text.</param>
		/// <param name="point">The point.</param>
		/// <param name="font">The font.</param>
		/// <param name="color">The color.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns>UIImage.</returns>
		public static UIImage AddText(
			this UIImage image,
			string text,
			CGPoint point,
			UIFont font,
			UIColor color,
			UITextAlignment alignment = UITextAlignment.Left)
		{
			//var labelRect = new RectangleF(point, new SizeF(image.Size.Width - point.X, image.Size.Height - point.Y));
			var h = text.StringHeight(font, image.Size.Width);
			var labelRect = new CGRect(point, new CGSize(image.Size.Width - point.X, h));

			var label = new UILabel(labelRect)
				            {
					            Font = font,
					            Text = text,
					            TextColor = color,
					            TextAlignment = alignment,
					            BackgroundColor = UIColor.Clear
				            };

			var labelImage = label.ToNativeImage();

			using (var context = image.Size.ToBitmapContext())
			{
				var rect = new CGRect(new CGPoint(0, 0), image.Size);
				context.DrawImage(rect, image.CGImage);
				context.DrawImage(labelRect, labelImage.CGImage);
				context.StrokePath();
				return UIImage.FromImage(context.ToImage());
			}
		}
	}
}