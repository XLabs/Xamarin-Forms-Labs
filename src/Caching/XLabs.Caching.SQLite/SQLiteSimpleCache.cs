using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using XLabs.Serialization;

namespace XLabs.Caching.SQLite
{
    /// <summary>
    /// Implements <see cref="ISimpleCache"/> and <see cref="IAsyncSimpleCache"/> caching interfaces
    /// using SQLite.Async.Pcl library.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class SQLiteSimpleCache : SQLiteConnectionWithLock, ISimpleCache, IAsyncSimpleCache
    {
        private readonly IByteSerializer serializer;
        private readonly SQLiteAsyncConnection asyncConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteSimpleCache"/> class.
        /// </summary>
        /// <param name="platform">SQLite platform.</param>
        /// <param name="connection">SQLite connection string.</param>
        /// <param name="defaultSerializer">Byte serializer to use.</param>
        public SQLiteSimpleCache(ISQLitePlatform platform, SQLiteConnectionString connection, IByteSerializer defaultSerializer)
            : base(platform, connection)
        {
            this.CreateTable<SQliteCacheTable>();
            this.serializer = defaultSerializer;
            this.asyncConnection = new SQLiteAsyncConnection(() => this);
        }

        #region ICacheProvider Members

        /// <summary>
        /// Removes the specified item from the cache.
        /// </summary>
        /// <param name="key">The identifier for the item to delete.</param>
        /// <returns>
        /// True if the item was successfully removed from the cache; false otherwise.
        /// </returns>
        public bool Remove(string key)
        {
            return this.Delete<SQliteCacheTable>(key) == 1;
        }

        /// <summary>
        /// Removes the cache for all the keys provided.
        /// </summary>
        /// <param name="keys">The keys to remove.</param>
        public void RemoveAll(IEnumerable<string> keys)
        {
            keys.Select(this.Remove);
        }

        /// <summary>
        /// Retrieves the specified item from the cache.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">The identifier for the item to retrieve.</param>
        /// <returns>
        /// The retrieved item, or <value>null</value> if the key was not found.
        /// </returns>
        public T Get<T>(string key)
        {
            return this.GetObject<T>(this.Find<SQliteCacheTable>(key));
        }

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
            return this.Insert(new SQliteCacheTable(key, this.GetBytes(value))) == 1;
        }

        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item.</param>
        /// <param name="value">Item to set.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        public bool Set<T>(string key, T value)
        {
            var n = this.InsertOrReplace(new SQliteCacheTable(key, this.GetBytes(value)));
            return n == 1;
        }

        /// <summary>
        /// Replaces the item at the cache.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item to replace.</param>
        /// <param name="value">Item to replace with.</param>
        /// <returns>True if the item exists, otherwise false.</returns>
        public bool Replace<T>(string key, T value)
        {
            return this.Remove(key) && this.Add(key, value);
        }

        /// <summary>
        /// Invalidates all data on the cache.
        /// </summary>
        public virtual void FlushAll()
        {
            this.DeleteAll<SQliteCacheTable>();
        }

        /// <summary>
        /// Retrieves multiple items from the cache. 
        /// The default value of T is set for all keys that do not exist.
        /// </summary>
        /// <typeparam name="T">Type of values to get.</typeparam>
        /// <param name="keys">The list of identifiers for the items to retrieve.</param>
        /// <returns>
        /// a Dictionary holding all items indexed by their key.
        /// </returns>
        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            return keys.Select(a => new { Key = a, Item = this.Get<T>(a) }).Where(a => a.Item != null).ToDictionary(item => item.Key, item => item.Item);
        }

        /// <summary>
        /// Sets multiple items to the cache. 
        /// </summary>
        /// <typeparam name="T">Type of values to set.</typeparam>
        /// <param name="values">The values.</param>
        public void SetAll<T>(IDictionary<string, T> values)
        {
            foreach (var value in values)
            {
                this.Set<T>(value.Key, value.Value);
            }
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
            return (item != null) ? this.serializer.Deserialize<T>(item.Blob) : default(T);
        }

        /// <summary>
        /// Gets bytes from the object.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="obj">Object to turn into bytes.</param>
        /// <returns>Byte array.</returns>
        protected virtual byte[] GetBytes<T>(T obj)
        {
            return this.serializer.SerializeToBytes(obj);
        }

        protected class SQliteCacheTable
        {
            public SQliteCacheTable()
            {
            }

            public SQliteCacheTable(string key, byte[] blob)
            {
                this.Key = key;
                this.Blob = blob;
            }

            [PrimaryKey]
            public string Key { get; set; }
            public byte[] Blob { get; set; }
        }

        #region IAsyncSimpleCache Members

        /// <summary>
        /// Removes the specified item from the cache.
        /// </summary>
        /// <param name="key">The identifier for the item to delete.</param>
        /// <returns>
        /// True if the item was successfully removed from the cache; false otherwise.
        /// </returns>
        public async Task<bool> RemoveAsync(string key)
        {
            var count = await this.asyncConnection.DeleteAsync<SQliteCacheTable>(key);
            return count == 1;
        }

        /// <summary>
        /// Removes the cache for all the keys provided.
        /// </summary>
        /// <param name="keys">The keys to remove.</param>
        public async Task RemoveAllAsync(IEnumerable<string> keys)
        {
            await Task.WhenAll(keys.Select(this.RemoveAsync));
        }

        /// <summary>
        /// Retrieves the specified item from the cache.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">The identifier for the item to retrieve.</param>
        /// <returns>
        /// The retrieved item, or <value>null</value> if the key was not found.
        /// </returns>
        public async Task<T> GetAsync<T>(string key)
        {
            var item = await this.asyncConnection.FindAsync<SQliteCacheTable>(key);

            return this.GetObject<T>(item);
        }

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
            var count = await this.asyncConnection.InsertAsync(new SQliteCacheTable(key, this.GetBytes(value)));
            return count == 1;
        }

        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item.</param>
        /// <param name="value">Item to set.</param>
        /// <returns>True if item was added, otherwise false.</returns>
        public async Task<bool> SetAsync<T>(string key, T value)
        {
            var n = await this.asyncConnection.InsertOrReplaceAsync(new SQliteCacheTable(key, this.GetBytes(value)));
            return n == 1;
        }

        /// <summary>
        /// Replaces the item at the cache.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key for the item to replace.</param>
        /// <param name="value">Item to replace with.</param>
        /// <returns>True if the item exists, otherwise false.</returns>
        public async Task<bool> ReplaceAsync<T>(string key, T value)
        {
            return await this.RemoveAsync(key) && await this.AddAsync(key, value);
        }

        /// <summary>
        /// Invalidates all data on the cache.
        /// </summary>
        public async Task FlushAllAsync()
        {
            await this.asyncConnection.DeleteAllAsync<SQliteCacheTable>();
        }

        /// <summary>
        /// Retrieves multiple items from the cache. 
        /// The default value of T is set for all keys that do not exist.
        /// </summary>
        /// <typeparam name="T">Type of values to get.</typeparam>
        /// <param name="keys">The list of identifiers for the items to retrieve.</param>
        /// <returns>
        /// a Dictionary holding all items indexed by their key.
        /// </returns>
        public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys)
        {
            var dict = new Dictionary<string, T>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var item in keys.Select(a => new { Key = a, Item = this.GetAsync<T>(a) }).Where(a => a.Item != null))
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
        public Task SetAllAsync<T>(IDictionary<string, T> values)
        {
            return Task.WhenAll(values.Select(value => this.SetAsync<T>(value.Key, value.Value)));
        }

        #endregion
    }
}
