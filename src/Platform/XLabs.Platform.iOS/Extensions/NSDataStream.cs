namespace XLabs.Platform.Extensions
{
	using System.IO;

	using Foundation;

	/// <summary>
	///     Class NsDataStream.
	/// </summary>
	internal unsafe class NsDataStream : UnmanagedMemoryStream
	{
		/// <summary>
		///     The _data
		/// </summary>
		private readonly NSData _data;

		/// <summary>
		///     Initializes a new instance of the <see cref="NsDataStream" /> class.
		/// </summary>
		/// <param name="data">The data.</param>
		public NsDataStream(NSData data)
			: base((byte*)data.Bytes, (long)data.Length)
		{
			_data = data;
		}

		/// <summary>
		///     Releases the unmanaged resources used by the <see cref="T:System.IO.UnmanagedMemoryStream" /> and optionally
		///     releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		///     true to release both managed and unmanaged resources; false to release only unmanaged
		///     resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_data.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}