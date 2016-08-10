// ***********************************************************************
// Assembly         : XLabs.Web
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="WebResponseException.cs" company="XLabs Team">
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
using System.Net;

namespace XLabs.Web
{
    /// <summary>
    /// Class WebResponseException.
    /// </summary>
    public class WebResponseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebResponseException"/> class.
        /// </summary>
        public WebResponseException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public WebResponseException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebResponseException"/> class.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        public WebResponseException(HttpStatusCode status, string message)
            : base(message)
        {
            this.Status = status;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public WebResponseException(string message, Exception innerException) 
            : base(message, innerException)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebResponseException"/> class.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebResponseException(HttpStatusCode status, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Status = status;
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public HttpStatusCode Status { get; set; }
    }
}

