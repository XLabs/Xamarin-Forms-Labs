//namespace XLabs.Platform.Services.Email
//{
//	using System.Collections.Generic;
//	using System.Linq;
//	using System.Text;
//	using System;
//	using Windows.ApplicationModel.DataTransfer;

//	/// <summary>
//	/// Class EmailService.
//	/// </summary>
//	public class EmailService : IEmailService
//	{
//		#region IEmailService Members

//		/// <summary>
//		/// Gets a value indicating whether this instance can send.
//		/// </summary>
//		/// <value><c>true</c> if this instance can send; otherwise, <c>false</c>.</value>
//		public bool CanSend
//		{
//			get
//			{			
//				return true;
//			}
//		}

//		/// <summary>
//		/// Shows the draft.
//		/// </summary>
//		/// <param name="subject">The subject.</param>
//		/// <param name="body">The body.</param>
//		/// <param name="html">if set to <c>true</c> [HTML].</param>
//		/// <param name="to">To.</param>
//		/// <param name="attachments">The attachments.</param>
//		public async void ShowDraft(string subject, string body, bool html, string to, IEnumerable<string> attachments = null)
//		{
//			ShowDraft(subject, body, html, new[] {to}, null, null, attachments);
//		}

//		/// <summary>
//		/// Shows the draft.
//		/// </summary>
//		/// <param name="subject">The subject.</param>
//		/// <param name="body">The body.</param>
//		/// <param name="html">if set to <c>true</c> [HTML].</param>
//		/// <param name="to">To.</param>
//		/// <param name="cc">The cc.</param>
//		/// <param name="bcc">The BCC.</param>
//		/// <param name="attachments">The attachments.</param>
//		public async void ShowDraft(
//			string subject,
//			string body,
//			bool html,
//			string[] to,
//			string[] cc,
//			string[] bcc,
//			IEnumerable<string> attachments = null)
//		{
//			//var emailMessage = new EmailMessage
//			//{
//			//	Subject = subject,
//			//	Body = body
//			//};

//			//if (to != null && to.Any())
//			//{
//			//	foreach (var t in to) { emailMessage.To.Add(new EmailRecipient(t));}
//			//}

//			//if (cc != null && cc.Any())
//			//{
//			//	foreach (var t in cc) { emailMessage.CC.Add(new EmailRecipient(t)); }
//			//}

//			//if (bcc != null && bcc.Any())
//			//{
//			//	foreach (var t in bcc) { emailMessage.Bcc.Add(new EmailRecipient(t)); }
//			//}

//			//if (attachments != null && attachments.Any())
//			//{
//			//	throw new NotImplementedException("Attachnents not supported yet");
//			//	//foreach (var attachment in attachments)
//			//	//{				
//			//	//	emailMessage.Attachments.Add(new EmailAttachment(attachment));
//			//	//}
//			//}

//			//await EmailManager.ShowComposeNewEmailAsync(emailMessage);
			 
//			throw new NotImplementedException();
//		}

//		#endregion
//	}
//}