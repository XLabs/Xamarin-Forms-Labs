// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Pod.cs" company="XLabs Team">
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

using System.ComponentModel;
using XLabs.Platform.Extensions;

namespace XLabs.Platform.Device
{
	/// <summary>
	/// Apple iPod.
	/// </summary>
	public class Pod : AppleDevice
	{
		/// <summary>
		/// Enum PodVersion
		/// </summary>
		public enum PodVersion
		{
			/// <summary>
			/// The first generation
			/// </summary>
			[Description("iPod Touch 1G")]
			FirstGeneration = 1,

			/// <summary>
			/// The second generation
			/// </summary>
			[Description("iPod Touch 2G")]
			SecondGeneration,

			/// <summary>
			/// The third generation
			/// </summary>
			[Description("iPod Touch 3G")]
			ThirdGeneration,

			/// <summary>
			/// The fourth generation
			/// </summary>
			[Description("iPod Touch 4G")]
			FourthGeneration,

			/// <summary>
			/// The fifth generation
			/// </summary>
			[Description("iPod Touch 5G")]
			FifthGeneration

			/// <summary>
			/// The fifth generation
			/// </summary>
			[Description("iPod Touch 6G")]
			SixthGeneration
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Pod" /> class.
		/// </summary>
		/// <param name="majorVersion">Major version.</param>
		/// <param name="minorVersion">Minor version.</param>
		public Pod(int majorVersion, int minorVersion)
		{
			if (majorVersion < 6)
			{
				Version = (PodVersion)majorVersion;
			}
			else if(majorVersion == 7)
			{
				Version = PodVersion.SixthGeneration;
			}

			PhoneService = null;

			int width;
			int height;
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				CoreGraphics.CGRect bounds = UIKit.UIScreen.MainScreen.NativeBounds;
				width = (int)bounds.Width;
				height = (int)bounds.Height;
			}
			else
			{
				//All older devices are portrait by design so treat the default bounds as such
				CoreGraphics.CGRect bounds = UIKit.UIScreen.MainScreen.Bounds;
				width = Math.Min((int)bounds.Width, (int)bounds.Height);
				height = Math.Max((int)bounds.Width, (int)bounds.Height);
			}

			width *= (int)UIKit.UIScreen.MainScreen.Scale;
			height *= (int)UIKit.UIScreen.MainScreen.Scale;

			double baseDPI = 163; //dpi from 1st Gen iPhone devices
			double dpi = baseDPI * UIKit.UIScreen.MainScreen.Scale;

			Display = new Display(height, width, dpi, dpi);

			Name = HardwareVersion = Version.GetDescription();
		}

		/// <summary>
		/// Gets the version of iPod.
		/// </summary>
		/// <value>The version.</value>
		public PodVersion Version { get; private set; }
	}
}