using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms.Labs.Mvvm;
using Xamarin.Forms.Labs.Services;
using XLabs.Caching;
using XLabs.Ioc;

namespace Xamarin.Forms.Labs.Sample
{
    /// <summary>
    /// Sample ViewModel for the CacheService.
    /// </summary>
    public class CacheServiceViewModel : Xamarin.Forms.Labs.Mvvm.ViewModel
    {
        /// <summary>
        /// Key used in the cache.
        /// </summary>
        private const string DemoKey = "test-key";

        /// <summary>
        /// Holds a reference to the CacheService.
        /// </summary>
        private readonly ISimpleCache cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheServiceViewModel"/> class.
        /// </summary>
        public CacheServiceViewModel()
        {
            cacheService = Resolver.Resolve<ISimpleCache>();
            CheckKeyAndDownloadNewContent();
        }

        /// <summary>
        /// Downloads items to the cache based on a key.
        /// </summary>
        private void CheckKeyAndDownloadNewContent()
        {
            if (cacheService == null)
            {
                throw new ArgumentNullException(
                    "_cacheService",
                    new Exception("Native SimpleCache implementation wasn't found."));
            }

            var keyValue = cacheService.Get<List<string>>(DemoKey);
            if (keyValue != null)
            {
                CacheInfo = "key found on cache";
                Items = new ObservableCollection<string>(keyValue);
            }
            else
            {
                CacheInfo = "key wasn't found on cache, you can save  it now";
                Items = new ObservableCollection<string> { "Bananas", "Oranges", "Apples" };
            }
        }

        /// <summary>
        /// Backing field for the CacheInfo property.
        /// </summary>
        private string cacheInfo;

        /// <summary>
        /// Gets or sets a <see cref="string"/> with cache information.
        /// </summary>
        /// <value>The cache information.</value>
        public string CacheInfo
        {
            get
            {
                return cacheInfo;
            }
            set
            {
                this.SetProperty(ref cacheInfo, value);
            }
        }

        /// <summary>
        /// Backing field for the Items property.
        /// </summary>
        private ObservableCollection<string> items;

        /// <summary>
        /// Gets or sets a collection of items in the cache.
        /// </summary>
        /// <value>The items in the cache.</value>
        public ObservableCollection<string> Items
        {
            get
            {
                return items;
            }
            set
            {
                this.SetProperty(ref items, value);
            }
        }

        /// <summary>
        /// Backing field to save items to the cache.
        /// </summary>
        private Command saveItemsToCacheCommand;

        /// <summary>
        /// Gets the command to save items to the cache.
        /// </summary>
        /// <value>A <see cref="Command"/> to save items to the cache.</value>
        public Command SaveItemsToCacheCacheCommand
        {
            get
            {
                return saveItemsToCacheCommand ?? (saveItemsToCacheCommand = new Command(
                    () =>
                    {
                        cacheService.Remove(DemoKey);
                        cacheService.Add(DemoKey, Items.ToList());
                        CacheInfo = "key was saved on cache";
                    },
                    () => true));
            }
        }

        /// <summary>
        /// Backing field to clear items from the cache.
        /// </summary>
        private Command clearCacheCommand;

        /// <summary>
        /// Gets the command to clear items from the cache.
        /// </summary>
        /// <value>A <see cref="Command"/> to clear items from the cache.</value>
        public Command ClearCacheCommand
        {
            get
            {
                return clearCacheCommand ?? (clearCacheCommand = new Command(
                     () => cacheService.FlushAll(),
                    () => true));
            }
        }
    }
}