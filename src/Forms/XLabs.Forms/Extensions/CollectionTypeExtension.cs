// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CollectionTypeExtension.cs" company="XLabs Team">
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XLabs.Forms
{
	/// <summary>
	/// Markup extension making it simpler to declare generic list types
	/// </summary>
	/// Element created at 09/11/2014,9:33 AM by Charles
	[ContentProperty("InstanceType")]
	public class CollectionTypeExtension : IMarkupExtension
	{
		/// <summary>Gets or sets the collection.</summary>
		/// <value>The Type of collection type to return.</value>
		/// Element created at 09/11/2014,9:35 AM by Charles
		/// 
		public CollectionTypes Collection { get; set; }
		/// <summary>Gets or sets the type of the instance.</summary>
		/// <value>The type to be contained in the collection type</value>
		/// Element created at 09/11/2014,9:35 AM by Charles
		public string InstanceType { get; set; }


		/// <summary>
		/// Initializes a new instance of the <see cref="CollectionTypeExtension"/> class 
		/// to return a ObservableCollection(string)
		/// </summary>
		/// Element created at 09/11/2014,10:03 AM by Charles
		public CollectionTypeExtension()
		{
			Collection=CollectionTypes.ObservableCollection;
			InstanceType = "string";
		}
		/// <summary>Returns the object created from the markup extension.</summary>
		/// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
		/// <returns>The Type of the desired collection</returns>
		/// Element created at 09/11/2014,9:35 AM by Charles
		/// <exception cref="System.NotImplementedException"></exception>
		public object ProvideValue(IServiceProvider serviceProvider)
		{
			////This markup extension is so simple we don't need the service provider...
			Type genericType;
			switch (Collection)
			{
				case CollectionTypes.GenericList:
					genericType = typeof(List<>);
					break;
				default:
					genericType = typeof(ObservableCollection<>);
					break;
			}

			var typeparams = new[] { Type.GetType(InstanceType) };
			return genericType.MakeGenericType(typeparams);
		}
	}

	/// <summary>The collection type to return</summary>
	/// Element created at 09/11/2014,9:31 AM by Charles
	public enum CollectionTypes
	{
		/// <summary>None, Invalid</summary>
		/// Element created at 09/11/2014,9:31 AM by Charles
		None=0,

		/// <summary>
		/// The observable collection type ObservableCollection(T)
		/// </summary>
		/// Element created at 09/11/2014,9:31 AM by Charles
		ObservableCollection,

		/// <summary>The Generic list type List(T)</summary>
		/// Element created at 09/11/2014,9:32 AM by Charles
		GenericList

	}
}
