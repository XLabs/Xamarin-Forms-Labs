using Xamarin.Forms;

namespace XLabs.Forms.Extensions
{
	using System;
	using System.Collections.Generic;

	using Android.Content;
	using Android.Graphics;

	using Xamarin.Forms.Platform.Android;

	/// <summary>
	/// Interface of TypefaceCaches
	/// </summary>
	public interface ITypefaceCache
	{
		/// <summary>
		/// Removes typeface from cache
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="typeface">Typeface.</param>
		void StoreTypeface(string key, Typeface typeface);

		void RemoveTypeface(string key);

		Typeface RetrieveTypeface(string key);
	}

	/// <summary>
	/// TypefaceCache caches used typefaces for performance and memory reasons. 
	/// Typeface cache is singleton shared through execution of the application.
	/// You can replace default implementation of the cache by implementing ITypefaceCache 
	/// interface and setting instance of your cache to static property SharedCache of this class
	/// </summary>
	public static class TypefaceCache
	{
		private static ITypefaceCache sharedCache;

		/// <summary>
		/// Returns the shared typeface cache.
		/// </summary>
		/// <value>The shared cache.</value>
		public static ITypefaceCache SharedCache {
			get {
				if(sharedCache == null)
				{
					sharedCache = new DefaultTypefaceCache();
				}
				return sharedCache;
			}
			set {
				if(sharedCache != null && sharedCache.GetType() == typeof(DefaultTypefaceCache))
				{
					((DefaultTypefaceCache)sharedCache).PurgeCache();
				}
				sharedCache = value;
			}
		}




	}

	/// <summary>
	/// Default implementation of the typeface cache.
	/// </summary>
	internal class DefaultTypefaceCache : ITypefaceCache
	{
		private Dictionary<string,Typeface> _cacheDict;

		public DefaultTypefaceCache()
		{
			_cacheDict = new Dictionary<string, Typeface>();
		}


		public Typeface RetrieveTypeface(string key)
		{
			if(_cacheDict.ContainsKey(key))
			{
				return _cacheDict[key];
			} else
			{
				return null;
			}
		}

		public void StoreTypeface(string key, Typeface typeface)
		{
			_cacheDict[key] = typeface;
		}

		public void RemoveTypeface(string key)
		{
			_cacheDict.Remove(key);
		}

		public void PurgeCache()
		{
			_cacheDict = new Dictionary<string, Typeface>();
		}
	}



	/// <summary>
	/// Andorid specific extensions for Font class.
	/// </summary>
	public static class FontExtensions
	{

		/// <summary>
		/// This method returns typeface for given typeface using following rules:
		/// 1. Lookup in the cache
		/// 2. If not found, look in the assets in the fonts folder. Save your font under its FontFamily name. 
		/// If no extension is written in the family name .ttf is asumed
		/// 3. If not found look in the files under fonts/ folder
		/// If no extension is written in the family name .ttf is asumed
		/// 4. If not found, try to return typeface from Xamarin.Forms ToTypeface() method
		/// 5. If not successfull, return Typeface.Default
		/// </summary>
		/// <returns>The extended typeface.</returns>
		/// <param name="font">Font</param>
		/// <param name="context">Android Context</param>
		public static Typeface ToExtendedTypeface(this Font font, Context context)
		{
			Typeface typeface = null;

			//1. Lookup in the cache
			var hashKey = font.ToHasmapKey();
			typeface = TypefaceCache.SharedCache.RetrieveTypeface(hashKey);
			#if DEBUG
			if(typeface != null)
				Console.WriteLine("Typeface for font {0} found in cache", font);
			#endif

			//2. If not found, try custom asset folder
			if(typeface == null && !string.IsNullOrEmpty(font.FontFamily))
			{
				string filename = font.FontFamily;
				//if no extension given then assume and add .ttf
				if(filename.LastIndexOf(".", System.StringComparison.Ordinal) != filename.Length - 4)
				{
					filename = string.Format("{0}.ttf", filename);
				}
				try
				{
					var path = "fonts/" + filename;
					#if DEBUG
					Console.WriteLine("Lookking for font file: {0}", path);
					#endif
					typeface = Typeface.CreateFromAsset(context.Assets, path);
					#if DEBUG
					Console.WriteLine("Found in assets and cached.");
					#endif
				} catch(Exception ex)
				{
					#if DEBUG
					Console.WriteLine("not found in assets. Exception: {0}", ex);
					Console.WriteLine("Trying creation from file");
					#endif
					try
					{
						typeface = Typeface.CreateFromFile("fonts/" + filename);
						#if DEBUG
						Console.WriteLine("Found in file and cached.");
						#endif
					} catch(Exception ex1)
					{
						#if DEBUG
						Console.WriteLine("not found by file. Exception: {0}", ex1);
						Console.WriteLine("Trying creation using Xamarin.Forms implementation");
						#endif

					}
				}

			}
			//3. If not found, fall back to default Xamarin.Forms implementation to load system font
			if(typeface == null)
			{
				typeface = font.ToTypeface();
			}

			if(typeface == null)
			{
				#if DEBUG
				Console.WriteLine("Falling back to default typeface");
				#endif
				typeface = Typeface.Default;
			}
			//Store in cache
			TypefaceCache.SharedCache.StoreTypeface(hashKey, typeface);

			return typeface;

		}

		/// <summary>
		/// Provides unique identifier for the given font.
		/// </summary>
		/// <returns>Unique string identifier for the given font</returns>
		/// <param name="font">Font.</param>
		private static string ToHasmapKey(this Font font)
		{
			return string.Format("{0}.{1}.{2}.{3}", font.FontFamily, font.FontSize, font.NamedSize, (int)font.FontAttributes);
		}
	}
}

