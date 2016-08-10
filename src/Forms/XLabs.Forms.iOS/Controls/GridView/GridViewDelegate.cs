// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="GridViewDelegate.cs" company="XLabs Team">
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

using Foundation;
using UIKit;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class GridViewDelegate.
	/// </summary>
	public class GridViewDelegate: UICollectionViewDelegate
	{
		/// <summary>
		/// Delegate OnItemSelected
		/// </summary>
		/// <param name="tableView">The table view.</param>
		/// <param name="indexPath">The index path.</param>
		public delegate void OnItemSelected (UICollectionView tableView, NSIndexPath indexPath);

		/// <summary>
		/// The _on item selected
		/// </summary>
		private readonly OnItemSelected _onItemSelected;

		/// <summary>
		/// Initializes a new instance of the <see cref="GridViewDelegate"/> class.
		/// </summary>
		/// <param name="onItemSelected">The on item selected.</param>
		public GridViewDelegate (OnItemSelected onItemSelected)
		{
		    this._onItemSelected = onItemSelected;
		}

		/// <summary>
		/// Items the selected.
		/// </summary>
		/// <param name="collectionView">The collection view.</param>
		/// <param name="indexPath">The index path.</param>
		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
		{
		    this._onItemSelected (collectionView, indexPath);
		}

		/// <summary>
		/// Items the highlighted.
		/// </summary>
		/// <param name="collectionView">The collection view.</param>
		/// <param name="indexPath">The index path.</param>
		public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
		{
		    this._onItemSelected.Invoke(collectionView, indexPath);
		}
	}
}

