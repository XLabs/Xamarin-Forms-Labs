//
// https://github.com/ServiceStack/ServiceStack.Text
// ServiceStack.Text: .NET C# POCO JSON, JSV and CSV Text Serializers.
//
// Authors:
//   Demis Bellot (demis.bellot@gmail.com)
//
// Copyright 2012 ServiceStack Ltd.
//
// Licensed under the same terms of ServiceStack: new BSD license.
//

using System.Collections.Generic;
using System.Text;
using ServiceStack.Text.Common;

namespace ServiceStack.Text
{
	/// <summary>
	/// Class MapExtensions.
	/// </summary>
	public static class MapExtensions
	{
		/// <summary>
		/// Joins the specified values.
		/// </summary>
		/// <typeparam name="K"></typeparam>
		/// <typeparam name="V"></typeparam>
		/// <param name="values">The values.</param>
		/// <returns>System.String.</returns>
		public static string Join<K, V>(this Dictionary<K, V> values)
		{
			return Join(values, JsWriter.ItemSeperatorString, JsWriter.MapKeySeperatorString);
		}

		/// <summary>
		/// Joins the specified item seperator.
		/// </summary>
		/// <typeparam name="K"></typeparam>
		/// <typeparam name="V"></typeparam>
		/// <param name="values">The values.</param>
		/// <param name="itemSeperator">The item seperator.</param>
		/// <param name="keySeperator">The key seperator.</param>
		/// <returns>System.String.</returns>
		public static string Join<K, V>(this Dictionary<K, V> values, string itemSeperator, string keySeperator)
		{
			var sb = new StringBuilder();
			foreach (var entry in values)
			{
				if (sb.Length > 0)
					sb.Append(itemSeperator);

				sb.Append(entry.Key).Append(keySeperator).Append(entry.Value);
			}
			return sb.ToString();
		}
	}
}