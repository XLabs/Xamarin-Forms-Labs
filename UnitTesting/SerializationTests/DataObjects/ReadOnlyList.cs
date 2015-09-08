namespace SerializationTests.DataObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    public class ReadOnlyList<T> : IEquatable<ReadOnlyList<T>>
    {
        [DataMember(Order = 1)]
        public IReadOnlyCollection<T> Collection { get; set; }

        #region IEquatable<ReadOnlyList<T>> Members

        public bool Equals(ReadOnlyList<T> other)
        {
            return other != null && this.Collection.SequenceEqual(other.Collection);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param>
        public override bool Equals(object obj)
        {
            return Equals(obj as ReadOnlyList<T>);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Collection.GetHashCode();
        }

        #endregion
    }
}
