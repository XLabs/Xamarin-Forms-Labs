//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace XLabs.Platform.Device
//{
//	using XLabs;

//	/// <summary>
//	/// Class Battery.
//	/// </summary>
//	public partial class Battery : IBattery
//	{
//		/// <summary>
//		/// Event handler for battery level changes.
//		/// </summary>
//		public event EventHandler<EventArgs<int>> OnLevelChange
//		{
//			add
//			{
//				// if this is the first subscriber lets start the monitoring
//				if (onLevelChange == null)
//				{
//					StartLevelMonitoring();
//				}
//				onLevelChange += value;
//			}

//			remove
//			{
//				onLevelChange -= value;
//				// if this is the last client then we want to stop monitoring
//				if (onLevelChange == null)
//				{
//					StopLevelMonitoring();
//				}
//			}
//		}

//		/// <summary>
//		/// Event handler for charger connection changes.
//		/// </summary>
//		public event EventHandler<EventArgs<bool>> OnChargerStatusChanged
//		{
//			add
//			{
//				if (onChargerStatusChanged == null)
//				{
//					StartChargerMonitoring();
//				}
//				onChargerStatusChanged += value;
//			}
//			remove
//			{
//				onChargerStatusChanged -= value;
//				if (onChargerStatusChanged == null)
//				{
//					StopChargerMonitoring();
//				}
//			}
//		}

//		/// <summary>
//		/// Private event handler for battery level changes.
//		/// </summary>
//		private static event EventHandler<EventArgs<int>> onLevelChange;

//		/// <summary>
//		/// Private event handler for charger connection changes.
//		/// </summary>
//		private static event EventHandler<EventArgs<bool>> onChargerStatusChanged;

//		/// <summary>
//		/// Start level monitoring
//		/// </summary>
//		/// <remarks>Partial method to be implemented by platform specific provider.</remarks>
//		partial void StartLevelMonitoring();

//		/// <summary>
//		/// Stop level monitoring
//		/// </summary>
//		/// <remarks>Partial method to be implemented by platform specific provider.</remarks>
//		partial void StopLevelMonitoring();

//		/// <summary>
//		/// Start charger monitoring
//		/// </summary>
//		/// <remarks>Partial method to be implemented by platform specific provider.</remarks>
//		partial void StartChargerMonitoring();

//		/// <summary>
//		/// Stop charger monitoring
//		/// </summary>
//		/// <remarks>Partial method to be implemented by platform specific provider.</remarks>
//		partial void StopChargerMonitoring();
//	}
//}
