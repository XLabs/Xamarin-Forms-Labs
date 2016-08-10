// ***********************************************************************
// Assembly       : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 01-06-2016
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-06-2016
// ***********************************************************************
// <copyright file="CheckBoxView.cs" company="XLabs Team">
//        Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//        This project is licensed under the Apache 2.0 license
//        https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
// 
//        XLabs is a open source project that aims to provide a powerfull and cross
//        platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using CoreGraphics;
using Foundation;
using UIKit;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class CheckBoxView.
    /// </summary>
    [Register("CheckBoxView")]
    public class CheckBoxView : UIButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxView"/> class.
        /// </summary>
        public CheckBoxView()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxView"/> class.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        public CheckBoxView(CGRect bounds) : base(bounds)
        {
            Initialize();
        }

        /// <summary>
        /// Sets the checked title.
        /// </summary>
        /// <value>The checked title.</value>
        public string CheckedTitle
        {
            set
            {
                SetTitle(value, UIControlState.Selected);
            }
        }

        /// <summary>
        /// Sets the unchecked title.
        /// </summary>
        /// <value>The unchecked title.</value>
        public string UncheckedTitle
        {
            set
            {
                SetTitle(value, UIControlState.Normal);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CheckBoxView"/> is checked.
        /// </summary>
        /// <value><c>true</c> if checked; otherwise, <c>false</c>.</value>
        public bool Checked
        {
            set { Selected = value; }
            get { return Selected; }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void Initialize()
        {
            AdjustEdgeInsets();
            ApplyStyle();

            TouchUpInside += (sender, args) => Selected = !Selected;
            // set default color, because type is not UIButtonType.System 
            SetTitleColor(UIColor.DarkTextColor, UIControlState.Normal);
            SetTitleColor(UIColor.DarkTextColor, UIControlState.Selected);
        }

        /// <summary>
        /// Adjusts the edge insets.
        /// </summary>
        void AdjustEdgeInsets()
        {
            const float Inset = 8f;

            HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
            ImageEdgeInsets = new UIEdgeInsets(0f, Inset, 0f, 0f);
            TitleEdgeInsets = new UIEdgeInsets(0f, Inset * 2, 0f, 0f);
        }

        /// <summary>
        /// Applies the style.
        /// </summary>
        void ApplyStyle()
        {
            SetImage(UIImage.FromBundle("Images/Checkbox/checked_checkbox.png"), UIControlState.Selected);
            SetImage(UIImage.FromBundle("Images/Checkbox/unchecked_checkbox.png"), UIControlState.Normal);
        }
    }
}