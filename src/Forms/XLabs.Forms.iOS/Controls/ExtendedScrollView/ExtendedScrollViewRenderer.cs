// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedScrollViewRenderer.cs" company="XLabs Team">
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
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedScrollView), typeof(ExtendedScrollViewRenderer))]
namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ExtendedScrollViewRenderer.
	/// </summary>
	public class ExtendedScrollViewRenderer : ScrollViewRenderer
	{
		/// <summary>
		/// Handles the <see cref="E:ElementChanged" /> event.
		/// </summary>
		/// <param name="e">The <see cref="VisualElementChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			Scrolled += (sender, ev) => {
				ExtendedScrollView sv = (ExtendedScrollView) Element;
				sv.UpdateBounds(Bounds.ToRectangle());
			};

			if (e.OldElement != null)
				e.OldElement.PropertyChanged -= OnElementPropertyChanged;
			e.NewElement.PropertyChanged += OnElementPropertyChanged;
		}

		/// <summary>
		/// The _epsilon
		/// </summary>
		double _epsilon = 0.1;

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == ExtendedScrollView.PositionProperty.PropertyName) {
				ExtendedScrollView sv = (ExtendedScrollView)Element;
				Point pt = sv.Position;

				if (System.Math.Abs(Bounds.Location.Y - pt.Y) < _epsilon
					&& System.Math.Abs(Bounds.Location.X - pt.X) < _epsilon)
					return;

				ScrollRectToVisible(
					new CGRect((float)pt.X, (float)pt.Y, Bounds.Width, Bounds.Height), sv.AnimateScroll);
			}
		}
	}
}

