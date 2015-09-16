using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLabs.Platform.Services.Media
{
    /// <summary>
    /// Exception raised if an error occure in exif processing
    /// </summary>
    public class MediaExifException : Exception
    {
        public MediaExifException()
        {
        }

        public MediaExifException(string message)
            : base(message)
        {
        }

        public MediaExifException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
