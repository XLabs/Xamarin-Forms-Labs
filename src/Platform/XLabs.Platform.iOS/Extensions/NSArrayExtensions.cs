using Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace XLabs.Platform.Extensions
{
    public static class NSArrayExtensions
    {
        /// <summary>
        /// Transform an NSArray in .NET IEnumerable of native .NET type, the items inside the NSArray have to be the same type
        /// V is .NET target type, T is the iOS source type in the NSArray
        /// </summary>
        /// <typeparam name="T">The source iOS type inside the NSarray</typeparam>
        /// <typeparam name="V">The target .NET type inside the Enumerable</typeparam>
        /// <param name="nsa"></param>
        /// <returns></returns>
        public static IEnumerable<V> ToEnumerable<V,T>(this NSArray nsa) where T : NSObject
        {
            List<V> result = new List<V>();
            for (nuint i = 0; i < nsa.Count; i++)
                result.Add((V)nsa.GetItem<T>(i).ToObject(typeof(V)));
            return result;
        }
    }
}
