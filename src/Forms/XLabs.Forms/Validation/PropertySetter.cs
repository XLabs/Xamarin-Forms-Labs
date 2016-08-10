// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="PropertySetter.cs" company="XLabs Team">
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

namespace XLabs.Forms.Validation
{
    /// <summary>Defines one property to set during validation</summary>
    /// Element created at 08/11/2014,3:54 PM by Charles
    public class PropertySetter : BindableObject
    {
        /// <summary>Definition for <see cref="Property"/></summary>
        /// Element created at 08/11/2014,3:54 PM by Charles
        public BindableProperty PropertyProperty =
            BindableProperty.Create<PropertySetter, string>(x => x.Property, default(string));

        /// <summary>
        /// Definition for <see cref="ValidValue"/>
        /// </summary>
        /// Element created at 08/11/2014,3:54 PM by Charles
        public BindableProperty ValidValueProperty =
            BindableProperty.Create<PropertySetter, string>(x => x.ValidValue, default(string));

        /// <summary>
        /// Definition for <see cref="InvalidValue"/>
        /// </summary>
        /// Element created at 08/11/2014,3:55 PM by Charles
        public BindableProperty InvalidValueProperty =
            BindableProperty.Create<PropertySetter, string>(x => x.InvalidValue, default(string));

        /// <summary>Gets or sets the property name.</summary>
        /// <value>The property.</value>
        /// Element created at 08/11/2014,3:55 PM by Charles
        public string Property
        {
            get { return (string)GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty,value);}
        }

        /// <summary>The value to set when the rule is valid property</summary>
        /// <value>The valid value.</value>
        /// Element created at 08/11/2014,3:55 PM by Charles
        public string ValidValue
        {
            get { return (string)GetValue(ValidValueProperty); }
            set { SetValue(ValidValueProperty, value); }
        }

        /// <summary>
        /// The value to set when the rule is invalid
        /// </summary>
        /// <value>The in valid value.</value>
        /// Element created at 08/11/2014,3:56 PM by Charles
        public string InvalidValue
        {
            get { return (string)GetValue(InvalidValueProperty); }
            set { SetValue(InvalidValueProperty, value); }
        }

    }
}
