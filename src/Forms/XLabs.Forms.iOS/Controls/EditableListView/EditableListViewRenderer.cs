// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="EditableListViewRenderer.cs" company="XLabs Team">
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
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(EditableListView<Object>), typeof(EditableListViewRenderer<Object>))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class EditableListViewRenderer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EditableListViewRenderer<T> : ViewRenderer<EditableListView<T>, UITableView>
    {
        /// <summary>
        /// The row height
        /// </summary>
        protected float RowHeight = 44;
        /// <summary>
        /// The _editable ListView source
        /// </summary>
        private EditableListViewSource _editableListViewSource;
        /// <summary>
        /// The _table view
        /// </summary>
        private UITableView _tableView;

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<EditableListView<T>> e)
        {
            base.OnElementChanged(e);

            Unbind (e.OldElement);
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    _tableView = new UITableView (new CGRect (0, 0, 1, 1), UITableViewStyle.Plain);
                    _editableListViewSource = new EditableListViewSource (this);
                    _tableView.Source = _editableListViewSource;

                    _tableView.SetEditing (true, true);
                    _tableView.TableFooterView = new UIView ();
                    SetNativeControl (_tableView);
                    if (e.NewElement.CellHeight > 0)
                        RowHeight = e.NewElement.CellHeight;

                }

                Bind (e.NewElement);
            }
        }

        /// <summary>
        /// Unbinds the specified old element.
        /// </summary>
        /// <param name="oldElement">The old element.</param>
        private void Unbind(EditableListView<T> oldElement)
        {
            if (oldElement != null)
            {
                oldElement.PropertyChanged -= ElementPropertyChanged;
                oldElement.Source.CollectionChanged -= DataCollectionChanged;
            }
        }

        /// <summary>
        /// Binds the specified new element.
        /// </summary>
        /// <param name="newElement">The new element.</param>
        private void Bind(EditableListView<T> newElement)
        {
            if (newElement != null)
            {
                newElement.PropertyChanged += ElementPropertyChanged;
                newElement.Source.CollectionChanged += DataCollectionChanged;
            }
        }

        /// <summary>
        /// Datas the collection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void DataCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _tableView.ReloadData();
        }

        /// <summary>
        /// Elements the property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Source")
            {
                if(Element.Source != null)
                    Element.Source.CollectionChanged += DataCollectionChanged;
            }
        }
        bool disposed;
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && !disposed)
            {
                Unbind (Element);
                _editableListViewSource.Dispose ();
                _editableListViewSource = null;
                disposed = true;
            }
            base.Dispose (disposing);
        }

        /// <summary>
        /// Class EditableListViewSource.
        /// </summary>
        private class EditableListViewSource : UITableViewSource 
        {
            /// <summary>
            /// The _container renderer
            /// </summary>
            EditableListViewRenderer<T> _containerRenderer;

            /// <summary>
            /// Initializes a new instance of the <see cref="EditableListViewSource"/> class.
            /// </summary>
            /// <param name="containerRenderer">The container renderer.</param>
            public EditableListViewSource(EditableListViewRenderer<T> containerRenderer)
            {
                _containerRenderer = containerRenderer;
            }

            /// <summary>
            /// Gets the cell.
            /// </summary>
            /// <param name="tableView">The table view.</param>
            /// <param name="indexPath">The index path.</param>
            /// <returns>UITableViewCell.</returns>
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                if (indexPath.Row == _containerRenderer.Element.Source.Count)
                {
                    var addCell = new UITableViewCell();
                    addCell.TextLabel.Text = "Add ...";
                    return addCell;
                }

                var item = _containerRenderer.Element.Source[indexPath.Row];

                var view = Activator.CreateInstance(_containerRenderer.Element.ViewType) as View;

                view.BindingContext = item;
                var viewCell = new ViewCell {View = view};
                return new ViewCellRenderer().GetCell(viewCell, null, tableView);
            }

            /// <summary>
            /// Commits the editing style.
            /// </summary>
            /// <param name="tableView">The table view.</param>
            /// <param name="editingStyle">The editing style.</param>
            /// <param name="indexPath">The index path.</param>
            public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
            {
                if (editingStyle == UITableViewCellEditingStyle.Delete)
                {
                    _containerRenderer.Element.Source.RemoveAt(indexPath.Row);
                    tableView.ReloadData();
                }
                else if (editingStyle == UITableViewCellEditingStyle.Insert)
                {
                    _containerRenderer.Element.ExecuteAddRow();
                }
            }

            /// <summary>
            /// Editings the style for row.
            /// </summary>
            /// <param name="tableView">The table view.</param>
            /// <param name="indexPath">The index path.</param>
            /// <returns>UITableViewCellEditingStyle.</returns>
            public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return indexPath.Row == _containerRenderer.Element.Source.Count ? 
                    UITableViewCellEditingStyle.Insert : 
                    UITableViewCellEditingStyle.Delete;
            }

            /// <summary>
            /// Rowses the in section.
            /// </summary>
            /// <param name="tableView">The table view.</param>
            /// <param name="section">The section.</param>
            /// <returns>System.Int32.</returns>
            public override nint RowsInSection(UITableView tableView, nint section)
            {
                return _containerRenderer.Element.Source.Count + 1;
            }

            /// <summary>
            /// Determines whether this instance [can move row] the specified table view.
            /// </summary>
            /// <param name="tableView">The table view.</param>
            /// <param name="indexPath">The index path.</param>
            /// <returns><c>true</c> if this instance [can move row] the specified table view; otherwise, <c>false</c>.</returns>
            public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
            {
                return indexPath.Row != _containerRenderer.Element.Source.Count;
            }

            /// <summary>
            /// Customizes the move target.
            /// </summary>
            /// <param name="tableView">The table view.</param>
            /// <param name="sourceIndexPath">The source index path.</param>
            /// <param name="proposedIndexPath">The proposed index path.</param>
            /// <returns>NSIndexPath.</returns>
            public override NSIndexPath CustomizeMoveTarget(UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath proposedIndexPath)
            {
                return proposedIndexPath.Row == _containerRenderer.Element.Source.Count ? 
                    sourceIndexPath : 
                    proposedIndexPath;
            }

            /// <summary>
            /// Moves the row.
            /// </summary>
            /// <param name="tableView">The table view.</param>
            /// <param name="sourceIndexPath">The source index path.</param>
            /// <param name="destinationIndexPath">The destination index path.</param>
            public override void MoveRow(UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
            {
                _containerRenderer.Element.Source.Move(sourceIndexPath.Row, destinationIndexPath.Row);
            }

            /// <summary>
            /// Gets the height for row.
            /// </summary>
            /// <param name="tableView">The table view.</param>
            /// <param name="indexPath">The index path.</param>
            /// <returns>System.Single.</returns>
            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return _containerRenderer.RowHeight;
            }

            /// <summary>
            /// Rows the selected.
            /// </summary>
            /// <param name="tableView">The table view.</param>
            /// <param name="indexPath">The index path.</param>
            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {

            }
        }
    }
}