// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CalendarArrowView.cs" company="XLabs Team">
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

namespace XLabs.Forms.Controls.MonoDroid.TimesSquare
{
	/// <summary>
	/// Class CalendarArrowView.
	/// </summary>
	public class CalendarArrowView : Android.Widget.Button
	{
		/// <summary>
		/// Enum ArrowDirection
		/// </summary>
		public enum ArrowDirection
		{
			/// <summary>
			/// The right
			/// </summary>
			RIGHT,
			/// <summary>
			/// The left
			/// </summary>
			LEFT
		}

		/// <summary>
		/// The _arrow direction
		/// </summary>
		private ArrowDirection _arrowDirection = ArrowDirection.LEFT;

		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		/// <value>The direction.</value>
		public ArrowDirection Direction {
			get {
				return _arrowDirection;
			}
			set {
				_arrowDirection = value;
				_trianglePath = GetEquilateralTriangle(this.Width, this.Height);
				Invalidate();
			}
		}

		/// <summary>
		/// Sets the color.
		/// </summary>
		/// <value>The color.</value>
		public Android.Graphics.Color Color {
			set {
				_trianglePaint.Color = value;
				Invalidate();
			}
		}

		/// <summary>
		/// The _triangle path
		/// </summary>
		Path _trianglePath;
		/// <summary>
		/// The _triangle paint
		/// </summary>
		Paint _trianglePaint;


		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarArrowView"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public CalendarArrowView(Context context) : base(context)
		{
			SharedConstructor();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarArrowView"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="attrSet">The attribute set.</param>
		public CalendarArrowView(Context context, IAttributeSet attrSet) : base(context, attrSet)
		{
			SharedConstructor();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarArrowView"/> class.
		/// </summary>
		/// <param name="javaReference">The java reference.</param>
		/// <param name="handleown">The handleown.</param>
		public CalendarArrowView(IntPtr javaReference, Android.Runtime.JniHandleOwnership handleown) : base(javaReference, handleown)
		{
			SharedConstructor();
		}

		/// <summary>
		/// Shareds the constructor.
		/// </summary>
		private void SharedConstructor()
		{
			_trianglePaint = new Paint();
			_trianglePaint.SetStyle(Android.Graphics.Paint.Style.Fill);
			_trianglePaint.AntiAlias = true;
			_trianglePaint.Color = Android.Graphics.Color.Black;
		}


		/// <summary>
		/// This is called during layout when the size of this view has changed.
		/// </summary>
		/// <param name="w">Current width of this view.</param>
		/// <param name="h">Current height of this view.</param>
		/// <param name="oldw">Old width of this view.</param>
		/// <param name="oldh">Old height of this view.</param>
		/// <since version="Added in API level 1" />
		/// <remarks><para tool="javadoc-to-mdoc">This is called during layout when the size of this view has changed. If
		/// you were just added to the view hierarchy, you're called with the old
		/// values of 0.</para>
		/// <para tool="javadoc-to-mdoc">
		///   <format type="text/html">
		///     <a href="http://developer.android.com/reference/android/view/View.html#onSizeChanged(int, int, int, int)" target="_blank">[Android Documentation]</a>
		///   </format>
		/// </para></remarks>
		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);
			_trianglePath = GetEquilateralTriangle(w, h);
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
			base.OnDraw(canvas);
			canvas.DrawPath(_trianglePath, _trianglePaint);
		}


		/// <summary>
		/// Gets the equilateral triangle.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns>Path.</returns>
		private Path GetEquilateralTriangle(int width, int height)
		{
			PointF p1, p2, p3;
			if(_arrowDirection == ArrowDirection.LEFT)
			{
				p1 = new PointF(0, (height) / 2);
				p2 = new PointF(width, 0);
				p3 = new PointF(width, height);
			} else
			{
				p1 = new PointF(width, height / 2);
				p2 = new PointF(0, 0);
				p3 = new PointF(0, height);
			}
			Path path = new Path();
			path.MoveTo(p1.X, p1.Y);
			path.LineTo(p2.X, p2.Y);
			path.LineTo(p3.X, p3.Y);
			return path;
		}


	}
}

