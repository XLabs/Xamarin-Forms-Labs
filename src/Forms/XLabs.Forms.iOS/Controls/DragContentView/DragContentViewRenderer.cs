// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="DragContentViewRenderer.cs" company="XLabs Team">
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
using System.Reflection;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(DragContentView), typeof(DragContentViewRenderer))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class DragContentViewRenderer.
    /// </summary>
    public class DragContentViewRenderer : ViewRenderer<DragContentView, UIView>
    {
        /// <summary>
        /// The touched view
        /// </summary>
        private UIView touchedView;
        /// <summary>
        /// The touched element
        /// </summary>
        private View touchedElement;
        /// <summary>
        /// The offset location
        /// </summary>
        private CGPoint offsetLocation;

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
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

        /// <summary>
        /// Toucheses the began.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The evt.</param>
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            var t = touches.ToArray<UITouch>();

            if (t.Length != 1) return;
            var loc = t[0].LocationInView(this);

            this.touchedView = this.Subviews[0].HitTest(loc, evt);

            if (this.touchedView == null) return;

            this.touchedElement = GetMovedElement(this.touchedView, this.Element.Content);
            this.offsetLocation = new CGPoint(loc.X - this.touchedView.Frame.X, loc.Y - this.touchedView.Frame.Y);

            this.BringSubviewToFront(this.touchedView);
        }

        /// <summary>
        /// Toucheses the cancelled.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The evt.</param>
        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);
            this.touchedView = null;
        }

        /// <summary>
        /// Toucheses the ended.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The evt.</param>
        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            this.touchedView = null;
        }

        /// <summary>
        /// Toucheses the moved.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The evt.</param>
        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            if (this.touchedView == null) return;

            var newLoc = ((UITouch)touches.First()).LocationInView(this);

            var frame = new CGRect(
                new CGPoint(newLoc.X - this.offsetLocation.X, newLoc.Y - this.offsetLocation.Y),
                this.touchedView.Frame.Size);

            if (this.touchedElement != null)
            {
                this.touchedElement.Layout(frame.ToRectangle());
            }
        }

        /// <summary>
        /// Gets the moved element.
        /// </summary>
        /// <param name="nativeView">The native view.</param>
        /// <param name="view">The view.</param>
        /// <returns>View.</returns>
        private static View GetMovedElement(object nativeView, View view)
        {
            View movedElement;

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

        /// <summary>
        /// Gets the accessibility identifier.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>System.String.</returns>
        private static string GetAccessibilityId(object view)
        {
            var ni = view.GetType().GetProperty("Control", BindingFlags.Public | BindingFlags.Instance);
            if (ni == null)
            {
                return string.Empty;
            }

            var control = ni.GetValue(view) as UIView;
            return control == null ? string.Empty : control.AccessibilityIdentifier;
        }
    }
}