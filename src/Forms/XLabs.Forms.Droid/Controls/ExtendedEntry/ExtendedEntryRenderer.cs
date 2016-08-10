// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedEntryRenderer.cs" company="XLabs Team">
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
using System.ComponentModel;
using Android.Graphics;
using Android.Text;
using Android.Text.Method;
using Android.Util;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls;
using XLabs.Forms.Extensions;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class ExtendedEntryRenderer.
    /// </summary>
    public class ExtendedEntryRenderer : EntryRenderer
    {
        private const int MinDistance = 10;

        private float downX, downY, upX, upY;

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var view = (ExtendedEntry)Element;
            
            if (Control != null && e.NewElement != null && e.NewElement.IsPassword)
            {
                Control.SetTypeface(Typeface.Default, TypefaceStyle.Normal);
                Control.TransformationMethod = new PasswordTransformationMethod();
            }

            SetFont(view);
            SetTextAlignment(view);
            //SetBorder(view);
            SetPlaceholderTextColor(view);
            SetMaxLength(view);

            if (e.NewElement == null)
            {
                this.Touch -= HandleTouch;
            }

            if (e.OldElement == null)
            {
                this.Touch += HandleTouch;
            }
        }

        /// <summary>
        /// Handles the touch.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Android.Views.View.TouchEventArgs"/> instance containing the event data.</param>
        void HandleTouch (object sender, TouchEventArgs e)
        {
            var element = (ExtendedEntry)this.Element;
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    this.downX = e.Event.GetX();
                    this.downY = e.Event.GetY();
                    return;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                case MotionEventActions.Move:
                    this.upX = e.Event.GetX();
                    this.upY = e.Event.GetY();

                    float deltaX = this.downX - this.upX;
                    float deltaY = this.downY - this.upY;

                        // swipe horizontal?
                    if(Math.Abs(deltaX) > Math.Abs(deltaY))
                    {
                        if(Math.Abs(deltaX) > MinDistance)
                        {
                            if (deltaX < 0)
                            {
                                element.OnRightSwipe(this, EventArgs.Empty); 
                                return;
                            }

                            if (deltaX > 0)
                            {
                                element.OnLeftSwipe(this, EventArgs.Empty); 
                                return;
                            }
                        }
                        else 
                        {
                            Log.Info("ExtendedEntry", "Horizontal Swipe was only " + Math.Abs(deltaX) + " long, need at least " + MinDistance);
                            return; // We don't consume the event
                        }
                    }
                    // swipe vertical?
//                    else 
//                    {
//                        if(Math.abs(deltaY) > MIN_DISTANCE){
//                            // top or down
//                            if(deltaY < 0) { this.onDownSwipe(); return true; }
//                            if(deltaY > 0) { this.onUpSwipe(); return true; }
//                        }
//                        else {
//                            Log.i(logTag, "Vertical Swipe was only " + Math.abs(deltaX) + " long, need at least " + MIN_DISTANCE);
//                            return false; // We don't consume the event
//                        }
//                    }

                    return;
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var view = (ExtendedEntry)Element;

            if (e.PropertyName == ExtendedEntry.FontProperty.PropertyName)
            {
                SetFont(view);
            }
            else if (e.PropertyName == ExtendedEntry.XAlignProperty.PropertyName)
            {
                SetTextAlignment(view);
            }
            else if (e.PropertyName == ExtendedEntry.HasBorderProperty.PropertyName)
            {
                //return;   
            }
            else if (e.PropertyName == ExtendedEntry.PlaceholderTextColorProperty.PropertyName)
            {
                SetPlaceholderTextColor(view);
            }
            else
            {
                base.OnElementPropertyChanged(sender, e);
                if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
                {
                    this.Control.SetBackgroundColor(view.BackgroundColor.ToAndroid());
                }
            }
        }

        ///// <summary>
        ///// Sets the border.
        ///// </summary>
        ///// <param name="view">The view.</param>
        //private void SetBorder(ExtendedEntry view)
        //{
        //    //NotCurrentlySupported: HasBorder peroperty not suported on Android
        //}

        /// <summary>
        /// Sets the text alignment.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetTextAlignment(ExtendedEntry view)
        {
            switch (view.XAlign)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    Control.Gravity = GravityFlags.CenterHorizontal;
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    Control.Gravity = GravityFlags.End;
                    break;
                case Xamarin.Forms.TextAlignment.Start:
                    Control.Gravity = GravityFlags.Start;
                    break;
            }
        }

        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetFont(ExtendedEntry view)
        {
            if (view.Font != Font.Default) 
            {
                Control.TextSize = view.Font.ToScaledPixel();
                Control.Typeface = view.Font.ToExtendedTypeface(Context);
            }
        }

        /// <summary>
        /// Sets the color of the placeholder text.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetPlaceholderTextColor(ExtendedEntry view)
        {
            if (view.PlaceholderTextColor != Color.Default)
            {
                Control.SetHintTextColor(view.PlaceholderTextColor.ToAndroid());	
            }		
        }

        /// <summary>
        /// Sets the MaxLength characteres.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetMaxLength(ExtendedEntry view)
        {
            Control.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(view.MaxLength) });
        }
    }
}

