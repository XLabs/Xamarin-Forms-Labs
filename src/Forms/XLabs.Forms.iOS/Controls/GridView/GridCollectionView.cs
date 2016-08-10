// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="GridCollectionView.cs" company="XLabs Team">
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
    /// Class GridCollectionView.
    /// </summary>
    public class GridCollectionView : UICollectionView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridCollectionView"/> class.
        /// </summary>
        public GridCollectionView () : this (default(CGRect))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridCollectionView"/> class.
        /// </summary>
        /// <param name="frm">The FRM.</param>
        public GridCollectionView(CGRect frm)
            : base(frm, new UICollectionViewFlowLayout())
        {
            AutoresizingMask = UIViewAutoresizing.All;
            ContentMode = UIViewContentMode.ScaleToFill;
            RegisterClassForCell(typeof(GridViewCell), new NSString (GridViewCell.Key));
        }

        /// <summary>
        /// Gets or sets a value indicating whether [selection enable].
        /// </summary>
        /// <value><c>true</c> if [selection enable]; otherwise, <c>false</c>.</value>
        public bool SelectionEnable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the row spacing.
        /// </summary>
        /// <value>The row spacing.</value>
        public double RowSpacing
        {
            get
            {
                return ((UICollectionViewFlowLayout)this.CollectionViewLayout).MinimumLineSpacing;
            }
            set
            {
                ((UICollectionViewFlowLayout)this.CollectionViewLayout).MinimumLineSpacing = (nfloat)value;
            }
        }

        /// <summary>
        /// Gets or sets the column spacing.
        /// </summary>
        /// <value>The column spacing.</value>
        public double ColumnSpacing
        {
            get
            {
                return ((UICollectionViewFlowLayout)this.CollectionViewLayout).MinimumInteritemSpacing;
            }
            set
            {
                ((UICollectionViewFlowLayout)this.CollectionViewLayout).MinimumInteritemSpacing = (nfloat)value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the item.
        /// </summary>
        /// <value>The size of the item.</value>
        public CGSize ItemSize
        {
            get
            {
                return ((UICollectionViewFlowLayout)this.CollectionViewLayout).ItemSize;
            }
            set
            {
                ((UICollectionViewFlowLayout)this.CollectionViewLayout).ItemSize = value;
            }
        }

        /// <summary>
        /// Cells for item.
        /// </summary>
        /// <param name="indexPath">The index path.</param>
        /// <returns>UICollectionViewCell.</returns>
        public override UICollectionViewCell CellForItem(NSIndexPath indexPath)
        {
            if (indexPath == null)
            {
                //calling base.CellForItem(indexPath) when indexPath is null causes an exception.
                //indexPath could be null in the following scenario:
                // - GridView is configured to show 2 cells per row and there are 3 items in ItemsSource collection
                // - you're trying to drag 4th cell (empty) like you're trying to scroll
                return null;
            }
            return base.CellForItem(indexPath);
        }

        /// <summary>
        /// Draws the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public override void Draw (CGRect rect)
        {
            this.CollectionViewLayout.InvalidateLayout ();

            base.Draw (rect);
        }

        public override CGSize SizeThatFits(CGSize size)
        {
            return ItemSize;
        }
    }
}

