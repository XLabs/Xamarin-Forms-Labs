// ***********************************************************************
// Assembly         : XLabs.Caching.Net45
// Author           : XLabs Team
// Created          : 12-27-2015
//
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="MemoryCache.cs" company="XLabs Team">
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
using System.Runtime.Caching;
using System.Threading.Tasks;

#endregion

namespace XLabs.Caching.Net45
{
	/// <summary>
	/// A .NET MemoryCache if the XLabs IAsyncCacheProvider and ICacheProvider .
	/// </summary>
	public class MemoryCache : IAsyncCacheProvider
	{
		/// <summary>
		/// The actual cache
		/// </summary>
		private readonly ObjectCache _cache = System.Runtime.Caching.MemoryCache.Default;
		/// <summary>
		/// The default cache item policy
		/// </summary>
		private readonly CacheItemPolicy _cacheItemPolicy;

		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryCache"/> class.
		/// </summary>
		/// <param name="cacheItemPolicy">The cache item policy.</param>
		public MemoryCache(CacheItemPolicy cacheItemPolicy)
		{
			_cacheItemPolicy = cacheItemPolicy;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (_cache != null)
			{
				FlushAll();

				((System.Runtime.Caching.MemoryCache)_cache).Dispose();
			}
		}

		#region Async

		#region Get
		/// <summary>
		/// Gets the specified key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <returns>T.</returns>
		public T Get<T>(string key)
		{
			return (T) _cache.Get(key);
		}

		/// <summary>
		/// Gets all.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="keys">The keys.</param>
		/// <returns>IDictionary&lt;System.String, T&gt;.</returns>
		public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
		{
			if (keys == null) return null;

			var result = _cache.GetValues(keys)?.ToDictionary(k => k.Key, v => (T)v.Value);

			return result;
		}

		/// <summary>
		/// get as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <returns>Task&lt;T&gt;.</returns>
		public async Task<T> GetAsync<T>(string key)
		{
			return await Task.Factory.StartNew(() => Get<T>(key));
		}

		/// <summary>
		/// get all as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="keys">The keys.</param>
		/// <returns>Task&lt;IDictionary&lt;System.String, T&gt;&gt;.</returns>
		public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys)
		{
			return await Task.Factory.StartNew(() => GetAll<T>(keys));
		}
		#endregion Get

		#region Remove
		/// <summary>
		/// Removes the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Remove(string key)
		{
			_cache.Remove(key);

			return true;
		}

		/// <summary>
		/// Removes all.
		/// </summary>
		/// <param name="keys">The keys.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool RemoveAll(IEnumerable<string> keys)
		{
			if (keys == null) return false;

			var enumerable = keys.Select(Remove);

			return true;
		}


		/// <summary>
		/// remove as an asynchronous operation.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public async Task<bool> RemoveAsync(string key)
		{
			return await Task.Factory.StartNew(() =>
			{
				Remove(key);

				return true;
			});
		}

		/// <summary>
		/// remove all as an asynchronous operation.
		/// </summary>
		/// <param name="keys">The keys.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public async Task<bool> RemoveAllAsync(IEnumerable<string> keys)
		{
			var tasks = keys.Select(RemoveAsync);

			await Task.WhenAll(tasks);

			return true;
		}
		#endregion Remove

		#region Add
		/// <summary>
		/// Adds the specified key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Add<T>(string key, T value)
		{
			_cache.Add(key, value, _cacheItemPolicy);

			return true;
		}

		/// <summary>
		/// Adds the specified key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="expiresAt">The expires at.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Add<T>(string key, T value, DateTime expiresAt)
		{
			_cache.Add(key, value, expiresAt);

			return true;
		}

		/// <summary>
		/// Adds the specified key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="expiresIn">The expires in.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Add<T>(string key, T value, TimeSpan expiresIn)
		{
			_cache.Add(key, value, DateTime.Now + expiresIn);

			return true;
		}

		/// <summary>
		/// add as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public async Task<bool> AddAsync<T>(string key, T value)
		{
			return await Task.Factory.StartNew(() => Add(key, value));
		}

		/// <summary>
		/// add as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="expiresIn">The expires in.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public async Task<bool> AddAsync<T>(string key, T value, TimeSpan expiresIn)
		{
			return await Task.Factory.StartNew(() => Add(key, value, DateTime.Now + expiresIn));
		}

		/// <summary>
		/// add as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="expiresAt">The expires at.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public async Task<bool> AddAsync<T>(string key, T value, DateTime expiresAt)
		{
			return await Task.Factory.StartNew(() => Add(key, value, expiresAt));
		}
		#endregion Add

		#region Set
		/// <summary>
		/// Sets the specified key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Set<T>(string key, T value)
		{
			_cache.Set(key, value, _cacheItemPolicy);

			return true;
		}

		/// <summary>
		/// Sets the specified key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="expiresAt">The expires at.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Set<T>(string key, T value, DateTime expiresAt)
		{
			_cache.Set(key, value, expiresAt);

			return true;
		}

		/// <summary>
		/// Sets the specified key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="expiresIn">The expires in.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Set<T>(string key, T value, TimeSpan expiresIn)
		{
			_cache.Set(key, value, DateTime.Now + expiresIn);

			return true;
		}

		/// <summary>
		/// set as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public async Task<bool> SetAsync<T>(string key, T value)
		{
			return await Task.Factory.StartNew(() => Set(key, value));
		}

		/// <summary>
		/// set as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="expiresIn">The expires in.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiresIn)
		{
			return await Task.Factory.StartNew(() => Set(key, value, expiresIn));
		}

		/// <summary>
		/// set as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="expiresAt">The expires at.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public async Task<bool> SetAsync<T>(string key, T value, DateTime expiresAt)
		{
			return await Task.Factory.StartNew(() => Set(key, value, expiresAt));
		}

		/// <summary>
		/// set all as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="values">The values.</param>
		/// <returns>Task.</returns>
		public async Task SetAllAsync<T>(IDictionary<string, T> values)
		{
			var tasks = values.Select(x => SetAsync(x.Key, x.Value));

			await Task.WhenAll(tasks);
		}
		#endregion Set

		#region Replace
		/// <summary>
		/// Replaces the specified key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Replace<T>(string key, T value)
		{
			return Set(key, value);
		}

		/// <summary>
		/// Replaces the specified key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="expiresAt">The expires at.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Replace<T>(string key, T value, DateTime expiresAt)
		{
			return Set(key, value, expiresAt);
		}

		/// <summary>
		/// Replaces the specified key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="expiresIn">The expires in.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Replace<T>(string key, T value, TimeSpan expiresIn)
		{
			return Set(key, value, expiresIn);
		}

		/// <summary>
		/// replace as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public async Task<bool> ReplaceAsync<T>(string key, T value)
		{
			return await SetAsync(key, value);
		}

		/// <summary>
		/// replace as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="expiresAt">The expires at.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public async Task<bool> ReplaceAsync<T>(string key, T value, DateTime expiresAt)
		{
			return await SetAsync(key, value, expiresAt);
		}

		/// <summary>
		/// replace as an asynchronous operation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="expiresIn">The expires in.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public async Task<bool> ReplaceAsync<T>(string key, T value, TimeSpan expiresIn)
		{
			return await SetAsync(key, value, DateTime.Now + expiresIn);
		}
		#endregion Replace

		#region Flush
		/// <summary>
		/// Flushes the entire cache (and clears it).
		/// </summary>
		/// <returns>Retrurns true on success</returns>
		public bool FlushAll()
		{
			return true;
		}

		/// <summary>
		/// flush all as an asynchronous operation.
		/// </summary>
		/// <returns>Retrurns true on success</returns>
		public async Task<bool> FlushAllAsync()
		{
			return true;
		}
		#endregion Flush
		#endregion Async
	}
}
