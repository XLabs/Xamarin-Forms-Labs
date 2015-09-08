using System.Collections.Generic;
using System.Text;

namespace XLabs.Serialization.AspNet
{
    public class MediaTypeJsonSerializer : MediaTypeSerializer
    {
        public MediaTypeJsonSerializer(IJsonSerializer jsonSerializer)
            : base(jsonSerializer, "application/json", new Encoding[] { new UTF8Encoding(false), new UnicodeEncoding(false, true) })
        {
            
        }

        public MediaTypeJsonSerializer(IJsonSerializer jsonSerializer, IEnumerable<Encoding> supportedEncodings)
            : base(jsonSerializer, "application/json", supportedEncodings)
        {

        }
    }
}
