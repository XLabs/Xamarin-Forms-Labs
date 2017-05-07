// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ContentControl.cs" company="XLabs Team">
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

using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// A view that renders its content based on a data template. Typical usage is to either set an explicit 
    /// <see cref="BindableObject.BindingContext"/> on this element or use an inhereted one, then set a display.
    /// </summary>
    public class ContentControl : ContentView
    {
        /// <summary>
        /// The content template property
        /// </summary>
        public static readonly BindableProperty ContentTemplateProperty = BindableProperty.Create<ContentControl, DataTemplate>(x => x.ContentTemplate, null);

        protected override void OnBindingContextChanged()
        {
          var template = ContentTemplate;

          if (template == null)
          {
            Content = null;
            return;
          }

          var dts = template as DataTemplateSelector;
          if (dts != null)
          {
            var item = GetValue(BindingContextProperty);
            template = dts.SelectTemplate(item, this);
          }          
          
          Content = (View)template.CreateContent();                      
        }

        /// <summary>
        /// A <see cref="DataTemplate"/> used to render the view. This property is bindable.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ContentTemplateProperty);
            }
            set
            {
                SetValue(ContentTemplateProperty, value);
            }
        }

    }
}
