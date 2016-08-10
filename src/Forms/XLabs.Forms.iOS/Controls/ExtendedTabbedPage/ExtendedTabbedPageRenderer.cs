// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedTabbedPageRenderer.cs" company="XLabs Team">
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

using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedTabbedPage), typeof(ExtendedTabbedPageRenderer))]
namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ExtendedTabbedPageRenderer.
	/// </summary>
	public class ExtendedTabbedPageRenderer : TabbedRenderer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedTabbedPageRenderer"/> class.
		/// </summary>
		public ExtendedTabbedPageRenderer()
		{
			
		}

		/// <summary>
		/// Handles the <see cref="E:ElementChanged" /> event.
		/// </summary>
		/// <param name="e">The <see cref="VisualElementChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			var page = (ExtendedTabbedPage)Element;

			TabBar.TintColor = page.TintColor.ToUIColor();
			TabBar.BarTintColor = page.BarTintColor.ToUIColor();
			TabBar.BackgroundColor = page.BackgroundColor.ToUIColor();
			

			if (!page.SwipeEnabled)
			{
				return;
			}

			var gesture1 = new UISwipeGestureRecognizer(sw =>
			{
				sw.ShouldReceiveTouch += (recognizer, touch) => !(touch.View is UITableView) && !(touch.View is UITableViewCell);

				if (sw.Direction == UISwipeGestureRecognizerDirection.Right)
				{
					page.InvokeSwipeLeftEvent(null, null);
				}

			}) { Direction = UISwipeGestureRecognizerDirection.Right };

			var gesture2 = new UISwipeGestureRecognizer(sw =>
			{
				sw.ShouldReceiveTouch += (recognizer, touch) => !(touch.View is UITableView) && !(touch.View is UITableViewCell);

				if (sw.Direction == UISwipeGestureRecognizerDirection.Left)
				{
					page.InvokeSwipeRightEvent(null, null);
				}

			}) { Direction = UISwipeGestureRecognizerDirection.Left };

			View.AddGestureRecognizer(gesture1);
			View.AddGestureRecognizer(gesture2);
		}

		/// <summary>
		/// Views the did load.
		/// </summary>
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

		}

		/// <summary>
		/// Views the will appear.
		/// </summary>
		/// <param name="animated">if set to <c>true</c> [animated].</param>
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			var page = (ExtendedTabbedPage)Element;

			if (!string.IsNullOrEmpty(page.TabBarSelectedImage))
			{
				TabBar.SelectionIndicatorImage = UIImage.FromFile(page.TabBarSelectedImage).CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0), UIImageResizingMode.Stretch);
			}

			if (!string.IsNullOrEmpty(page.TabBarBackgroundImage))
			{
				TabBar.BackgroundImage = UIImage.FromFile(page.TabBarBackgroundImage).CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0), UIImageResizingMode.Stretch);
			}

			if (page.Badges != null && page.Badges.Count != 0)
			{
				var items = TabBar.Items;

				for (var i = 0; i < page.Badges.Count; i++)
				{
					if (i >= items.Count())
					{
						continue;
					}

					items[i].BadgeValue = page.Badges[i];
				}
			}
		}
	}
}
