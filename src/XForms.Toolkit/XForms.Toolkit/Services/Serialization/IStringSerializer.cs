using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Toolkit.Services.Serialization
{
    /// <summary>
    /// Serialization interface for strings
    /// </summary>
    public interface IStringSerializer
    {
        /// <summary>
        /// Serializes object to a string
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object to serialize</param>
        /// <returns>Serialized string of the object</returns>
        string Serialize<T>(T obj);

        /// <summary>
        /// Deserializes string into an object
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to</typeparam>
        /// <param name="data">Serialized object</param>
        /// <returns>Object of type T</returns>
        T Deserialize<T>(string data);
    }
}
