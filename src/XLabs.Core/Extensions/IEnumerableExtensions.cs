namespace XLabs
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Converts <see cref="IEnumerable{T}"/> to a <see cref="IReadOnlyCollection{T}"/>
        /// </summary>
        /// <param name="enumerable">Enumerable object.</param>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <returns>IReadOnlyCollection{T}</returns>
        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ReadOnlyCollection<T>((enumerable as IList<T>) ?? enumerable.ToList());
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
                yield return item;
            }
        }
    }
}