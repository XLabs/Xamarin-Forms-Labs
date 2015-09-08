namespace XLabs.Platform.Device
{
	using System.IO;
	using System.Threading.Tasks;

	/// <summary>
	/// Interface IBluetoothDevice
	/// </summary>
	public interface IBluetoothDevice
	{
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; }
		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>The address.</value>
		string Address { get; }

		/// <summary>
		/// Gets the input stream.
		/// </summary>
		/// <value>The input stream.</value>
		Stream InputStream { get; }
		/// <summary>
		/// Gets the output stream.
		/// </summary>
		/// <value>The output stream.</value>
		Stream OutputStream { get; }

		/// <summary>
		/// Connects this instance.
		/// </summary>
		/// <returns>Task.</returns>
		Task Connect();
		/// <summary>
		/// Disconnects this instance.
		/// </summary>
		void Disconnect(); 
	}
}