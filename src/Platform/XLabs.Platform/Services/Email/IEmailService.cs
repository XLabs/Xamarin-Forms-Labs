namespace XLabs.Platform.Services.Email
{
	using System.Collections.Generic;

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
