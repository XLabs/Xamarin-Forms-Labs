//using System;

namespace XLabs.Platform.Services.Email
{
	using System.Collections.Generic;

	using Android.Content;

	/// <summary>
	/// Class EmailService.
	/// </summary>
	public class EmailService : IEmailService
	{
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
			var intent = new Intent(Intent.ActionSend);

			intent.SetType(html ? "text/html" : "text/plain");
			intent.PutExtra(Intent.ExtraEmail, to);
			intent.PutExtra(Intent.ExtraCc, cc);
			intent.PutExtra(Intent.ExtraBcc, bcc);
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
				intent.AddAttachments (attachments);
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
			var intent = new Intent(Intent.ActionSend);
			intent.SetType(html ? "text/html" : "text/plain");
			intent.PutExtra(Intent.ExtraEmail, new string[]{ to });
			intent.PutExtra(Intent.ExtraSubject, subject ?? string.Empty);

			if (html)
			{
				intent.PutExtra(Intent.ExtraText, body);
			}
			else
			{
				intent.PutExtra(Intent.ExtraText, body ?? string.Empty);
			}

			intent.AddAttachments(attachments);

			this.StartActivity(intent);
		}

		#endregion
	}
}
