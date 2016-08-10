// ***********************************************************************
// Assembly         : XLabs.Platform.WP81
// Author           : XLabs Team
// Created          : 01-01-2016
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

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Email;

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
				return true;
			}
		}

		/// <summary>
		/// Shows the draft.
		/// </summary>
		/// <param name="subject">The subject.</param>
		/// <param name="body">The body.</param>
		/// <param name="html">if set to <c>true</c> [HTML].</param>
		/// <param name="to">To.</param>
		/// <param name="attachments">The attachments.</param>
		public async void ShowDraft(string subject, string body, bool html, string to, IEnumerable<string> attachments = null)
		{
			ShowDraft(subject, body, html, new[] {to}, null, null, attachments);
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
		public async void ShowDraft(
			string subject,
			string body,
			bool html,
			string[] to,
			string[] cc,
			string[] bcc,
			IEnumerable<string> attachments = null)
		{
			var emailMessage = new EmailMessage
			{
				Subject = subject,
				Body = body
			};

			if (to != null && to.Any())
			{
				foreach (var t in to) { emailMessage.To.Add(new EmailRecipient(t));}
			}

			if (cc != null && cc.Any())
			{
				foreach (var t in cc) { emailMessage.CC.Add(new EmailRecipient(t)); }
			}

			if (bcc != null && bcc.Any())
			{
				foreach (var t in bcc) { emailMessage.Bcc.Add(new EmailRecipient(t)); }
			}

			if (attachments != null && attachments.Any())
			{
				throw new NotImplementedException("Attachnents not supported yet");
				//foreach (var attachment in attachments)
				//{
				//	emailMessage.Attachments.Add(new EmailAttachment(attachment));
				//}
			}

			await EmailManager.ShowComposeNewEmailAsync(emailMessage);
		}

		#endregion
	}
}