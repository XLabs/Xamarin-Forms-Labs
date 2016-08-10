// ***********************************************************************
// Assembly         : XLabs.Caching
// Author           : XLabs Team
// Created          : 12-28-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="SimpleCacheProvider.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 
#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace XLabs.Caching
{
	/// <summary>
	/// Class SimpleCacheProvider.
	/// </summary>
	public class SimpleCacheProvider : IAsyncCacheProvider
	{
		/// <summary>
		/// The _cache
		/// </summary>
		private readonly IDictionary<string, object> _cache = new Dictionary<string, object>();

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{			
		}

		#region ICacheProvider

		#region Add
		/// <summary>
		/// Adds a new item into the cache at the specified cache key only if the cache is empty.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item.</param>
		/// <param name="value">Item to add.</param>
		/// <returns>True if item was added, otherwise false.</returns>
		public bool Add<T>(string key, T value)
		{
			return Set(key, value);
		}

		/// <summary>
		/// Adds a new item into the cache at the specified cache key only if the cache is empty.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item.</param>
		/// <param name="value">Item to add.</param>
		/// <param name="expiresAt">Expiration time.</param>
		/// <returns>True if item was added, otherwise false.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public bool Add<T>(string key, T value, DateTime expiresAt)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Adds a new item into the cache at the specified cache key only if the cache is empty.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item.</param>
		/// <param name="value">Item to add.</param>
		/// <param name="expiresIn">Expiration timespan.</param>
		/// <returns>True if item was added, otherwise false.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public bool Add<T>(string key, T value, TimeSpan expiresIn)
		{
			throw new NotImplementedException();
		}
		#endregion Add

		#region Get
		/// <summary>
		/// Retrieves the requested item from the cache.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">The key.</param>
		/// <returns>Requested cache item.</returns>
		public T Get<T>(string key)
		{
			object value;

			if (!_cache.TryGetValue(key, out value))
				return default(T);

			return (T) value;
		}

		/// <summary>
		/// Gets all of the items in the cache related to the specified keys.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="keys">The keys.</param>
		/// <returns>A dictionary with thne requested items</returns>
		public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
		{
			if (keys == null) return null;

			return _cache.Where(x => keys.Contains(x.Key)).ToDictionary(k => k.Key, v => (T)v.Value);
		}
		#endregion Get

		#region Set
		/// <summary>
		/// Sets an item into the cache at the cache key specified regardless if it already exists or not.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item.</param>
		/// <param name="value">Item to set.</param>
		/// <returns>True if item was set, otherwise false.</returns>
		public bool Set<T>(string key, T value)
		{
			_cache[key] = value;

			return true;
		}

		/// <summary>
		/// Sets an item into the cache at the cache key specified regardless if it already exists or not.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item.</param>
		/// <param name="value">Item to set.</param>
		/// <param name="expiresAt">Expiration time.</param>
		/// <returns>True if item was set, otherwise false.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public bool Set<T>(string key, T value, DateTime expiresAt)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Sets an item into the cache at the cache key specified regardless if it already exists or not.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item.</param>
		/// <param name="value">Item to set.</param>
		/// <param name="expiresIn">Expiration timespan.</param>
		/// <returns>True if item was set, otherwise false.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public bool Set<T>(string key, T value, TimeSpan expiresIn)
		{
			throw new NotImplementedException();
		}
		#endregion Set

		#region Replace
		/// <summary>
		/// Replaces the item at the cache.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item to replace.</param>
		/// <param name="value">Item to replace with.</param>
		/// <returns>True if the item exists, otherwise false.</returns>
		public bool Replace<T>(string key, T value)
		{
			return Set(key, value);
		}

		/// <summary>
		/// Replaces the item at the cache.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item to replace.</param>
		/// <param name="value">Item to replace with.</param>
		/// <param name="expiresAt">Expiration time.</param>
		/// <returns>True if the item exists, otherwise false.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public bool Replace<T>(string key, T value, DateTime expiresAt)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Replaces an item in the cache.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item to replace.</param>
		/// <param name="value">Item to replace with.</param>
		/// <param name="expiresIn">Expiration timespan.</param>
		/// <returns>True if item was replaced, otherwise false.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public bool Replace<T>(string key, T value, TimeSpan expiresIn)
		{
			throw new NotImplementedException();
		}
		#endregion Replace

		#region Remove
		/// <summary>
		/// Removes the requested item from the cache.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Retrurns true on success</returns>
		public bool Remove(string key)
		{
			if (_cache.ContainsKey(key))
			{
				return _cache.Remove(key);
			}

			return false;
		}

		/// <summary>
		/// Removes all items in the cache with the specified keys.
		/// </summary>
		/// <param name="keys">The keys.</param>
		/// <returns>Retrurns true on success</returns>
		public bool RemoveAll(IEnumerable<string> keys)
		{
			if (keys == null) return false;

			var enumerable = keys.Select(Remove);

			return true;
		}
		#endregion Remove

		/// <summary>
		/// Flushes the entire cache (and clears it).
		/// </summary>
		/// <returns>Retrurns true on success</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public bool FlushAll()
		{
			throw new NotImplementedException();
		}
		#endregion ICacheProvider

		#region IAsyncCacheProvider
		#region Add
		/// <summary>
		/// add as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item.</param>
		/// <param name="value">Item to add.</param>
		/// <returns>True if item was added, otherwise false.</returns>
		public async Task<bool> AddAsync<T>(string key, T value)
		{
			return await Task.Factory.StartNew(() => Add(key, value));
		}

		/// <summary>
		/// add as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item.</param>
		/// <param name="value">Item to add.</param>
		/// <param name="expiresAt">Expiration time.</param>
		/// <returns>True if item was added, otherwise false.</returns>
		public async Task<bool> AddAsync<T>(string key, T value, DateTime expiresAt)
		{
			return await Task.Factory.StartNew(() => Add(key, value, expiresAt));
		}

		/// <summary>
		/// add as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item.</param>
		/// <param name="value">Item to add.</param>
		/// <param name="expiresIn">Expiration timespan.</param>
		/// <returns>True if item was added, otherwise false.</returns>
		public async Task<bool> AddAsync<T>(string key, T value, TimeSpan expiresIn)
		{
			return await Task.Factory.StartNew(() => Add(key, value, expiresIn));
		}
		#endregion Add

		#region Get
		/// <summary>
		/// get as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">The key.</param>
		/// <returns>Requested cache item.</returns>
		public async Task<T> GetAsync<T>(string key)
		{
			return await Task.Factory.StartNew(() => Get<T>(key));
		}

		/// <summary>
		/// get all as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="keys">The keys.</param>
		/// <returns>A dictionary with thne requested items</returns>
		public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys)
		{
			return await Task.Factory.StartNew(() => GetAll<T>(keys));
		}
		#endregion Get

		#region Set
		/// <summary>
		/// set as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item.</param>
		/// <param name="value">Item to set.</param>
		/// <returns>True if item was set, otherwise false.</returns>
		public async Task<bool> SetAsync<T>(string key, T value)
		{
			return await Task.Factory.StartNew(() => Set(key, value));
		}

		/// <summary>
		/// set as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item.</param>
		/// <param name="value">Item to set.</param>
		/// <param name="expiresAt">Expiration time.</param>
		/// <returns>True if item was set, otherwise false.</returns>
		public async Task<bool> SetAsync<T>(string key, T value, DateTime expiresAt)
		{
			return await Task.Factory.StartNew(() => Set(key, value, expiresAt));
		}

		/// <summary>
		/// set as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item.</param>
		/// <param name="value">Item to set.</param>
		/// <param name="expiresIn">Expiration timespan.</param>
		/// <returns>True if item was set, otherwise false.</returns>
		public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiresIn)
		{
			return await Task.Factory.StartNew(() => Set(key, value, expiresIn));
		}
		#endregion Set

		#region Replace
		/// <summary>
		/// replace as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item to replace.</param>
		/// <param name="value">Item to replace with.</param>
		/// <returns>True if the item exists, otherwise false.</returns>
		public async Task<bool> ReplaceAsync<T>(string key, T value)
		{
			return await Task.Factory.StartNew(() => Replace(key, value));
		}

		/// <summary>
		/// replace as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item to replace.</param>
		/// <param name="value">Item to replace with.</param>
		/// <param name="expiresAt">Expiration time.</param>
		/// <returns>True if the item exists, otherwise false.</returns>
		public async Task<bool> ReplaceAsync<T>(string key, T value, DateTime expiresAt)
		{
			return await Task.Factory.StartNew(() => Replace(key, value, expiresAt));
		}

		/// <summary>
		/// replace as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="key">Key for the item to replace.</param>
		/// <param name="value">Item to replace with.</param>
		/// <param name="expiresIn">Expiration timespan.</param>
		/// <returns>True if item was replaced, otherwise false.</returns>
		public async Task<bool> ReplaceAsync<T>(string key, T value, TimeSpan expiresIn)
		{
			return await Task.Factory.StartNew(() => Replace(key, value, expiresIn));
		}
		#endregion Replace

		#region Remove
		/// <summary>
		/// remove as an asynchronous operation.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public async Task<bool> RemoveAsync(string key)
		{
			return await Task.Factory.StartNew(() => Remove(key));
		}

		/// <summary>
		/// remove all as an asynchronous operation.
		/// </summary>
		/// <param name="keys">The keys.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public async Task<bool> RemoveAllAsync(IEnumerable<string> keys)
		{
			return await Task.Factory.StartNew(() => RemoveAll(keys));
		}
		#endregion Remove

		/// <summary>
		/// flush all as an asynchronous operation.
		/// </summary>
		/// <returns>Retrurns true on success</returns>
		public async Task<bool> FlushAllAsync()
		{
			return await Task.Factory.StartNew(FlushAll);
		}
		#endregion IAsyncCacheProvider
	}
}
