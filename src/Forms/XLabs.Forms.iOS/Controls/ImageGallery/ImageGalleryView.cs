// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ImageGalleryView.cs" company="XLabs Team">
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
using System.Collections.Specialized;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ImageGalleryView.
	/// </summary>
	public class ImageGalleryView : UIView
	{
		/// <summary>
		/// The _page control
		/// </summary>
		private readonly UIPageControl _pageControl;

		/// <summary>
		/// The _scroller
		/// </summary>
		private readonly UIScrollView _scroller;

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageGalleryView"/> class.
		/// </summary>
		/// <param name="images">The images.</param>
		public ImageGalleryView(ObservableCollection<string> images)
			: this(default(CGRect), images)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageGalleryView"/> class.
		/// </summary>
		/// <param name="frame">The frame.</param>
		/// <param name="images">The images.</param>
		public ImageGalleryView(CGRect frame, ObservableCollection<string> images = null)
			: base(frame)
		{
			AutoresizingMask = UIViewAutoresizing.All;
			ContentMode = UIViewContentMode.ScaleToFill;
			
			FadeImages = true;
			
			BackgroundColor = UIColor.White;
			
			Frame = frame == default(CGRect) ? UIScreen.MainScreen.Bounds : frame;

			Images = images ?? new ObservableCollection<string>();

			_pageControl = new UIPageControl
				               {
					               AutoresizingMask = UIViewAutoresizing.All,
					               ContentMode = UIViewContentMode.ScaleToFill
				               };

			_pageControl.ValueChanged += (object sender, EventArgs e) => UpdateScrollPositionBasedOnPageControl();

			_scroller = new UIScrollView
				            {
					            AutoresizingMask = UIViewAutoresizing.All,
					            ContentMode = UIViewContentMode.ScaleToFill,
					            PagingEnabled = true,
					            Bounces = false,
					            ShowsHorizontalScrollIndicator = false,
					            ShowsVerticalScrollIndicator = false
				            };

			Add(_scroller);
			Add(_pageControl);
		}

		/// <summary>
		/// Gets or sets a value indicating whether [fade images].
		/// </summary>
		/// <value><c>true</c> if [fade images]; otherwise, <c>false</c>.</value>
		public bool FadeImages { get; set; }

		/// <summary>
		/// Gets or sets the images.
		/// </summary>
		/// <value>The images.</value>
		public ObservableCollection<string> Images { get; set; }

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			_scroller.Scrolled -= ScrollChanged;
			Images.CollectionChanged -= HandleCollectionChanged;
			Images.Clear();
		}

		/// <summary>
		/// Draws the specified rect.
		/// </summary>
		/// <param name="rect">The rect.</param>
		public override void Draw(CGRect rect)
		{
			Images.CollectionChanged += HandleCollectionChanged;
			_scroller.Scrolled += ScrollChanged;

			//TODO: need to remove observer if using the lambda?
			NSNotificationCenter.DefaultCenter.AddObserver(
				UIApplication.DidChangeStatusBarOrientationNotification,
				not =>
					{
						var orientation = UIDevice.CurrentDevice.Orientation;
						if ((UIDeviceOrientation.LandscapeLeft == orientation || UIDeviceOrientation.LandscapeRight == orientation))
						{
							_scroller.ContentSize = new CGSize(Frame.Height * Images.Count - 1, Frame.Width);
						}
						else
						{
							_scroller.ContentSize = new CGSize(rect.Width * Images.Count - 1, rect.Height);
						}
						UpdateScrollPositionBasedOnPageControl();
					});
			_pageControl.Frame = new CGRect(rect.Left, rect.Height - 40, rect.Width, 40);
			_scroller.Frame = new CGRect(rect.Left, rect.Top, rect.Width, rect.Height);
			
			var curr = 0;
			foreach (var im in  Images)
			{
				try
				{
					AddImage(rect, curr, im);
					curr++;
				}
				catch (Exception)
				{
					// ignored
				}
			}
			_scroller.ContentSize = new CGSize(_scroller.Frame.Width * curr - 1, _scroller.Frame.Height);
			_pageControl.Pages = curr;

			base.Draw(rect);
		}

		/// <summary>
		/// Adds the image.
		/// </summary>
		/// <param name="rect">The rect.</param>
		/// <param name="position">The position.</param>
		/// <param name="im">The im.</param>
		private void AddImage(CGRect rect, nint position, string im)
		{
			var img = new UIImage();
			var isRemote = Helpers.IsValidUrl(im);
			
			if (isRemote)
			{
				//dont await , fire and forget
				LoadImageAsync(position, im);
			}
			else
			{
				img = UIImage.FromFile(im);
			}
			
			var imgView = new UIImageView(img)
				              {
					              AutoresizingMask = UIViewAutoresizing.All,
					              ContentMode = UIViewContentMode.ScaleToFill
				              };
			
			if (FadeImages)
			{
				imgView.Alpha = 0;
			}
			
			//if first image is local, fade it in
			if (position == 0 && !isRemote)
			{
				FadeImageViewIn(imgView);
			}
			
			imgView.Frame = new CGRect(rect.Width * position, rect.Top, rect.Width, rect.Height);
			_scroller.AddSubview(imgView);
		}

		/// <summary>
		/// Loads the image asynchronous.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="url">The URL.</param>
		/// <returns>Task.</returns>
		private Task LoadImageAsync(nint position, string url)
		{
			return Task.Run(
				() =>
					{
						var img = Helpers.LoadFromUrl(url);

						InvokeOnMainThread(
							() =>
								{
									var imgView = _scroller.Subviews[position] as UIImageView;
									if (_pageControl.CurrentPage == position && FadeImages)
									{
										FadeImageViewIn(imgView, img);
									}
									else
									{
										imgView.Image = img;
									}
								});
					});
		}

		/// <summary>
		/// Scrolls the changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void ScrollChanged(object sender, EventArgs e)
		{
			var pageWidth = double.Parse(_scroller.Bounds.Width.ToString());
			var oof = double.Parse(_scroller.ContentOffset.X.ToString());
			var pageNumber = int.Parse((Math.Floor((oof - pageWidth / 2) / pageWidth) + 1).ToString());
			var imgView = _scroller.Subviews[pageNumber] as UIImageView;
			FadeImageViewIn(imgView);
			_pageControl.CurrentPage = pageNumber;
		}

		/// <summary>
		/// Handles the collection changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
		private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (var newImage in e.NewItems)
				{
					try
					{
						BeginInvokeOnMainThread(
							() =>
								{
								AddImage(Frame, _pageControl.Pages, newImage as string);
									_scroller.ContentSize = new CGSize(Frame.Width * (_pageControl.Pages + 1), _scroller.Frame.Height);
									_pageControl.Pages = _pageControl.Pages + 1;
								});
					}
					catch (Exception)
					{
					}
				}
			}
		}

		/// <summary>
		/// Sets the image.
		/// </summary>
		/// <param name="imgView">The img view.</param>
		/// <param name="img">The img.</param>
		private void SetImage(UIImageView imgView, UIImage img)
		{
			if (img != null)
			{
				imgView.Image = img;
			}
			imgView.Alpha = 1;
		}

		/// <summary>
		/// Updates the scroll position based on page control.
		/// </summary>
		private void UpdateScrollPositionBasedOnPageControl()
		{
			var off = _pageControl.CurrentPage * _scroller.Frame.Width;
			_scroller.SetContentOffset(new CGPoint(off, 0), true);
		}

		/// <summary>
		/// Fades the image view in.
		/// </summary>
		/// <param name="imgView">The img view.</param>
		/// <param name="img">The img.</param>
		private void FadeImageViewIn(UIImageView imgView, UIImage img = null)
		{
			if (FadeImages)
			{
				Animate(0.3, 0, UIViewAnimationOptions.TransitionCrossDissolve, () => { SetImage(imgView, img); }, () => { });
			}
			else
			{
				SetImage(imgView, img);
			}
		}
	}

	/// <summary>
	/// Class Helpers.
	/// </summary>
	public class Helpers
	{
		/// <summary>
		/// Determines whether [is valid URL] [the specified URL string].
		/// </summary>
		/// <param name="urlString">The URL string.</param>
		/// <returns><c>true</c> if [is valid URL] [the specified URL string]; otherwise, <c>false</c>.</returns>
		public static bool IsValidUrl(string urlString)
		{
			Uri uri;
			return Uri.TryCreate(urlString, UriKind.Absolute, out uri)
			       && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeFtp
			           || uri.Scheme == Uri.UriSchemeMailto);
		}

		/// <summary>
		/// Loads from URL.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <returns>UIImage.</returns>
		public static UIImage LoadFromUrl(string uri)
		{
			using (var url = new NSUrl(uri)) using (var data = NSData.FromUrl(url)) return UIImage.LoadFromData(data);
		}
	}
}