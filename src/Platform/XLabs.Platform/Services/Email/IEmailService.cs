// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IEmailService.cs" company="XLabs Team">
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

namespace XLabs.Platform.Services.Email
{
	/// <summary>
	/// Interface IEmailService
	/// </summary>
	public interface IEmailService
	{
		/// <summary>
		/// Gets a value indicating whether this instance can send.
		/// </summary>
		/// <value><c>true</c> if this instance can send; otherwise, <c>false</c>.</value>
		bool CanSend { get; }
		/// <summary>
		/// Shows the draft.
		/// </summary>
		/// <param name="subject">The subject.</param>
		/// <param name="body">The body.</param>
		/// <param name="html">if set to <c>true</c> [HTML].</param>
		/// <param name="to">To.</param>
		/// <param name="attachments">The attachments.</param>
		void ShowDraft(string subject, string body, bool html, string to, IEnumerable<string> attachments = null);
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
		void ShowDraft(string subject, string body, bool html, string[] to, string[] cc, string[] bcc, IEnumerable<string> attachments = null);
	}
}
