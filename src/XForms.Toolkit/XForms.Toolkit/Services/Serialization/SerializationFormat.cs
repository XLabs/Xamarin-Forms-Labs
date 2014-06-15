using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Toolkit.Services.Serialization
{
    /// <summary>
    /// Serialization format type
    /// </summary>
    public enum SerializationFormat
    {
        /// <summary>
        /// Custom undefined format
        /// </summary>
        Custom,

        /// <summary>
        /// JSON format
        /// </summary>
        Json,

        /// <summary>
        /// XML format
        /// </summary>
        Xml,

        /// <summary>
        /// ProtoBuffer format
        /// </summary>
        ProtoBuffer,

        /// <summary>
        /// Custom binary
        /// </summary>
        Binary
    }
}
