// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
//
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ImageButtonRenderer.shared.cs" company="XLabs Team">
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
using Xamarin.Forms;

#if __ANDROID__
using Xamarin.Forms.Platform.Android;

#elif __IOS__
using Xamarin.Forms.Platform.iOS;

#elif WINDOWS_PHONE
using Xamarin.Forms.Platform.WinPhone;

#elif WINDOWS_APP || WINDOWS_PHONE_APP
using Xamarin.Forms.Platform.WinRT;

#elif NETFX_CORE
using Xamarin.Forms.Platform.UWP;

#endif

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Draws a button on the Android platform with the image shown in the right
	/// position with the right size.
	/// </summary>
	public partial class ImageButtonRenderer
	{
		/// <summary>
		/// Returns the proper <see cref="IImageSourceHandler"/> based on the type of <see cref="ImageSource"/> provided.
		/// </summary>
		/// <param name="source">The <see cref="ImageSource"/> to get the handler for.</param>
		/// <returns>The needed handler.</returns>
		private static IImageSourceHandler GetHandler(ImageSource source)
		{
			IImageSourceHandler returnValue = null;
			if (source is UriImageSource)
			{
				returnValue = new ImageLoaderSourceHandler();
			}
			else if (source is FileImageSource)
			{
				returnValue = new FileImageSourceHandler();
			}
			else if (source is StreamImageSource)
			{
				returnValue = new StreamImagesourceHandler();
			}
			return returnValue;
		}

		/// <summary>
		/// Gets the width based on the requested width, if request less than 0, returns 50.
		/// </summary>
		/// <param name="requestedWidth">The requested width.</param>
		/// <returns>The width to use.</returns>
		private int GetWidth(int requestedWidth)
		{
			const int DefaultWidth = 50;
			return requestedWidth <= 0 ? DefaultWidth : requestedWidth;
		}

		/// <summary>
		/// Gets the height based on the requested height, if request less than 0, returns 50.
		/// </summary>
		/// <param name="requestedHeight">The requested height.</param>
		/// <returns>The height to use.</returns>
		private int GetHeight(int requestedHeight)
		{
			const int DefaultHeight = 50;
			return requestedHeight <= 0 ? DefaultHeight : requestedHeight;
		}
	}
}