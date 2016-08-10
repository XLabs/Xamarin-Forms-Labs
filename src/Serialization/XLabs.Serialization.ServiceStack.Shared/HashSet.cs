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
//*****************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if NETFX_CORE
namespace ServiceStack.Text.WinRT
#else
namespace ServiceStack.Text.WP
#endif
{
	/// <summary>
	/// A hashset implementation that uses an IDictionary
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class HashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
		/// <summary>
		/// The _dict
		/// </summary>
		private readonly Dictionary<T, short> _dict;

		/// <summary>
		/// Initializes a new instance of the <see cref="HashSet{T}"/> class.
		/// </summary>
		public HashSet()
        {
            _dict = new Dictionary<T, short>();
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="HashSet{T}"/> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <exception cref="System.ArgumentNullException">collection</exception>
		public HashSet(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            _dict =  new Dictionary<T, short>(collection.Count());
            foreach (T item in collection)
                Add(item);
        }

		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Add(T item)
        {
            _dict.Add(item, 0);
        }

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		public void Clear()
        {
            _dict.Clear();
        }

		/// <summary>
		/// Determines whether [contains] [the specified item].
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
		public bool Contains(T item)
        {
            return _dict.ContainsKey(item);
        }

		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Index of the array.</param>
		public void CopyTo(T[] array, int arrayIndex)
        {
            _dict.Keys.CopyTo(array, arrayIndex);
        }

		/// <summary>
		/// Removes the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Remove(T item)
        {
            return _dict.Remove(item);
        }

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		public IEnumerator<T> GetEnumerator()
        {
            return _dict.Keys.GetEnumerator();
        }

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
        {
            return _dict.Keys.GetEnumerator();
        }

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <value>The count.</value>
		public int Count
        {
            get { return _dict.Keys.Count(); }
        }

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
		/// </summary>
		/// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
		public bool IsReadOnly
        {
            get { return false; }
        }
    }
}
