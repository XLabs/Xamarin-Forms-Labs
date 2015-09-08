namespace XLabs.Platform
{
	using System.Threading.Tasks;

	using XLabs.Platform.Device;
	using XLabs.Platform.Services.Geolocation;

	/// <summary>
	///     Class DeviceExtensions.
	/// </summary>
	public static class DeviceExtensions
	{
		/// <summary>
		///     Drives to.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <param name="position">The position.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public static Task<bool> DriveTo(this IDevice device, Position position)
		{
			return device.LaunchUriAsync(position.DriveToLink());
		}
	}
}