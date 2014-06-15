using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Toolkit.Services.Serialization
{
    /// <summary>
    /// Defines a generic interface for stream serializer
    /// </summary>
    public interface IStreamSerializer
    {
        /// <summary>
        /// Serializes object to a stream
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object to serialize</param>
        /// <param name="stream">Stream to serialize to</param>
        void Serialize<T>(T obj, Stream stream);

        /// <summary>
        /// Deserializes stream into an object
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to</typeparam>
        /// <param name="stream">Stream to deserialize from</param>
        /// <returns>Object of type T</returns>
        T Deserialize<T>(Stream stream);
    }
}
