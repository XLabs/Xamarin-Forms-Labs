// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="SensorBarDroidView.cs" company="XLabs Team">
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
using Android.Content;
using Android.Graphics;
using Android.Util;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class SensorBarDroidView.
	/// </summary>
	public class SensorBarDroidView :Android.Views.View
	{
		/// <summary>
		/// The _positive color
		/// </summary>
		private Android.Graphics.Color _positiveColor = Android.Graphics.Color.Green;
		/// <summary>
		/// The _negative color
		/// </summary>
		private Android.Graphics.Color _negativeColor = Android.Graphics.Color.Red;
		/// <summary>
		/// The _limit
		/// </summary>
		private double _limit = 1;
		/// <summary>
		/// The _current value
		/// </summary>
		private double _currentValue = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="SensorBarDroidView"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public SensorBarDroidView (Context context) :
		base (context)
		{
			Initialize ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SensorBarDroidView"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="attrs">The attrs.</param>
		public SensorBarDroidView (Context context, IAttributeSet attrs) :
		base (context, attrs)
		{
			Initialize ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SensorBarDroidView"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="attrs">The attrs.</param>
		/// <param name="defStyle">The definition style.</param>
		public SensorBarDroidView (Context context, IAttributeSet attrs, int defStyle) :
		base (context, attrs, defStyle)
		{
			Initialize ();
		}

		/// <summary>
		/// Gets or sets the current value.
		/// </summary>
		/// <value>The current value.</value>
		public double CurrentValue
		{
			get { return this._currentValue; }
			set 
			{ 
				if (Math.Abs(value) <= this.Limit)
				{
					this._currentValue = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the limit.
		/// </summary>
		/// <value>The limit.</value>
		public double Limit
		{
			get { return this._limit; }
			set { this._limit = value; }
		}

		/// <summary>
		/// Gets or sets the color of the positive.
		/// </summary>
		/// <value>The color of the positive.</value>
		public Android.Graphics.Color PositiveColor
		{
			get { return this._positiveColor; }
			set { this._positiveColor = value; }
		}

		/// <summary>
		/// Gets or sets the color of the negative.
		/// </summary>
		/// <value>The color of the negative.</value>
		public Android.Graphics.Color NegativeColor
		{
			get { return this._negativeColor; }
			set { this._negativeColor = value; }
		}

		/// <summary>
		/// Implement this to do your drawing.
		/// </summary>
		/// <param name="canvas">the canvas on which the background will be drawn</param>
		/// <since version="Added in API level 1" />
		/// <remarks><para tool="javadoc-to-mdoc">Implement this to do your drawing.</para>
		/// <para tool="javadoc-to-mdoc">
		///   <format type="text/html">
		///     <a href="http://developer.android.com/reference/android/view/View.html#onDraw(android.graphics.Canvas)" target="_blank">[Android Documentation]</a>
		///   </format>
		/// </para></remarks>
		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw (canvas);

			var r = new Rect ();
			this.GetLocalVisibleRect (r);

			var half = r.Width() / 2;
			var height = r.Height();

			var percentage = (this.Limit - Math.Abs(this.CurrentValue)) / this.Limit;


			var paint = new Paint()
			{
				Color = this.CurrentValue < 0 ? this._negativeColor : this._positiveColor,
				StrokeWidth = 5
			};

			paint.SetStyle(Paint.Style.Fill);

			if (this.CurrentValue < 0)
			{
				var start = (float)percentage * half;
				var size = half - start;
				canvas.DrawRect (new Rect ((int)start, 0, (int)(start + size), height), paint);
			}
			else
			{
				canvas.DrawRect (new Rect((int)half, 0, (int)(half + percentage * half), height), paint);
			}
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		private void Initialize()
		{

		}
	}
}

