using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace XLabs.Forms.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Platform.Device;
    using Utilities;

    /// <summary>
    /// Class FontManager.
    /// </summary>
    public partial class FontManager : IFontManager
    {
        #region static FontManager
        static FontManager()
        {
            FontDirectories = new List<string>(new[] { "/system/fonts", "/system/font", "/data/fonts" });
        }

        /// <summary>
        /// Directories that are searched for font files.
        /// </summary>
        /// <remarks>Add or remove directories before accessing method <see cref="IFontManager.AvailableFonts"/>.</remarks>
        public static ICollection<string> FontDirectories { get; private set; } 
        #endregion

        /// <summary>
        /// The _display
        /// </summary>
        private readonly IDisplay display;

        private List<string> availableFonts;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontManager"/> class.
        /// </summary>
        /// <param name="display">The display.</param>
        public FontManager(IDisplay display)
        {
            this.display = display;
        }

        #region IFontManager Members

        /// <summary>
        /// Gets all available system fonts.
        /// </summary>
        /// <value>The available fonts.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<string> AvailableFonts
        {
            get
            {
                return this.availableFonts ??
                    (this.availableFonts = FontDirectories.Select(a => new DirectoryInfo(a))
                                              .Where(a => a.Exists)
                                              .SelectMany(a => a.GetFiles())
                                              .Where(f => f.Exists)
                                              .Select(f => TtfFileInfo.FromStream(f.OpenRead()))
                                              .Where(font => font != null)
                                              .Select(font => font.FontName)
                                              .ToList());
            }
        }

        /// <summary>
        /// Gets height for the font.
        /// </summary>
        /// <param name="font">Font whose height is calculated.</param>
        /// <returns>Height of the font in inches.</returns>
        public double GetHeight(Font font)
        {
            var scaled = font.ToScaledPixel();
            return scaled *  Display.Metrics.Density / this.display.Ydpi;
        }

        #endregion
    }
}
