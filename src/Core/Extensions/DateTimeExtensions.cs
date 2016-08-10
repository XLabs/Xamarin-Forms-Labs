// ***********************************************************************
// Assembly         : XLabs.Core
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="DateTimeExtensions.cs" company="XLabs Team">
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

namespace XLabs
{
    /// <summary>
    /// Class DateTimeExtensions.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// The unix time
        /// </summary>
        public static DateTime UnixTime = new DateTime(1970, 1, 1);

        /// <summary>
        /// Sinces the unix time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns>TimeSpan.</returns>
        public static TimeSpan SinceUnixTime(this DateTime time)
        {
            return time - UnixTime;
        }

        /// <summary>
        /// Sinces the unix time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns>TimeSpan.</returns>
        public static TimeSpan SinceUnixTime(this DateTimeOffset time)
        {
            return time - UnixTime;
        }

        /// <summary>
        /// Sinces the unix time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns>System.Nullable&lt;TimeSpan&gt;.</returns>
        public static TimeSpan? SinceUnixTime(this DateTime? time)
        {
            return time - UnixTime;
        }

        /// <summary>
        /// Sinces the unix time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns>System.Nullable&lt;TimeSpan&gt;.</returns>
        public static TimeSpan? SinceUnixTime(this DateTimeOffset? time)
        {
            return time - UnixTime;
        }

        /// <summary>
        /// Fulls the milliseconds.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <returns>System.Nullable&lt;System.Int64&gt;.</returns>
        public static long? FullMilliseconds(this TimeSpan? timeSpan)
        {
            return timeSpan == null ? default(long?) : (long)timeSpan.Value.TotalMilliseconds;
        }
    }
}
