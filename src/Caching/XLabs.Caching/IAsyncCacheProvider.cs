// ***********************************************************************
// Assembly         : XLabs.Caching
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IAsyncCacheProvider.cs" company="XLabs Team">
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
using System.Threading.Tasks;

#endregion

namespace XLabs.Caching
{
    /// <summary>
    /// Async caching interface.
    /// </summary>
    public interface IAsyncCacheProvider : ICacheProvider
    {
        /// <summary>
        /// Adds a new item into the cache at the specified cache key only if the cache is empty.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item.</param>
        /// <param name="value">Item to add.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        Task<bool> AddAsync<T>(string key, T value);

        /// <summary>
        /// Adds a new item into the cache at the specified cache key only if the cache is empty.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item.</param>
        /// <param name="value">Item to add.</param>
        /// <param name="expiresAt">Expiration time.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        Task<bool> AddAsync<T>(string key, T value, DateTime expiresAt);

        /// <summary>
        /// Adds a new item into the cache at the specified cache key only if the cache is empty.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item.</param>
        /// <param name="value">Item to add.</param>
        /// <param name="expiresIn">Expiration timespan.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        Task<bool> AddAsync<T>(string key, T value, TimeSpan expiresIn);

        /// <summary>
        /// Retrieves the requested item from the cache.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Requested cache item.</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Gets all of the items in the cache related to the specified keys.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="keys">The keys.</param>
        /// <returns>A dictionary with thne requested items</returns>
        Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys);

        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item.</param>
        /// <param name="value">Item to set.</param>
        /// <returns>True if item was set, otherwise false.</returns>
        Task<bool> SetAsync<T>(string key, T value);

        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item.</param>
        /// <param name="value">Item to set.</param>
        /// <param name="expiresAt">Expiration time.</param>
        /// <returns>True if item was set, otherwise false.</returns>
        Task<bool> SetAsync<T>(string key, T value, DateTime expiresAt);

        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item.</param>
        /// <param name="value">Item to set.</param>
        /// <param name="expiresIn">Expiration timespan.</param>
        /// <returns>True if item was set, otherwise false.</returns>
        Task<bool> SetAsync<T>(string key, T value, TimeSpan expiresIn);

        /// <summary>
        /// Replaces the item at the cache.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item to replace.</param>
        /// <param name="value">Item to replace with.</param>
        /// <returns>True if the item exists, otherwise false.</returns>
        Task<bool> ReplaceAsync<T>(string key, T value);

        /// <summary>
        /// Replaces the item at the cache.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item to replace.</param>
        /// <param name="value">Item to replace with.</param>
        /// <param name="expiresAt">Expiration time.</param>
        /// <returns>True if the item exists, otherwise false.</returns>
        Task<bool> ReplaceAsync<T>(string key, T value, DateTime expiresAt);

        /// <summary>
        /// Replaces an item in the cache.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item to replace.</param>
        /// <param name="value">Item to replace with.</param>
        /// <param name="expiresIn">Expiration timespan.</param>
        /// <returns>True if item was replaced, otherwise false.</returns>
        Task<bool> ReplaceAsync<T>(string key, T value, TimeSpan expiresIn);

        /// <summary>
        /// Removes the requested item from the cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// Removes all items in the cache with the specified keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> RemoveAllAsync(IEnumerable<string> keys);

        /// <summary>
        /// Flushes the entire cache (and clears it).
        /// </summary>
        /// <returns>Retrurns true on success</returns>
        Task<bool> FlushAllAsync();
    }
}
