using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Toolkit.Services.Serialization
{
    /// <summary>
    /// Common interface for byte buffer serializer
    /// </summary>
    public interface IByteSerializer
    {
        /// <summary>
        /// Serializes object to a string
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to</typeparam>
        /// <param name="obj">Object to serialize</param>
        /// <returns>Serialized byte[] of the object</returns>
        byte[] Serialize<T>(T obj);

        /// <summary>
        /// Deserializes string into an object
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to</typeparam>
        /// <param name="data">Serialized object as byte buffer</param>
        /// <returns>Object of type T</returns>
        T Deserialize<T>(byte[] data);
    }
}
