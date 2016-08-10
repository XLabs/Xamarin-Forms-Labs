// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
//
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="FontManager.designer.cs" company="XLabs Team">
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
using Xamarin.Forms;

namespace XLabs.Forms.Services
{
    /// <summary>
    /// Class FontManager.
    /// </summary>
    public partial class FontManager
    {
        private const short InitialSize = 24;

        /// <summary>
        /// Finds the closest.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="desiredHeight">Height of the desired.</param>
        /// <returns>Font.</returns>
        public Font FindClosest(string name, double desiredHeight)
        {
            var height = this.GetHeight(Font.OfSize(name, InitialSize));

            var multiply = (int)((desiredHeight / height) * InitialSize);


            var f1 = Font.OfSize(name, multiply);
            var f2 = Font.OfSize(name, multiply + 1);

            var h1 = this.GetHeight(f1);
            var h2 = this.GetHeight(f2);

            var d1 = Math.Abs(h1 - desiredHeight);
            var d2 = Math.Abs(h2 - desiredHeight);

            return d1 < d2 ? f1 : f2;
        }

    }
}
