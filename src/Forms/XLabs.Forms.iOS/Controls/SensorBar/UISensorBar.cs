// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="UISensorBar.cs" company="XLabs Team">
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
using CoreGraphics;
using Foundation;
using UIKit;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// The UI sensor bar view.
	/// </summary>
	[Register("SensorBarView")]
	public class UISensorBar : UIView
	{
		private UIColor _positiveColor = UIColor.Green;
		private UIColor _negativeColor = UIColor.Red;
		private double _limit = 1;
		private double _currentValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="UISensorBar"/> class.
		/// </summary>
		public UISensorBar()
		{
			Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UISensorBar"/> class.
		/// </summary>
		/// <param name="bounds">
		/// The bounds.
		/// </param>
		public UISensorBar(CGRect bounds)
			: base(bounds)
		{
			Initialize();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UISensorBar"/> class.
		/// </summary>
		/// <param name="handle">
		/// The handle.
		/// </param>
		public UISensorBar(IntPtr handle)
			: base(handle)
		{
			Initialize();
		}

		/// <summary>
		/// Gets or sets the current value.
		/// </summary>
		//[Export, Browsable(true)]
		public double CurrentValue
		{
			get
			{
				return _currentValue;
			}

			set
			{ 
				if (Math.Abs(value) <= Limit)
				{
					_currentValue = value;
					SetNeedsDisplayInRect (Bounds);
				}
			}
		}

		/// <summary>
		/// Gets or sets the limit.
		/// </summary>
		//[Export, Browsable(true)]
		public double Limit
		{
			get { return _limit; }
			set { _limit = value; }
		}

		/// <summary>
		/// Gets or sets the positive color.
		/// </summary>
		//[Export, Browsable(true)]
		public UIColor PositiveColor
		{
			get { return _positiveColor; }
			set { _positiveColor = value; }
		}

		/// <summary>
		/// Gets or sets the negative color.
		/// </summary>
		//[Export, Browsable(true)]
		public UIColor NegativeColor
		{
			get { return _negativeColor; }
			set { _negativeColor = value; }
		}

		/// <summary>
		/// The draw.
		/// </summary>
		/// <param name="rect">
		/// The rectangle for draw.
		/// </param>
		public override void Draw(CGRect rect)
		{
			base.Draw(rect);
			var half = Bounds.Size.Width / 2.0f;
			var height = Bounds.Size.Height;
			var percentage = (Limit - Math.Abs(CurrentValue)) / Limit;

			var context = UIGraphics.GetCurrentContext();

			context.ClearRect(rect);

			context.SetFillColor(CurrentValue < 0 ? _negativeColor.CGColor : _positiveColor.CGColor);
			if (CurrentValue < 0)
			{
				var start = (float)percentage * half;
				var size = half - start;
				context.FillRect(new CGRect(start, 0, size, height));
			}
			else
			{
				context.FillRect(new CGRect(half, 0, (float)percentage * half, height));
			}
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		private static void Initialize()
		{
		}
	}
}