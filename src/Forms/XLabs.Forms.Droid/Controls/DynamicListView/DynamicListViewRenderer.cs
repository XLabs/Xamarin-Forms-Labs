// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="DynamicListViewRenderer.cs" company="XLabs Team">
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

using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ListView = Android.Widget.ListView;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class DynamicListViewRenderer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DynamicListViewRenderer<T> : ViewRenderer<DynamicListView<T>,ListView>
    {
        /// <summary>
        /// The _table view
        /// </summary>
        private ListView _tableView;

        /// <summary>
        /// The _source
        /// </summary>
        private DataSource _source;

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>The source.</value>
        private DataSource Source
        {
            get
            {
                return _source ?? (_source = new DataSource(this));
            }
        }

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<DynamicListView<T>> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                this._tableView = new ListView(this.Context);
                this.SetNativeControl(this._tableView);
            }

            this.Unbind(e.OldElement);
            this.Bind(e.NewElement);

            this._tableView.Adapter = this.Source;
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="convertView">The convert view.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>Android.Views.View.</returns>
        protected virtual Android.Views.View GetView(T item, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var v = convertView as TextView ?? new TextView(parent.Context);
            v.Text = item.ToString();
            return v;
        }

        /// <summary>
        /// Unbinds the specified old element.
        /// </summary>
        /// <param name="oldElement">The old element.</param>
        private void Unbind(DynamicListView<T> oldElement)
        {
            if (oldElement != null)
            {
                oldElement.PropertyChanging += ElementPropertyChanging;
                oldElement.PropertyChanged -= ElementPropertyChanged;
                oldElement.Data.CollectionChanged += DataCollectionChanged;
            }
        }

        /// <summary>
        /// Binds the specified new element.
        /// </summary>
        /// <param name="newElement">The new element.</param>
        private void Bind(DynamicListView<T> newElement)
        {
            if (newElement != null)
            {
                newElement.PropertyChanging += ElementPropertyChanging;
                newElement.PropertyChanged += ElementPropertyChanged;
                newElement.Data.CollectionChanged += DataCollectionChanged;
            }
        }

        /// <summary>
        /// Elements the property changing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangingEventArgs"/> instance containing the event data.</param>
        private void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == "Data")
            {
                this.Element.Data.CollectionChanged -= DataCollectionChanged;
            }
        }

        /// <summary>
        /// Datas the collection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void DataCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.Source.NotifyDataSetChanged();
        }

        /// <summary>
        /// Elements the property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Data")
            {
                this.Element.Data.CollectionChanged += DataCollectionChanged;
            }
        }

        /// <summary>
        /// Class DataSource.
        /// </summary>
        private class DataSource : BaseAdapter
        {
            /// <summary>
            /// The _parent
            /// </summary>
            DynamicListViewRenderer<T> _parent;

            /// <summary>
            /// Initializes a new instance of the <see cref="DataSource"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            public DataSource(DynamicListViewRenderer<T> parent)
            {
                this._parent = parent;
            }

            #region implemented abstract members of BaseAdapter
            /// <summary>
            /// To be added.
            /// </summary>
            /// <param name="position">To be added.</param>
            /// <returns>To be added.</returns>
            /// <remarks>To be added.</remarks>
            public override Java.Lang.Object GetItem(int position)
            {
                return position;
            }

            /// <summary>
            /// To be added.
            /// </summary>
            /// <param name="position">To be added.</param>
            /// <returns>To be added.</returns>
            /// <remarks>To be added.</remarks>
            public override long GetItemId(int position)
            {
                return position;
            }

            /// <summary>
            /// To be added.
            /// </summary>
            /// <param name="position">To be added.</param>
            /// <param name="convertView">To be added.</param>
            /// <param name="parent">To be added.</param>
            /// <returns>To be added.</returns>
            /// <remarks>To be added.</remarks>
            public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
            {
                return this._parent.GetView(this._parent.Element.Data [position], convertView, parent);
            }

            /// <summary>
            /// To be added.
            /// </summary>
            /// <value>To be added.</value>
            /// <remarks>To be added.</remarks>
            public override int Count
            {
                get
                {
                    return this._parent.Element.Data.Count;
                }
            }
            #endregion
        }
    }
}

