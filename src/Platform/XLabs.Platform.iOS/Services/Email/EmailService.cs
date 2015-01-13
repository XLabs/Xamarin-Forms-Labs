namespace XLabs.Platform.Services.Email
{
	using System.Collections.Generic;
	using System.IO;

	using Foundation;
	using MessageUI;
	using UIKit;

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

			UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(mailer, true, null);
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