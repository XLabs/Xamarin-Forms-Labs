using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLabs.Platform.Services.Media
{
    public class MediaFormatException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFormatException" /> class.
		/// </summary>
		/// <param name="path">The path.</param>
		public MediaFormatException(string format)
			: base("The file is not a valid "+format)
		{
			
		}

		/// <summary>
        /// Initializes a new instance of the <see cref="MediaFormatException" /> class.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="innerException">The inner exception.</param>
        public MediaFormatException(string format,Exception innerException)
            : base("The file  is not a valid " + format, innerException)
		{
			
		}

		
    }
}
