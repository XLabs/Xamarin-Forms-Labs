// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Logr.cs" company="XLabs Team">
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

using Android.Util;

namespace XLabs.Forms.Controls.MonoDroid.TimesSquare
{
    /// <summary>
    /// Class Logr.
    /// </summary>
    public class Logr
    {
        /// <summary>
        /// ds the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void D(string message)
        {
#if DEBUG
            Log.Debug("XLabs.Forms.Controls.Calendar", message);
#endif
        }

        /// <summary>
        /// ds the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public static void D(string message, params object[] args)
        {
#if DEBUG
            D(string.Format(message, args));
#endif
        }
    }
}