// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Separator.cs" company="XLabs Team">
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

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Enum StrokeType
	/// </summary>
	public enum StrokeType{
		/// <summary>
		/// The solid
		/// </summary>
		Solid,
		/// <summary>
		/// The dotted
		/// </summary>
		Dotted,
		/// <summary>
		/// The dashed
		/// </summary>
		Dashed
	}
	/// <summary>
	/// Enum SeparatorOrientation
	/// </summary>
	public enum SeparatorOrientation{
		/// <summary>
		/// The vertical
		/// </summary>
		Vertical,
		/// <summary>
		/// The horizontal
		/// </summary>
		Horizontal
	}

	/// <summary>
	/// Class Separator.
	/// </summary>
	public class Separator : View
	{

		/**
		 * Orientation property
		 */
		/// <summary>
		/// The orientation property
		/// </summary>
		public static readonly BindableProperty OrientationProperty = BindableProperty.Create("Orientation", typeof(SeparatorOrientation), typeof(Separator), SeparatorOrientation.Horizontal, BindingMode.OneWay, null, null, null, null);

		/**
		 * Orientation of the separator. Only
		 */
		/// <summary>
		/// Gets the orientation.
		/// </summary>
		/// <value>The orientation.</value>
		public SeparatorOrientation Orientation {
			get {
				return (SeparatorOrientation)base.GetValue(Separator.OrientationProperty);
			}

			private set {
				base.SetValue(Separator.OrientationProperty, value);
			}
		}

		/**
		 * Color property
		 */
		/// <summary>
		/// The color property
		/// </summary>
		public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(Separator), Color.Default, BindingMode.OneWay, null, null, null, null);

		/**
		 * Color of the separator. Black is a default color
		 */
		/// <summary>
		/// Gets or sets the color.
		/// </summary>
		/// <value>The color.</value>
		public Color Color {
			get {
				return (Color)base.GetValue(Separator.ColorProperty);
			}
			set {
				base.SetValue(Separator.ColorProperty, value);
			}
		}


		/**
		 * SpacingBefore property
		 */

		/// <summary>
		/// The spacing before property
		/// </summary>
		public static readonly BindableProperty SpacingBeforeProperty = BindableProperty.Create("SpacingBefore", typeof(double), typeof(Separator), (double)1, BindingMode.OneWay, null, null, null, null);

		/**
		 * Padding before the separator. Default is 1.
		 */
		/// <summary>
		/// Gets or sets the spacing before.
		/// </summary>
		/// <value>The spacing before.</value>
		public double SpacingBefore {
			get {
				return (double)base.GetValue(Separator.SpacingBeforeProperty);
			}
			set {
				base.SetValue(Separator.SpacingBeforeProperty, value);
			}
		}

		/**
		 * Spacing After property
		 */
		/// <summary>
		/// The spacing after property
		/// </summary>
		public static readonly BindableProperty SpacingAfterProperty = BindableProperty.Create("SpacingAfter", typeof(double), typeof(Separator), (double)1, BindingMode.OneWay, null, null, null, null);

		/**
		 * Padding after the separator. Default is 1.
		 */
		/// <summary>
		/// Gets or sets the spacing after.
		/// </summary>
		/// <value>The spacing after.</value>
		public double SpacingAfter {
			get {
				return (double)base.GetValue(Separator.SpacingAfterProperty);
			}
			set {
				base.SetValue(Separator.SpacingAfterProperty, value);
			}
		}

		/**
		 * Thickness property
		 */
		/// <summary>
		/// The thickness property
		/// </summary>
		public static readonly BindableProperty ThicknessProperty = BindableProperty.Create("Thickness", typeof(double), typeof(Separator), (double)1, BindingMode.OneWay, null, null, null, null);


		/**
		 * How thick should the separator be. Default is 1
		 */

		/// <summary>
		/// Gets or sets the thickness.
		/// </summary>
		/// <value>The thickness.</value>
		public double Thickness {
			get {
				return (double)base.GetValue(Separator.ThicknessProperty);
			}
			set {
				base.SetValue(Separator.ThicknessProperty, value);
			}
		}


		/**
		 * Stroke type property
		 */
		/// <summary>
		/// The stroke type property
		/// </summary>
		public static readonly BindableProperty StrokeTypeProperty = BindableProperty.Create("StrokeType", typeof(StrokeType), typeof(Separator), StrokeType.Solid, BindingMode.OneWay, null, null, null, null);

		/**
		 * Stroke style of the separator. Default is Solid.
		 */
		/// <summary>
		/// Gets or sets the type of the stroke.
		/// </summary>
		/// <value>The type of the stroke.</value>
		public StrokeType StrokeType {
			get {
				return (StrokeType)base.GetValue(Separator.StrokeTypeProperty);
			}
			set {
				base.SetValue(Separator.StrokeTypeProperty, value);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Separator"/> class.
		/// </summary>
		public Separator()
		{
			UpdateRequestedSize();
		}

		/// <summary>
		/// Call this method from a child class to notify that a change happened on a property.
		/// </summary>
		/// <param name="propertyName">The name of the property that changed.</param>
		/// <remarks>A <see cref="T:Xamarin.Forms.BindableProperty" /> triggers this by itself. An inheritor only needs to call this for properties without <see cref="T:Xamarin.Forms.BindableProperty" /> as the backend store.</remarks>
		protected override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);
			if(propertyName == ThicknessProperty.PropertyName ||
			   propertyName == ColorProperty.PropertyName ||
			   propertyName == SpacingBeforeProperty.PropertyName ||
			   propertyName == SpacingAfterProperty.PropertyName ||
			   propertyName == StrokeTypeProperty.PropertyName ||
			   propertyName == OrientationProperty.PropertyName) {
				UpdateRequestedSize();
			}
		}


		/// <summary>
		/// Updates the size of the requested.
		/// </summary>
		private void UpdateRequestedSize(){
			var minSize = Thickness;
			var optimalSize = SpacingBefore + Thickness + SpacingAfter;
			if(Orientation == SeparatorOrientation.Horizontal){
				MinimumHeightRequest = minSize;
				HeightRequest = optimalSize;
				HorizontalOptions = LayoutOptions.FillAndExpand;
			}else{
				MinimumWidthRequest = minSize;
				WidthRequest = optimalSize;
				VerticalOptions = LayoutOptions.FillAndExpand;
			}
		}
	}
}

