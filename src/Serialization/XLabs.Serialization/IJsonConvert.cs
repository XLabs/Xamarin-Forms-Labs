namespace XLabs.Serialization
{
	/// <summary>
	/// Interface IJsonConvert
	/// </summary>
	public interface IJsonConvert
    {
		/// <summary>
		/// To the json.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns>System.String.</returns>
		string ToJson(object obj);
    }
}
