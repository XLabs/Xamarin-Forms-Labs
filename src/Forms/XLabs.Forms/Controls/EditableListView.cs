// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="EditableListView.cs" company="XLabs Team">
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
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class EditableListView.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EditableListView<T> : View
	{
		/// <summary>
		/// The source property
		/// </summary>
		public static readonly BindableProperty SourceProperty = BindableProperty.Create<EditableListView<T>, ObservableCollection<T>>(p => p.Source, default(ObservableCollection<T>));
		/// <summary>
		/// Gets or sets the source.
		/// </summary>
		/// <value>The source.</value>
		public ObservableCollection<T> Source
		{
			get { return (ObservableCollection<T>)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		/// <summary>
		/// The add row command property
		/// </summary>
		public static readonly BindableProperty AddRowCommandProperty = BindableProperty.Create<EditableListView<T>, Command>(p => p.AddRowCommand, default(Command));
		/// <summary>
		/// Gets or sets the add row command.
		/// </summary>
		/// <value>The add row command.</value>
		public Command AddRowCommand
		{
			get { return (Command)GetValue(AddRowCommandProperty); }
			set { SetValue(AddRowCommandProperty, value); }
		}

		/// <summary>
		/// Gets or sets the height of the cell.
		/// </summary>
		/// <value>The height of the cell.</value>
		public float CellHeight { get; set; }
		/// <summary>
		/// Gets or sets the type of the view.
		/// </summary>
		/// <value>The type of the view.</value>
		public Type ViewType { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="EditableListView{T}"/> class.
		/// </summary>
		public EditableListView ()
		{
		}

		/// <summary>
		/// Executes the add row.
		/// </summary>
		public void ExecuteAddRow()
		{
			var addRowCommand = AddRowCommand;
			if (addRowCommand != null)
				addRowCommand.Execute(null);
		}
	}
}

