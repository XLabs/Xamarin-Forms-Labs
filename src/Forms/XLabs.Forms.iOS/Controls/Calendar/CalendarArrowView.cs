// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CalendarArrowView.cs" company="XLabs Team">
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

using CoreGraphics;
using UIKit;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class CalendarArrowView.
    /// </summary>
    public class CalendarArrowView : UIButton
    {
        /// <summary>
        /// Enum ArrowDirection
        /// </summary>
        public enum ArrowDirection {
            /// <summary>
            /// The right arrow direction.
            /// </summary>
            Right,
            /// <summary>
            /// The left arrow direction.
            /// </summary>
            Left
        }

        /// <summary>
        /// The _arrow direction
        /// </summary>
        private ArrowDirection _arrowDirection = ArrowDirection.Left;

        /// <summary>
        /// The _color
        /// </summary>
        private UIColor _color;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarArrowView"/> class.
        /// </summary>
        /// <param name="frame">The frame.</param>
        public CalendarArrowView(CGRect frame)
        {
            Frame = frame;
            UserInteractionEnabled = true;
            BackgroundColor = UIColor.Clear;
        }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>The direction.</value>
        public ArrowDirection Direction
        {
            get
            {
                return _arrowDirection;
            }
            set
            {
                _arrowDirection = value;
                SetBackgroundImage(GenerateImageForButton(Frame), UIControlState.Normal);
                //_trianglePath = GetEquilateralTriangle(this.Width, this.Height);
                SetNeedsDisplay();
            }
        }


        /// <summary>
        /// Sets the color.
        /// </summary>
        /// <value>The color.</value>
        public UIColor Color
        {
            set
            {
                _color = value;
                SetNeedsDisplay();
            }
        }

        /// <summary>
        /// Draws the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

        }

        /// <summary>
        /// Generates the image for button.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns>UIImage.</returns>
        private UIImage GenerateImageForButton(CGRect rect)
        {
            UIGraphics.BeginImageContextWithOptions(rect.Size, false, 0);
            UIImage image;

            using(var context = UIGraphics.GetCurrentContext())
            {
                CGPoint p1 , p2 , p3;
                if(_arrowDirection == ArrowDirection.Left)
                {
                    p1 = new CGPoint(0, (rect.Height) / 2);
                    p2 = new CGPoint(rect.Width, 0);
                    p3 = new CGPoint(rect.Width, rect.Height);
                }
                else
                {
                    p1 = new CGPoint(rect.Width, rect.Height / 2);
                    p2 = new CGPoint(0, 0);
                    p3 = new CGPoint(0, rect.Height);
                }

                context.SetFillColor(UIColor.Clear.CGColor);
                context.FillRect(rect);
                context.SetFillColor(_color.CGColor);
                context.MoveTo(p1.X, p1.Y);
                context.AddLineToPoint(p2.X, p2.Y);
                context.AddLineToPoint(p3.X, p3.Y);
                context.FillPath();
                image = UIGraphics.GetImageFromCurrentImageContext();
            }

            UIGraphics.EndImageContext();
            return image;
        }
    }
}

