// ***********************************************************************
// Assembly         : XLabs.Web.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ProtoBufRestClient.cs" company="XLabs Team">
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
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using XLabs.Serialization;

namespace XLabs.Web.RestClient
{
    /// <summary>
    /// The protobuf Rest client.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class ProtoBufRestClient : RestCoreClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProtoBufRestClient"/> class.
        /// </summary>
        /// <param name="serializer">
        /// The serializer.
        /// </param>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an exception if the serializer does not support ProtoBuffer
        /// </exception>
        public ProtoBufRestClient(ISerializer serializer, HttpClient client = null) : base(serializer, client)
        {
            if (serializer.Format != SerializationFormat.ProtoBuffer)
            {
                throw new Exception(string.Format("Invalid serializer type: {0}. Valid type is: {1}", serializer.Format, SerializationFormat.ProtoBuffer));
            }
        }

        /// <summary>
        /// Gets the string content type.
        /// </summary>
        protected override string StringContentType
        {
            get { return "application/x-protobuf"; }
        }
    }
}
