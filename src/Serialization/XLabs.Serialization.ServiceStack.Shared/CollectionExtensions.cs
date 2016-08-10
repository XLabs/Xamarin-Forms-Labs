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

namespace ServiceStack.Text
{
	/// <summary>
	/// Class CollectionExtensions.
	/// </summary>
	public static class CollectionExtensions
    {
		/// <summary>
		/// Creates the and populate.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="ofCollectionType">Type of the of collection.</param>
		/// <param name="withItems">The with items.</param>
		/// <returns>ICollection&lt;T&gt;.</returns>
		public static ICollection<T> CreateAndPopulate<T>(Type ofCollectionType, T[] withItems)
        {
            if (ofCollectionType == null) return new List<T>(withItems);

            var genericType = ofCollectionType.GetGenericType();
            var genericTypeDefinition = genericType != null
                ? genericType.GetGenericTypeDefinition()
                : null;
#if !XBOX
            if (genericTypeDefinition == typeof(HashSet<T>))
                return new HashSet<T>(withItems);
#endif
            if (genericTypeDefinition == typeof(LinkedList<T>))
                return new LinkedList<T>(withItems);

            var collection = (ICollection<T>)ofCollectionType.CreateInstance();
            foreach (var item in withItems)
            {
                collection.Add(item);
            }
            return collection;
        }

		/// <summary>
		/// To the array.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>T[].</returns>
		public static T[] ToArray<T>(this ICollection<T> collection)
        {
            var to = new T[collection.Count];
            collection.CopyTo(to, 0);
            return to;
        }

		/// <summary>
		/// Converts the specified object collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="objCollection">The object collection.</param>
		/// <param name="toCollectionType">Type of to collection.</param>
		/// <returns>System.Object.</returns>
		public static object Convert<T>(object objCollection, Type toCollectionType)
        {
            var collection = (ICollection<T>) objCollection;
            var to = new T[collection.Count];
            collection.CopyTo(to, 0);
            return CreateAndPopulate(toCollectionType, to);
        }
    }
}