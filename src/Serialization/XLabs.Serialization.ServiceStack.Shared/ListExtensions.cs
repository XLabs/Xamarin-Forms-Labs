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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Text.Common;

namespace ServiceStack.Text
{
	/// <summary>
	/// Class ListExtensions.
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		/// Joins the specified values.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="values">The values.</param>
		/// <returns>System.String.</returns>
		public static string Join<T>(this IEnumerable<T> values)
		{
			return Join(values, JsWriter.ItemSeperatorString);
		}

		/// <summary>
		/// Joins the specified seperator.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="values">The values.</param>
		/// <param name="seperator">The seperator.</param>
		/// <returns>System.String.</returns>
		public static string Join<T>(this IEnumerable<T> values, string seperator)
		{
			var sb = new StringBuilder();
			foreach (var value in values)
			{
				if (sb.Length > 0)
					sb.Append(seperator);
				sb.Append(value);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Determines whether [is null or empty] [the specified list].
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <returns><c>true</c> if [is null or empty] [the specified list]; otherwise, <c>false</c>.</returns>
		public static bool IsNullOrEmpty<T>(this List<T> list)
		{
			return list == null || list.Count == 0;
		}

		//TODO: make it work
		/// <summary>
		/// Safes the where.
		/// </summary>
		/// <typeparam name="TFrom">The type of the t from.</typeparam>
		/// <param name="list">The list.</param>
		/// <param name="predicate">The predicate.</param>
		/// <returns>IEnumerable&lt;TFrom&gt;.</returns>
		public static IEnumerable<TFrom> SafeWhere<TFrom>(this List<TFrom> list, Func<TFrom, bool> predicate)
		{
			return list.Where(predicate);
		}

		/// <summary>
		/// Nullables the count.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <returns>System.Int32.</returns>
		public static int NullableCount<T>(this List<T> list)
		{
			return list == null ? 0 : list.Count;
		}

		/// <summary>
		/// Adds if not exists.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="item">The item.</param>
		public static void AddIfNotExists<T>(this List<T> list, T item)
		{
			if (!list.Contains(item))
				list.Add(item);
		}
	}
}