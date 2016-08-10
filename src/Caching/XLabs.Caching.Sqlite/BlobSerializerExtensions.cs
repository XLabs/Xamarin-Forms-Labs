// ***********************************************************************
// Assembly         : XLabs.Caching.SQLite
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="BlobSerializerExtensions.cs" company="XLabs Team">
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
#region

using System;
using SQLite.Net;
using XLabs.Serialization;

#endregion

namespace XLabs.Caching.SQLite
{
    /// <summary>
    /// Converts <see cref="IByteSerializer"/> to <see cref="IBlobSerializer"/>.
    /// </summary>
    public static class BlobSerializerExtensions
    {
        /// <summary>
        /// Converts <see cref="IByteSerializer"/> to <see cref="IBlobSerializer"/>.
        /// </summary>
        /// <param name="serializer">Byte serializer.</param>
        /// <returns>IBlobSerializer</returns>
        public static IBlobSerializer AsBlobSerializer(this IByteSerializer serializer)
        {
            return new BlobSerializerDelegate(serializer.SerializeToBytes, serializer.Deserialize, serializer.CanDeserialize);
        }

        private static bool CanDeserialize(this IByteSerializer serializer, Type type)
        {
            return true;
        }
    }
}
