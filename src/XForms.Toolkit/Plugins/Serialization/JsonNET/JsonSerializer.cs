using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XForms.Toolkit.Services.Serialization.JsonNET
{
    /// <summary>
    /// JSON serializer using Newtonsoft.Json library
    /// </summary>
    /// <remarks>
    /// 
    /// Newtonsoft.Json copyright information
    /// 
    /// The MIT License (MIT)
    /// Copyright (c) 2007 James Newton-King
    /// 
    /// https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md
    /// 
    /// </remarks>
    public class JsonSerializer : StringSerializer, IJsonSerializer
    {
        public override SerializationFormat Format
        {
            get { return SerializationFormat.Json; }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Serializes object to a string
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>Serialized string of the object</returns>
        public override string Serialize<T>(T obj)
        {
            JsonConvert.DefaultSettings().ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Deserializes string into an object
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to</typeparam>
        /// <param name="data">Serialized object</param>
        /// <returns>Object of type T</returns>
        public override T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
