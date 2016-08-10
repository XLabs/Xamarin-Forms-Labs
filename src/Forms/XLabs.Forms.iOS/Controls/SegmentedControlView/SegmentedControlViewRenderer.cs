// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="SegmentedControlViewRenderer.cs" company="XLabs Team">
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
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(SegmentedControlView), typeof(SegmentedControlViewRenderer))]

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class SegmentedControlViewRenderer.
	/// </summary>
	public class SegmentedControlViewRenderer : ViewRenderer<SegmentedControlView, UISegmentedControl>
	{
		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Control.ValueChanged -= HandleControlValueChanged;
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Handles the control value changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void HandleControlValueChanged(object sender, EventArgs e)
		{
			Element.SelectedItem = (int)Control.SelectedSegment;
		}

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == "SelectedItem")
			{
				Control.SelectedSegment = Element.SelectedItem;
			}
		}

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<SegmentedControlView> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				return;
			}

			var native = new UISegmentedControl(CGRect.Empty);
			var segments = e.NewElement.SegmentsItens.Split(';');

			for (var i = 0; i < segments.Length; i++)
			{
				native.InsertSegment(segments[i], i, false);
			}

			native.TintColor = e.NewElement.TintColor.ToUIColor();
			native.SelectedSegment = e.NewElement.SelectedItem;

			SetNativeControl(native);

			Control.ValueChanged += HandleControlValueChanged;
		}
	}
}

