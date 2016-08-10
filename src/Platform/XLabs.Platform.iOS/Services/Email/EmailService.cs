// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
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
using System.IO;
using Foundation;
using MessageUI;
using UIKit;

namespace XLabs.Platform.Services.Email
{
    /// <summary>
    /// Class EmailService.
    /// </summary>
    public class EmailService : IEmailService
    {
        #region IEmailService Members

        /// <summary>
        /// Gets a value indicating whether this instance can send.
        /// </summary>
        /// <value><c>true</c> if this instance can send; otherwise, <c>false</c>.</value>
        public bool CanSend
        {
            get
            {
                return MFMailComposeViewController.CanSendMail;
            }
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
        public void ShowDraft(
            string subject,
            string body,
            bool html,
            string[] to,
            string[] cc,
            string[] bcc,
            IEnumerable<string> attachments = null)
        {
            var mailer = new MFMailComposeViewController();

            mailer.SetMessageBody(body ?? string.Empty, html);
            mailer.SetSubject(subject ?? string.Empty);
            mailer.SetCcRecipients(cc);
            mailer.SetToRecipients(to);
            mailer.Finished += (s, e) => ((MFMailComposeViewController)s).DismissViewController(true, () => { });

            if (attachments != null) 
            {
                foreach (var attachment in attachments) 
                {
                    mailer.AddAttachmentData (NSData.FromFile (attachment), GetMimeType (attachment), Path.GetFileName (attachment));
                }
            }

            UIViewController vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while(vc.PresentedViewController != null) 
            {
                vc = vc.PresentedViewController;
            }
            vc.PresentViewController(mailer, true, null);
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
            ShowDraft(subject, body, html, new[] { to }, new string[] { }, new string[] { }, attachments);
        }

        // TODO: make this more robust
        /// <summary>
        /// Gets the type of the MIME.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>System.String.</returns>
        private string GetMimeType(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return null;
            }

            var extension = Path.GetExtension(filename.ToLowerInvariant());

            switch (extension)
            {
                case "png":
                    return "image/png";
                case "doc":
                    return "application/msword";
                case "pdf":
                    return "application/pdf";
                case "jpeg":
                case "jpg":
                    return "image/jpeg";
                case "zip":
                case "docx":
                case "xlsx":
                case "pptx":
                    return "application/zip";
                case "htm":
                case "html":
                    return "text/html";
            }

            return "application/octet-stream";
        }

        #endregion
    }
}