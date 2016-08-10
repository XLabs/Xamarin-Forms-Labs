// ***********************************************************************
// Assembly         : XLabs.Platform.WinUniversal
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Display.cs" company="XLabs Team">
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

using Windows.Graphics.Display;

namespace XLabs.Platform.Device
{
	/// <summary>
	/// Windows Phone 8 Display.
	/// </summary>
	public class Display : IDisplay
	{
		private DeviceInfo.DeviceProperties _deviceProperties;

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents the current <see cref="Display" />.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents the current <see cref="Display" />.</returns>
		public override string ToString()
		{
			return string.Format("[Screen: Height={0}, Width={1}, Xdpi={2:0.0}, Ydpi={3:0.0}]", Height, Width, Xdpi, Ydpi);
		}

		#region IDisplay Members

		/// <summary>
		/// Gets the screen height in pixels
		/// </summary>
		/// <value>The height.</value>
		public int Height { get { return (int)(_deviceProperties ?? (_deviceProperties = DeviceInfo.DeviceProperties.GetInstance())).ScreenResolutionSize.Height; } }

		/// <summary>
		/// Gets the screen width in pixels
		/// </summary>
		/// <value>The width.</value>
		public int Width { get { return (int)(_deviceProperties ?? (_deviceProperties = DeviceInfo.DeviceProperties.GetInstance())).ScreenResolutionSize.Width; } }

		/// <summary>
		/// Gets the screens X pixel density per inch
		/// </summary>
		/// <value>The xdpi.</value>
		public double Xdpi { get { return Info.RawDpiY; } }

		/// <summary>
		/// Gets the screens Y pixel density per inch
		/// </summary>
		/// <value>The ydpi.</value>
		public double Ydpi { get { return Info.RawDpiY; } }

		/// <summary>
		/// Gets the scale value of the display.
		/// </summary>
		/// <value>The scale.</value>
		public double Scale
		{
			get { return ((int)Info.ResolutionScale) / 100; }
		}

		/// <summary>
		/// Convert width in inches to runtime pixels
		/// </summary>
		/// <param name="inches">The inches.</param>
		/// <returns>System.Double.</returns>
		public double WidthRequestInInches(double inches)
		{
			return inches * Info.LogicalDpi;
		}

		/// <summary>
		/// Convert height in inches to runtime pixels
		/// </summary>
		/// <param name="inches">The inches.</param>
		/// <returns>System.Double.</returns>
		public double HeightRequestInInches(double inches)
		{
			return inches * Info.LogicalDpi;
		}

		#endregion

		private static DisplayInformation Info
		{
			get { return DisplayInformation.GetForCurrentView(); }
		}
	}
}