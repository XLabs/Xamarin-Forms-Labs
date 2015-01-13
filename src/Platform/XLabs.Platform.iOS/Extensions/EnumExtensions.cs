namespace XLabs.Platform.Extensions
{
	using System;
	using System.ComponentModel;

	/// <summary>
	/// Class EnumExtensions.
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Gets the description.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>System.String.</returns>
		public static string GetDescription(this Enum value)
		{
			var field = value.GetType().GetField(value.ToString());

			var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

			return attribute == null ? value.ToString() : attribute.Description;
		}
	}
}