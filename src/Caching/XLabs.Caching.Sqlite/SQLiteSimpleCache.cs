// ***********************************************************************
// Assembly         : XLabs.Caching.SQLite
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="SQLiteSimpleCache.cs" company="XLabs Team">
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
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using XLabs.Serialization;

#endregion

namespace XLabs.Caching.SQLite
{
    /// <summary>
    /// Implements <see cref="IAsyncCacheProvider" /> caching interface
    /// using SQLite.Async.Pcl library.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class SQLiteSimpleCache : SQLiteConnectionWithLock, IAsyncCacheProvider
    {
        /// <summary>
        /// The serializer
        /// </summary>
        private readonly IByteSerializer _serializer;
        /// <summary>
        /// The asynchronous connection
        /// </summary>
        private readonly SQLiteAsyncConnection _asyncConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteSimpleCache" /> class.
        /// </summary>
        /// <param name="platform">SQLite platform.</param>
        /// <param name="connection">SQLite connection string.</param>
        /// <param name="defaultSerializer">Byte serializer to use.</param>
        public SQLiteSimpleCache(ISQLitePlatform platform, SQLiteConnectionString connection, IByteSerializer defaultSerializer)
            : base(platform, connection)
        {
            CreateTable<SQliteCacheTable>();
            _serializer = defaultSerializer;
            _asyncConnection = new SQLiteAsyncConnection(() => this);
        }

        #region ICacheProvider Members

        /// <summary>
        /// Removes the specified item from the cache.
        /// </summary>
        /// <param name="key">The identifier for the item to delete.</param>
        /// <returns>True if the item was successfully removed from the cache; false otherwise.</returns>
        public bool Remove(string key)
        {
            return Delete<SQliteCacheTable>(key) == 1;
        }

        /// <summary>
        /// Removes the cache for all the keys provided.
        /// </summary>
        /// <param name="keys">The keys to remove.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool RemoveAll(IEnumerable<string> keys)
        {
            var enumerable = keys.Select(Remove);

            return true;
        }

        /// <summary>
        /// Retrieves the specified item from the cache.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">The identifier for the item to retrieve.</param>
        /// <returns>The retrieved item, or <value>null</value> if the key was not found.</returns>
        public T Get<T>(string key)
        {
            return GetObject<T>(Find<SQliteCacheTable>(key));
        }

        #region Add
        /// <summary>
        /// Adds a new item into the cache at the specified cache key only if the cache is empty.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="value">The object to be inserted into the cache.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        /// <remarks>The item does not expire unless it is removed due memory pressure.</remarks>
        public bool Add<T>(string key, T value)
        {
            return Insert(new SQliteCacheTable(key, GetBytes(value))) == 1;
        }

        /// <summary>
        /// Adds a new item into the cache at the specified cache key only if the cache is empty.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="value">The object to be inserted into the cache.</param>
        /// <param name="expiresIn">The expires in.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <remarks>The item does not expire unless it is removed due memory pressure.</remarks>
        public bool Add<T>(string key, T value, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new item into the cache at the specified cache key only if the cache is empty.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="value">The object to be inserted into the cache.</param>
        /// <param name="expiresAt">The expires at.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <remarks>The item does not expire unless it is removed due memory pressure.</remarks>
        public bool Add<T>(string key, T value, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }
        #endregion Add

        #region Set
        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item.</param>
        /// <param name="value">Item to set.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        public bool Set<T>(string key, T value)
        {
            var n = InsertOrReplace(new SQliteCacheTable(key, GetBytes(value)));
            return n == 1;
        }

        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item.</param>
        /// <param name="value">Item to set.</param>
        /// <param name="expiresIn">The expires in.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Set<T>(string key, T value, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item.</param>
        /// <param name="value">Item to set.</param>
        /// <param name="expiresAt">The expires at.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Set<T>(string key, T value, DateTime expiresAt)
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
            return Remove(key) && Add(key, value);
        }

        /// <summary>
        /// Replaces the item at the cache.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item to replace.</param>
        /// <param name="value">Item to replace with.</param>
        /// <param name="expiresIn">The expires in.</param>
        /// <returns>True if the item exists, otherwise false.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Replace<T>(string key, T value, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Replaces the item at the cache.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item to replace.</param>
        /// <param name="value">Item to replace with.</param>
        /// <param name="expiresAt">The expires at.</param>
        /// <returns>True if the item exists, otherwise false.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Replace<T>(string key, T value, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }
        #endregion Replace

        /// <summary>
        /// Invalidates all data on the cache.
        /// </summary>
        /// <returns>Retrurns true on success</returns>
        public virtual bool FlushAll()
        {
            DeleteAll<SQliteCacheTable>();

            return true;
        }

        /// <summary>
        /// Retrieves multiple items from the cache.
        /// The default value of T is set for all keys that do not exist.
        /// </summary>
        /// <typeparam name="T">Type of values to get.</typeparam>
        /// <param name="keys">The list of identifiers for the items to retrieve.</param>
        /// <returns>a Dictionary holding all items indexed by their key.</returns>
        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            return keys.Select(a => new { Key = a, Item = Get<T>(a) }).Where(a => a.Item != null).ToDictionary(item => item.Key, item => item.Item);
        }

        /// <summary>
        /// Sets multiple items to the cache.
        /// </summary>
        /// <typeparam name="T">Type of values to set.</typeparam>
        /// <param name="values">The values.</param>
        public void SetAll<T>(IDictionary<string, T> values)
        {
            var enumerable = values.Select(pair => Set(pair.Key, pair.Value));
        }

        #endregion

        /// <summary>
        /// Gets the object from the table.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="item">Table item.</param>
        /// <returns>Object T if item found, otherwise default{T}.</returns>
        protected virtual T GetObject<T>(SQliteCacheTable item)
        {
            return (item != null) ? _serializer.Deserialize<T>(item.Blob) : default(T);
        }

        /// <summary>
        /// Gets bytes from the object.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="obj">Object to turn into bytes.</param>
        /// <returns>Byte array.</returns>
        protected virtual byte[] GetBytes<T>(T obj)
        {
            return _serializer.SerializeToBytes(obj);
        }

        /// <summary>
        /// Class SQliteCacheTable.
        /// </summary>
        protected class SQliteCacheTable
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SQliteCacheTable" /> class.
            /// </summary>
            public SQliteCacheTable()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SQliteCacheTable" /> class.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="blob">The BLOB.</param>
            public SQliteCacheTable(string key, byte[] blob)
            {
                Key = key;
                Blob = blob;
            }

            /// <summary>
            /// Gets or sets the key.
            /// </summary>
            /// <value>The key.</value>
            [PrimaryKey]
            public string Key { get; set; }
            /// <summary>
            /// Gets or sets the BLOB.
            /// </summary>
            /// <value>The BLOB.</value>
            public byte[] Blob { get; set; }
        }

        #region IAsyncSimpleCache Members

        /// <summary>
        /// Removes the specified item from the cache.
        /// </summary>
        /// <param name="key">The identifier for the item to delete.</param>
        /// <returns>True if the item was successfully removed from the cache; false otherwise.</returns>
        public async Task<bool> RemoveAsync(string key)
        {
            var count = await _asyncConnection.DeleteAsync<SQliteCacheTable>(key);
            return count == 1;
        }

        /// <summary>
        /// Removes the cache for all the keys provided.
        /// </summary>
        /// <param name="keys">The keys to remove.</param>
        /// <returns>Task.</returns>
        public async Task<bool> RemoveAllAsync(IEnumerable<string> keys)
        {
            await Task.WhenAll(keys.Select(RemoveAsync));

            return true;
        }

        /// <summary>
        /// Retrieves the specified item from the cache.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">The identifier for the item to retrieve.</param>
        /// <returns>The retrieved item, or <value>null</value> if the key was not found.</returns>
        public async Task<T> GetAsync<T>(string key)
        {
            var item = await _asyncConnection.FindAsync<SQliteCacheTable>(key);

            return GetObject<T>(item);
        }

        #region Add
        /// <summary>
        /// Adds a new item into the cache at the specified cache key only if the cache is empty.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="value">The object to be inserted into the cache.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        /// <remarks>The item does not expire unless it is removed due memory pressure.</remarks>
        public async Task<bool> AddAsync<T>(string key, T value)
        {
            var count = await _asyncConnection.InsertAsync(new SQliteCacheTable(key, GetBytes(value)));
            return count == 1;
        }

        /// <summary>
        /// Adds a new item into the cache at the specified cache key only if the cache is empty.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="value">The object to be inserted into the cache.</param>
        /// <param name="expiresIn">The expires in.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <remarks>The item does not expire unless it is removed due memory pressure.</remarks>
        public async Task<bool> AddAsync<T>(string key, T value, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new item into the cache at the specified cache key only if the cache is empty.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="value">The object to be inserted into the cache.</param>
        /// <param name="expiresAt">The expires at.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <remarks>The item does not expire unless it is removed due memory pressure.</remarks>
        public async Task<bool> AddAsync<T>(string key, T value, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }
        #endregion Add

        #region Set
        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item.</param>
        /// <param name="value">Item to set.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        public async Task<bool> SetAsync<T>(string key, T value)
        {
            var n = await _asyncConnection.InsertOrReplaceAsync(new SQliteCacheTable(key, GetBytes(value)));
            return n == 1;
        }

        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item.</param>
        /// <param name="value">Item to set.</param>
        /// <param name="expiresIn">The expires in.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item.</param>
        /// <param name="value">Item to set.</param>
        /// <param name="expiresAt">The expires at.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<bool> SetAsync<T>(string key, T value, DateTime expiresAt)
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
        public async Task<bool> ReplaceAsync<T>(string key, T value)
        {
            return await RemoveAsync(key) && await AddAsync(key, value);
        }

        /// <summary>
        /// Replaces the item at the cache.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item to replace.</param>
        /// <param name="value">Item to replace with.</param>
        /// <param name="expiresIn">The expires in.</param>
        /// <returns>True if the item exists, otherwise false.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<bool> ReplaceAsync<T>(string key, T value, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Replaces the item at the cache.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item to replace.</param>
        /// <param name="value">Item to replace with.</param>
        /// <param name="expiresAt">The expires at.</param>
        /// <returns>True if the item exists, otherwise false.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<bool> ReplaceAsync<T>(string key, T value, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }
        #endregion Replace 

        /// <summary>
        /// Invalidates all data on the cache.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task<bool> FlushAllAsync()
        {
            await _asyncConnection.DeleteAllAsync<SQliteCacheTable>();

            return true;
        }

        /// <summary>
        /// Retrieves multiple items from the cache.
        /// The default value of T is set for all keys that do not exist.
        /// </summary>
        /// <typeparam name="T">Type of values to get.</typeparam>
        /// <param name="keys">The list of identifiers for the items to retrieve.</param>
        /// <returns>a Dictionary holding all items indexed by their key.</returns>
        public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys)
        {
            var dict = new Dictionary<string, T>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var item in keys.Select(a => new { Key = a, Item = GetAsync<T>(a) }).Where(a => a.Item != null))
            {
                dict.Add(item.Key, await item.Item);
            }

            return dict;
        }

        /// <summary>
        /// Sets multiple items to the cache.
        /// </summary>
        /// <typeparam name="T">Type of values to set.</typeparam>
        /// <param name="values">The values.</param>
        /// <returns>Task.</returns>
        public Task SetAllAsync<T>(IDictionary<string, T> values)
        {
            return Task.WhenAll(values.Select(value => SetAsync(value.Key, value.Value)));
        }

        #endregion
    }
}
