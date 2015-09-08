using UIKit;

using Xamarin.Forms;

using XLabs.Forms.Pages;
using ObjCRuntime;

[assembly: ExportRenderer(typeof(ExtendedMasterDetailPage), typeof(ExtendedPhoneMasterDetailPageRenderer), UIUserInterfaceIdiom.Phone)]
[assembly: ExportRenderer(typeof(ExtendedMasterDetailPage), typeof(ExtendedTabletMasterDetailPageRenderer), UIUserInterfaceIdiom.Pad)]

namespace XLabs.Forms.Pages
{
	using System;
	using System.Reflection;

	using UIKit;

	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// Class ExtendedTabletMasterDetailPageRenderer.
	/// </summary>
	public class ExtendedTabletMasterDetailPageRenderer : TabletMasterDetailRenderer
	{
		/// <summary>
		/// The should hide menu
		/// </summary>
		public static Func<bool> ShouldHideMenu;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedTabletMasterDetailPageRenderer"/> class.
		/// </summary>
		public ExtendedTabletMasterDetailPageRenderer()
		{
			var version = new Version(Constants.Version);
			if (version >= new Version(8, 0))
			{
				// Code that uses features from Xamarin.iOS 7.0
			}
			else
			{
				var fi = typeof(TabletMasterDetailRenderer).GetField("innerDelegate", BindingFlags.NonPublic | BindingFlags.Instance);
				var d = fi.GetValue(this) as UISplitViewControllerDelegate;
				Delegate = new ExtendedDelegate(d);
			}
		}

		/// <summary>
		/// Handles the <see cref="E:ElementChanged" /> event.
		/// </summary>
		/// <param name="e">The <see cref="VisualElementChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);
			var version = new Version(Constants.Version);
#if (IOS_8)
			if (version >= new Version(8, 0))
			{
				this.PreferredDisplayMode = ShouldHide ?
					UISplitViewControllerDisplayMode.PrimaryHidden :
					UISplitViewControllerDisplayMode.Automatic;
			}
#endif
		}

		/// <summary>
		/// Gets a value indicating whether [should hide].
		/// </summary>
		/// <value><c>true</c> if [should hide]; otherwise, <c>false</c>.</value>
		private static bool ShouldHide
		{
			get
			{
				return ShouldHideMenu == null || ShouldHideMenu();
			}
		}

		/// <summary>
		/// Class ExtendedDelegate.
		/// </summary>
		private class ExtendedDelegate : UISplitViewControllerDelegate
		{
			/// <summary>
			/// The button information
			/// </summary>
			private readonly PropertyInfo _buttonInfo;
			/// <summary>
			/// The base delegate
			/// </summary>
			private readonly object _baseDelegate;

			/// <summary>
			/// Initializes a new instance of the <see cref="ExtendedDelegate"/> class.
			/// </summary>
			/// <param name="baseDelegate">The base delegate.</param>
			public ExtendedDelegate(object baseDelegate)
			{
				_baseDelegate = baseDelegate;
				_buttonInfo = baseDelegate.GetType().GetProperty("PresentButton");
			}

			/// <summary>
			/// Gets or sets the present button.
			/// </summary>
			/// <value>The present button.</value>
			public UIBarButtonItem PresentButton
			{
				get
				{
					return _buttonInfo.GetValue(_baseDelegate) as UIBarButtonItem;
				}
				set
				{
					_buttonInfo.SetValue(_baseDelegate, value);
				}
			}


			/// <summary>
			/// Wills the show view controller.
			/// </summary>
			/// <param name="svc">The SVC.</param>
			/// <param name="aViewController">a view controller.</param>
			/// <param name="button">The button.</param>
			public override void WillShowViewController(UISplitViewController svc, UIViewController aViewController, UIBarButtonItem button)
			{
				PresentButton = (UIBarButtonItem)null;
			}

			/// <summary>
			/// Wills the hide view controller.
			/// </summary>
			/// <param name="svc">The SVC.</param>
			/// <param name="aViewController">a view controller.</param>
			/// <param name="barButtonItem">The bar button item.</param>
			/// <param name="pc">The pc.</param>
			public override void WillHideViewController(UISplitViewController svc, UIViewController aViewController, UIBarButtonItem barButtonItem, UIPopoverController pc)
			{
				PresentButton = barButtonItem;
			}

			/// <summary>
			/// Shoulds the hide view controller.
			/// </summary>
			/// <param name="svc">The SVC.</param>
			/// <param name="viewController">The view controller.</param>
			/// <param name="inOrientation">The in orientation.</param>
			/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
			public override bool ShouldHideViewController(UISplitViewController svc, UIViewController viewController, UIInterfaceOrientation inOrientation)
			{
				return ShouldHide;
			}
		}
	}

	public class ExtendedPhoneMasterDetailPageRenderer : PhoneMasterDetailRenderer
	{
		public ExtendedPhoneMasterDetailPageRenderer()
		{
		}
	}
}

