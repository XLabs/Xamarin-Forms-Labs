using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Toolkit.Services.Serialization
{
    public abstract class StreamSerializer : ISerializer
    {
        #region ISerializer Members
        public abstract SerializationFormat Format { get; }

        public abstract void Flush();
        #endregion

        #region IByteSerializer Members

        byte[] IByteSerializer.Serialize<T>(T obj)
        {
            return (this as IStreamSerializer).SerializeToBytes(obj);
        }

        public T Deserialize<T>(byte[] data)
        {
            return (this as IStreamSerializer).DeserializeFromBytes<T>(data);
        }
        #endregion

        #region IStreamSerializer Members
        public abstract void Serialize<T>(T obj, System.IO.Stream stream);

        public abstract T Deserialize<T>(System.IO.Stream stream);
        #endregion

        #region IStringSerializer Members
        string IStringSerializer.Serialize<T>(T obj)
        {
            return (this as IStreamSerializer).SerializeToString(obj);
        }

        public T Deserialize<T>(string data)
        {
            return (this as IStreamSerializer).DeserializeFromString<T>(data);
        }
        #endregion
    }
}
