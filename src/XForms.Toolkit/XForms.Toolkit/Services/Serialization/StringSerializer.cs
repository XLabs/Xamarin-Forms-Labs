using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Toolkit.Services.Serialization
{
    public abstract class StringSerializer : ISerializer
    {
        #region ISerializer Members
        public abstract SerializationFormat Format { get; }

        public abstract void Flush();
        #endregion

        #region IByteSerializer Members

        byte[] IByteSerializer.Serialize<T>(T obj)
        {
            return (this as IStringSerializer).SerializeToBytes(obj);
        }

        public T Deserialize<T>(byte[] data)
        {
            return (this as IStringSerializer).DeserializeFromBytes<T>(data);
        }
        #endregion

        #region IStreamSerializer Members
        public void Serialize<T>(T obj, System.IO.Stream stream)
        {
            (this as IStringSerializer).SerializeToStream(obj, stream);
        }

        public T Deserialize<T>(System.IO.Stream stream)
        {
            return (this as IStringSerializer).DeserializeFromStream<T>(stream);
        }
        #endregion

        #region IStringSerializer Members
        public abstract string Serialize<T>(T obj);

        public abstract T Deserialize<T>(string data);
        #endregion
    }
}
