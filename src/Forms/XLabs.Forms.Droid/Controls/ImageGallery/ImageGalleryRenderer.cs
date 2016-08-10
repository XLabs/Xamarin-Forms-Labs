// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ImageGalleryRenderer.cs" company="XLabs Team">
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

using System.Collections.Specialized;
using System.Linq;
using System.Net;
using Android.Graphics;
using Android.Webkit;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ImageGallery), typeof(ImageGalleryRenderer))]
namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ImageGalleryRenderer.
	/// </summary>
	public class ImageGalleryRenderer : ViewRenderer<ImageGallery,Gallery>
	{

		/// <summary>
		/// The gallery
		/// </summary>
		private Gallery _gallery;
		/// <summary>
		/// The source
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
		protected override void OnElementChanged(ElementChangedEventArgs<ImageGallery> e)
		{
			base.OnElementChanged(e);

			if(e.OldElement == null)
			{
				_gallery = new Gallery(Context);
				SetNativeControl(_gallery);
			}
			Bind(e.NewElement);
			_gallery.Adapter = Source;
		}

		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="convertView">The convert view.</param>
		/// <param name="parent">The parent.</param>
		/// <param name="position">The position.</param>
		/// <returns>Android.Views.View.</returns>
		protected virtual Android.Views.View GetView(string item, Android.Views.View convertView, Android.Views.ViewGroup parent, int position)
		{
			var imageView = convertView as ImageView ?? new ImageView(parent.Context);

			if (IsValidUrl(item))
				imageView.SetImageBitmap(GetBitmapFromUrl(item));
			else
				imageView.SetImageResource(Resources.GetIdentifier(System.IO.Path.GetFileNameWithoutExtension(item), "drawable", Context.PackageName));

			imageView.SetScaleType(ImageView.ScaleType.FitXy);
			return imageView;
		}


		/// <summary>
		/// Binds the specified new element.
		/// </summary>
		/// <param name="newElement">The new element.</param>
		private void Bind(ImageGallery newElement)
		{
			if (newElement != null)
			{
				newElement.PropertyChanging += ElementPropertyChanging;
				newElement.PropertyChanged += ElementPropertyChanged;
				((INotifyCollectionChanged)newElement.ItemsSource).CollectionChanged += DataCollectionChanged;
			}
		}

		/// <summary>
		/// Elements the property changing.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangingEventArgs"/> instance containing the event data.</param>
		private void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == "ItemsSource")
				((INotifyCollectionChanged)Element.ItemsSource).CollectionChanged -= DataCollectionChanged;
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
			if (e.PropertyName == "ItemsSource")
				((INotifyCollectionChanged)Element.ItemsSource).CollectionChanged += DataCollectionChanged;
		}

		/// <summary>
		/// Determines whether [is valid URL] [the specified URL string].
		/// </summary>
		/// <param name="urlString">The URL string.</param>
		/// <returns><c>true</c> if [is valid URL] [the specified URL string]; otherwise, <c>false</c>.</returns>
		private bool IsValidUrl(string urlString)
		{
			return URLUtil.IsValidUrl(urlString);
		}

		/// <summary>
		/// Gets the bitmap from URL.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns>Bitmap.</returns>
		private Bitmap GetBitmapFromUrl(string url)
		{
			Bitmap imageBitmap = null;

			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
			}

			return imageBitmap;
		}

		/// <summary>
		/// Class DataSource.
		/// </summary>
		private class DataSource : BaseAdapter
		{
			/// <summary>
			/// The gallery renderer
			/// </summary>
			ImageGalleryRenderer _galleryRenderer;

			/// <summary>
			/// Initializes a new instance of the <see cref="DataSource"/> class.
			/// </summary>
			/// <param name="galleryRenderer">The gallery renderer.</param>
			public DataSource(ImageGalleryRenderer galleryRenderer)
			{
				this._galleryRenderer = galleryRenderer;
			}

			#region abstract members of BaseAdapter
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
				return _galleryRenderer.GetView(_galleryRenderer.Element.ItemsSource.Cast<string>().ToArray()[position], convertView, parent, position);
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
					return _galleryRenderer.Element.ItemsSource.Cast<string>().Count();
				}
			}
			#endregion
		}
	}
}