// ***********************************************************************
// Assembly         : XLabs.Web.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="HttpHandler.cs" company="XLabs Team">
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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using XLabs;

namespace XLabs.Web
{
    /// <summary>
    /// Class HttpHandler.
    /// </summary>
    public class HttpHandler : HttpClientHandler
    {
        /// <summary>
        /// The cookie header
        /// </summary>
        private const string CookieHeader = "Set-Cookie";

        /// <summary>
        /// The cookie received
        /// </summary>
        public EventHandler<EventArgs<Cookie>> CookieReceived;

        /// <summary>
        /// send as an asynchronous operation.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode && response.Headers.Contains(CookieHeader))
            {
                try
                {
                    var cookie = response.Headers.GetValues(CookieHeader).First().Split(';')[0].Split('=');
                    this.CookieReceived.Invoke<Cookie>(this, new Cookie(cookie[0], cookie[1]));
                }
                catch
                {

                }
            }

            return response;
        }
    }
}
