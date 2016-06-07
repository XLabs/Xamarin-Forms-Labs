// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : Tim Klingeleers
// Last Modified On : 03-27-2016
// ***********************************************************************
// <copyright file="EmailService.cs" company="XLabs Team">
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

using System.Collections.Generic;
using Android.Content;

namespace XLabs.Platform.Services.Email
{
    /// <summary>
    /// Class EmailService.
    /// </summary>
    public class EmailService : IEmailService
    {
        private IEnumerable<string> fileProviders;

        public EmailService()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XLabs.Platform.Services.Email.EmailService"/> class.
        /// </summary>
        /// <param name="fileProviders">A list of file providers. 
        /// Required as of Android M (API level 23) and higher.</param>
        public EmailService(IEnumerable<string> fileProviders)
        {
            this.fileProviders = fileProviders;
        }

        #region IEmailService Members

        // TODO: Check if there is a way to check this on Android
        /// <summary>
        /// Gets a value indicating whether this instance can send.
        /// </summary>
        /// <value><c>true</c> if this instance can send; otherwise, <c>false</c>.</value>
        public bool CanSend
        {
            get { return true; }
        }

        /// <summary>
        /// Shows the draft.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="html">if set to <c>true</c> [HTML].</param>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="bcc">The BCC.</param>
        /// <param name="attachments">The attachments.</param>
        public void ShowDraft(string subject, string body, bool html, string[] to, string[] cc, string[] bcc, IEnumerable<string> attachments = null)
        {
            var intent = new Intent(Intent.ActionSendMultiple);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);

            intent.SetType(html ? "text/html" : "text/plain");
            intent.PutExtra(Intent.ExtraEmail, to);
            if (cc != null)
            {
                intent.PutExtra(Intent.ExtraCc, cc);
            }
            if (bcc != null)
            {
                intent.PutExtra(Intent.ExtraBcc, bcc);
            }
            intent.PutExtra(Intent.ExtraSubject, subject ?? string.Empty);

            if (html)
            {
                intent.PutExtra(Intent.ExtraText, Android.Text.Html.FromHtml(body));
            }
            else
            {
                intent.PutExtra(Intent.ExtraText, body ?? string.Empty);
            }

            if (attachments != null)
            {
                intent.AddAttachments(attachments, fileProviders);
            }

            this.StartActivity(intent);
        }

        /// <summary>
        /// Shows the draft.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="html">if set to <c>true</c> [HTML].</param>
        /// <param name="to">To.</param>
        /// <param name="attachments">The attachments.</param>
        public void ShowDraft(string subject, string body, bool html, string to, IEnumerable<string> attachments = null)
        {
            ShowDraft(subject, body, html, new string[] { to }, null, null, attachments);
        }

        #endregion
    }
}
