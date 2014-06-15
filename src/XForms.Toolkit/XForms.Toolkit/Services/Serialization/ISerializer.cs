using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Toolkit.Services.Serialization
{
    /// <summary>
    /// Common serializer interface
    /// </summary>
    public interface ISerializer : IByteSerializer, IStreamSerializer, IStringSerializer
    {
        /// <summary>
        /// Gets the serialization format
        /// </summary>
        SerializationFormat Format { get; }

        /// <summary>
        /// Cleans memory
        /// </summary>
        void Flush();
    }
}
