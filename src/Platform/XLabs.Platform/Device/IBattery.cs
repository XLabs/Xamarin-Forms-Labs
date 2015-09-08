namespace XLabs.Platform.Device
{
	using System;

	/// <summary>
	/// Defines battery interface
	/// </summary>
	public interface IBattery
	{
		/// <summary>
		/// Gets the level.
		/// </summary>
		/// <value>The level in percentage 0-100.</value>
		int Level { get; }

		/// <summary>
		/// Gets a value indicating whether battery is charging
		/// </summary>
		/// <value><c>true</c> if charging; otherwise, <c>false</c>.</value>
		bool Charging { get; }

		/// <summary>
		/// Occurs when level changes.
		/// </summary>
		event EventHandler<EventArgs<int>> OnLevelChange;

		/// <summary>
		/// Occurs when charger is connected or disconnected.
		/// </summary>
		event EventHandler<EventArgs<bool>> OnChargerStatusChanged;
	}
}
