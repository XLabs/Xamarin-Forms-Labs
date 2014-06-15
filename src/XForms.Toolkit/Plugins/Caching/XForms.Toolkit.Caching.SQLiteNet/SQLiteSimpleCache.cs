using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using XForms.Toolkit.Services;
using XForms.Toolkit.Services.Serialization;

namespace XForms.Toolkit.Caching.SQLiteNet
{
    public class SQLiteSimpleCache : SQLiteConnectionWithLock, ISimpleCache
    {
        private IByteSerializer serializer;

        public SQLiteSimpleCache(ISQLitePlatform platform, SQLiteConnectionString connection, IByteSerializer defaultSerializer)
            : base(platform, connection)
        {
            this.CreateTable<SQliteCacheTable>();
            this.serializer = defaultSerializer;
        }

        #region ICacheProvider Members

        public bool Remove(string key)
        {
            return this.Delete<SQliteCacheTable>(key) == 1;
        }

        public void RemoveAll(IEnumerable<string> keys)
        {
            keys.Select(a => this.Remove(a));
        }

        public T Get<T>(string key)
        {
            var items = this.Find<SQliteCacheTable>(key);
            return this.GetObject<T>(this.Find<SQliteCacheTable>(key));
        }

        public bool Add<T>(string key, T value)
        {
            return this.Insert(new SQliteCacheTable(key, this.GetBytes(value))) == 1;
        }

        public bool Set<T>(string key, T value)
        {
            var n = this.InsertOrReplace(new SQliteCacheTable(key, this.GetBytes(value)));
            return n == 1;
        }

        public bool Replace<T>(string key, T value)
        {
            return this.Remove(key) ?
                this.Add(key, value) :
                false;
        }

        public virtual void FlushAll()
        {
            this.DeleteAll<SQliteCacheTable>();
        }

        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            var dict = new Dictionary<string, T>();

            foreach (var item in keys.Select(a => new { Key = a, Item = this.Get<T>(a) }).Where(a => a.Item != null))
            {
                dict.Add(item.Key, item.Item);
            }

            return dict;
        }

        public void SetAll<T>(IDictionary<string, T> values)
        {
            foreach (var value in values)
            {
                this.Set<T>(value.Key, value.Value);
            }
        }

        #endregion

        protected virtual T GetObject<T>(SQliteCacheTable item)
        {
            return (item != null) ? this.serializer.Deserialize<T>(item.Blob) : default(T);
        }

        protected virtual byte[] GetBytes<T>(T obj)
        {
            return this.serializer.Serialize(obj);
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
    }
}
