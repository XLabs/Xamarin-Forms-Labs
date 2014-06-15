using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace XForms.Toolkit.Services.Serialization.ProtoBuf
{
    public class ProtoBufSerializer : StreamSerializer
    {
        public override SerializationFormat Format
        {
            get { return SerializationFormat.ProtoBuffer; }
        }

        public override void Flush()
        {
            Serializer.FlushPool();
        }

        public override void Serialize<T>(T obj, System.IO.Stream stream)
        {
            Serializer.Serialize(stream, obj);
        }

        public override T Deserialize<T>(System.IO.Stream stream)
        {
            return Serializer.Deserialize<T>(stream);
        }
    }
}
