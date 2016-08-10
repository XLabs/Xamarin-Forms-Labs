// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
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
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
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
			get { return (WebImage)Element; }
		}


		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			UIImage image;
			var networkStatus = Reachability.InternetConnectionStatus();

			var isReachable = networkStatus != NetworkStatus.NotReachable;

			if (isReachable)
			{
				image = GetImageFromWeb(WebImage.ImageUrl);
			}
			else
			{
				image = string.IsNullOrEmpty(WebImage.DefaultImage)
					? new UIImage()
					: UIImage.FromBundle(WebImage.DefaultImage);
			}

			var imageView = new UIImageView(image);

			SetNativeControl(imageView);
		}


		/// <summary>
		/// Gets the image from web.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns>UIImage.</returns>
		private UIImage GetImageFromWeb(string url)
		{
			using (var webclient = new WebClient())
			{
				var imageBytes = webclient.DownloadData(url);

				return UIImage.LoadFromData(NSData.FromArray(imageBytes));
			}
		}
	}
}