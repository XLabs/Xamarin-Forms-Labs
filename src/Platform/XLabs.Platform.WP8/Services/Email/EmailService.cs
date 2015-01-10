namespace XLabs.Platform.Services.Email
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Microsoft.Phone.Tasks;

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
		public void ShowDraft(string subject, string body, bool html, string to, IEnumerable<string> attachments = null)
		{
			var task = new EmailComposeTask { Subject = subject, Body = body, To = to };

			task.Show();
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
			var task = new EmailComposeTask { Subject = subject, Body = body };

			var stringBuilder = new StringBuilder();

			if (to.Any())
			{
				foreach (var t in to)
				{
					stringBuilder.Append(t);
					stringBuilder.Append(";");
				}

				task.To = stringBuilder.ToString();
				stringBuilder.Clear();
			}

			if (cc.Any())
			{
				foreach (var c in cc)
				{
					stringBuilder.Append(c);
					stringBuilder.Append(";");
				}

				task.Cc = stringBuilder.ToString();
				stringBuilder.Clear();
			}

			if (bcc.Any())
			{
				foreach (var b in bcc)
				{
					stringBuilder.Append(b);
					stringBuilder.Append(";");
				}

				task.Bcc = stringBuilder.ToString();
				stringBuilder.Clear();
			}

			task.Show();
		}

		#endregion
	}
}