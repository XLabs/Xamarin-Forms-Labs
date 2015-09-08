namespace XLabs.Platform
{
	using System.Collections.Generic;
	using System.Linq;

	using Android.Content;
	using Android.Net;

	/// <summary>
	/// Class IntentExtensions.
	/// </summary>
	public static class IntentExtensions
    {
		/// <summary>
		/// Adds the attachments.
		/// </summary>
		/// <param name="intent">The intent.</param>
		/// <param name="attachments">The attachments.</param>
        public static void AddAttachments(this Intent intent, IEnumerable<string> attachments)
        {
            if (attachments == null || !attachments.Any())
            {
                Android.Util.Log.Warn("Intent.AddAttachments", "No attachments to attach.");
                return;
            }

			foreach (var attachment in attachments) {
				var file = new Java.IO.File(attachment);
				// File existence check
				if (file.Exists()) {
					intent.PutExtra(Intent.ExtraStream, Uri.FromFile(file));
				}
				else {
                    Android.Util.Log.Warn("Intent.AddAttachments", "Unable to attach file '{0}', because it doesn't exist.", attachment);
				}
			}
        }
    }
}
