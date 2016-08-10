// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="WebImageRenderer.cs" company="XLabs Team">
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

using System.Net;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls;
using XLabs.Platform.Services;

[assembly: ExportRenderer(typeof(WebImage), typeof(WebImageRenderer))]
namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class WebImageRenderer.
	/// </summary>
	public class WebImageRenderer : ImageRenderer
	{
		/// <summary>
		/// Gets the underlying control typed as an <see cref="WebImage" />
		/// </summary>
		/// <value>The web image.</value>
		private WebImage WebImage
		{
			get { return (WebImage) Element; }
		}


		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			var targetImageView = this.Control;

			Bitmap image = null;
			var networkStatus = Reachability.InternetConnectionStatus();
			var isReachable = networkStatus != NetworkStatus.NotReachable;

			if (isReachable && !string.IsNullOrEmpty(WebImage.ImageUrl))
			{
				image = GetImageFromWeb(WebImage.ImageUrl);
			}
			else
			{
				if (!string.IsNullOrEmpty(WebImage.DefaultImage))
				{
					var handler = new FileImageSourceHandler();
					image = handler.LoadImageAsync(ImageSource.FromFile(WebImage.DefaultImage), this.Context).Result;
				}
			}

			targetImageView.SetImageBitmap(image);
		}

		/// <summary>
		/// Gets the image from web.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns>Bitmap.</returns>
		private Bitmap GetImageFromWeb(string url)
		{
			Bitmap imageBitmap = null;
			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = Android.Graphics.BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
				}
			}

			return imageBitmap;
		}
	}
}