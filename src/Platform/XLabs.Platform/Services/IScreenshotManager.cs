using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLabs.Platform.Services
{
    /// <summary>
    /// Manager to handle screenshots
    /// </summary>
    public interface IScreenshotManager
    {
        /// <summary>
        /// Capture the current screen
        /// </summary>
        /// <returns>The screenshot with metadata</returns>
        Task<Screenshot> CaptureAsync();
    }

    /// <summary>
    /// A screenshot and info about the screenshot
    /// </summary>
    public class Screenshot
    {
        public byte[] Filebytes { get; set; }
        public string ContentType { get; set; }
    }
}
