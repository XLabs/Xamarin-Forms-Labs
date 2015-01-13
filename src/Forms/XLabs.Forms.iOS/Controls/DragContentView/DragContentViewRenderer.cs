using System;
using System.Collections.Generic;
using CoreGraphics;
using System.Linq;
using System.Reflection;
using System.Text;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(DragContentView), typeof(DragContentViewRenderer))]

namespace XLabs.Forms.Controls
{
	public class DragContentViewRenderer : ViewRenderer<DragContentView, UIView>
	{
		private UIView touchedView;
		private View touchedElement;
		private CGPoint offsetLocation;
		private CGPoint homePosition;

		protected override void OnElementChanged(ElementChangedEventArgs<DragContentView> e)
		{
			base.OnElementChanged(e);

			if (this.Control == null)
			{
				this.SetNativeControl(new UIView());
			}

			if (e.NewElement == null)
			{
				this.touchedView = null;
				this.touchedElement = null;
			}
		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

			var t = touches.ToArray<UITouch>();

			if (t.Length == 1)
			{
				var loc = t[0].LocationInView(this);

				this.touchedView = this.HitTest(loc, evt);

				if (this.touchedView != null)
				{
					this.touchedElement = GetMovedElement(this.touchedView, this.Element.Content);
					this.homePosition = this.touchedView.Frame.Location;
					this.offsetLocation = new CGPoint(loc.X - this.touchedView.Frame.X, loc.Y - this.touchedView.Frame.Y);

					this.BringSubviewToFront(this.touchedView);
				}
			}
		}

		public override void TouchesCancelled(NSSet touches, UIEvent evt)
		{
			base.TouchesCancelled(touches, evt);
			this.touchedView = null;
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			base.TouchesEnded(touches, evt);

			this.touchedView = null;
		}

		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			base.TouchesMoved(touches, evt);

			var t = touches.ToArray<UITouch>();

			if (this.touchedView != null)
			{
				var newLoc = t[0].LocationInView(this);

				var frame = new CGRect(
					new CGPoint(newLoc.X - this.offsetLocation.X, newLoc.Y - this.offsetLocation.Y),
					this.touchedView.Frame.Size);

				if (this.touchedElement != null)
				{
					this.touchedElement.Layout(frame.ToRectangle());
				}
				//else
				//{
				//    this.touchedView.Frame = frame;
				//}
			}
		}

		private static View GetMovedElement(object nativeView, View view)
		{
			View movedElement = null;

			var id = GetAccessibilityId(nativeView);

			if (string.IsNullOrWhiteSpace(id))
			{
				movedElement = null;
			}
			else if (view.StyleId == id)
			{
				movedElement = view;
			}
			else
			{
				var d = view.GetInternalChildren();

				movedElement = d == null ? null : d.OfType<View>().FirstOrDefault(a => a.StyleId == id);
			}

			return movedElement;
		}

		
#if __IOS_10__
		private static string GetAccessibilityId(object view)
		{
			var ni = view.GetType().GetProperty("Control", BindingFlags.Public | BindingFlags.Instance);
			if (ni == null)
			{
				return string.Empty;
			}

			var control = ni.GetValue(view) as UIView;
			if (control == null)
			{
				return string.Empty;
			}

			return control.AccessibilityIdentifier;
		}
#else
		private static string GetAccessibilityId(object view)
		{
			throw new NotImplementedException("IOS 10 Not Supported");
		}
#endif
	}
}