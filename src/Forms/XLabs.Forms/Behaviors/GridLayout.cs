// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="GridLayout.cs" company="XLabs Team">
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
using Xamarin.Forms;
using XLabs.Forms.Exceptions;

namespace XLabs.Forms.Behaviors
{
	/// <summary>
	/// GridLayout provides attached properties to simplify
	/// the layout of Grids in Xaml
	/// </summary>
	/// <example>
	/// Move from this:
	/// <code>
	/// <![CDATA[
	/// <Grid HorizontalOptions="FillAndExpand" VerticalOptions="FillAndExpand">
	///     <Grid.RowDefinitions>
	///         <RowDefinition Height="Auto"/>
	///         <RowDefinition Height="Auto"/>
	///         <RowDefinition Height="*"/>
	///         <RowDefinition Height="24"/>
	///     </Grid.RowDefinitions>
	///     <Grid.ColumnDefinitions>
	///         <ColumnDefinition Width="Auto"/>
	///         <ColumnDefinition Width="*"/>
	///         <ColumnDefinition Width="2*"/>
	///         <ColumnDefinition Width="64"/>
	///     </Grid.ColumnDefinitions>
	/// </Grid>
	/// ]]>
	/// </code>
	/// To this:(where lb is the XLabs.Forms.Behavior namespace prefix
	/// 
	/// <code>
	/// <![CDATA[
	/// <Grid HorizontalOptions="FillAndExpand" VerticalOptions="FillAndExpand" lb:GridLayout.RowHeights="Auto,Auto,*,24" lb.GridLayout.ColumnWidths="Auto,*,2*,64">
	/// </Grid>
	/// ]]>
	/// </code>
	/// 
	/// </example>
	public class GridLayout : BindableObject
	{
		/// <summary>
		/// The property definition of the RowHeights property
		/// </summary>
		public static BindableProperty RowHeightsProperty = BindableProperty.CreateAttached<GridLayout, string>(
			(x)=>x.GetValue<string>(RowHeightsProperty),
			"*",BindingMode.OneWay,
			(bo, v) =>!string.IsNullOrEmpty(v),
			RowHeightsChanged);

		/// <summary>
		/// The property definition of the ColumnWidthProperty
		/// </summary>
		public static BindableProperty ColumnWidthsProperty = BindableProperty.CreateAttached<GridLayout, string>(
			(x)=>x.GetValue<string>(ColumnWidthsProperty),
			"*",BindingMode.OneWay,
			(bo, v) =>!string.IsNullOrEmpty(v),
			ColumnWidthsChanged);

		
		/// <summary>
		/// This method is responsible for converting the RowHeights string
		/// into RowDefinitions.  The RowHeights string is a comma sepearated 
		/// string of height values.
		/// Auto,*,n*,and absolute values are supported ie:
		/// "Auto,*,2*,64" Will result in 4 row
		/// auto height, 1/3 of remaining height, 2/3 of remaining height and 64 height
		/// </summary>
		/// <param name="bo">The object the property was attached to. Must be a <see cref="Xamarin.Forms.Grid"/> or subclass</param>
		/// <param name="oldval">ignored</param>
		/// <param name="newval">the new height string</param>
		public static void RowHeightsChanged(BindableObject bo, string oldval, string newval)
		{
			var grid = bo as Grid;
			if (grid == null)
				throw new InvalidBindableException(bo,typeof(Grid));

			//Clear the old rows
			grid.RowDefinitions.Clear();
			var heights = newval.Split(',');
			foreach (var height in heights)
				grid.RowDefinitions.Add(new RowDefinition{Height=LengthFromString(height)});
		}

		/// <summary>
		/// This method is responsible for converting the ColumnWidths string
		/// into ColumnDefinitions.  The ColumnWidths string is a comma sepearated 
		/// string of width values.
		/// Auto,*,n*,and absolute values are supported ie:
		/// "Auto,*,2*,64" Will result in 4 columns
		/// auto width, 1/3 of remaining width, 2/3 of remaining width and 64 width
		/// </summary>
		/// <param name="bo">The object the property was attached to. Must be a <see cref="Xamarin.Forms.Grid"/> or subclass</param>
		/// <param name="oldval">ignored</param>
		/// <param name="newval">the new wdith string</param>
		public static void ColumnWidthsChanged(BindableObject bo, string oldval, string newval)
		{
			var grid = bo as Grid;
			if (grid == null)
				throw new InvalidBindableException(bo, typeof(Grid));
			//Clear the old columns
			grid.ColumnDefinitions.Clear();
			var widths = newval.Split(',');
			foreach(var width in widths)
				grid.ColumnDefinitions.Add(new ColumnDefinition{Width = LengthFromString(width)});
		}

		/// <summary>
		/// Private utility function to 
		/// convert a string into a <see cref="Xamarin.Forms.GridLength"/>
		/// </summary>
		/// <param name="measure">The lenght string</param>
		/// <returns></returns>
		private static GridLength LengthFromString(string measure)
		{

			if (measure.ToLower() == "auto")
				return GridLength.Auto;
		   
			if (measure.EndsWith("*"))
			{
				var unit = measure.Replace("*", "");
				if (string.IsNullOrEmpty(unit)) unit = "1";
				double numunit;
				if (!double.TryParse(unit, out numunit))
					throw new ArgumentException(string.Format("RowHeightChanged cannot parse {0}", unit));
				return  new GridLength(numunit, GridUnitType.Star);
			}

			double unitheight;
			if (!double.TryParse(measure, out unitheight))
				throw new ArgumentException(string.Format("RowHeightChanged cannot parse {0}", measure));
			return new GridLength(unitheight, GridUnitType.Absolute);
		}

	}
}
