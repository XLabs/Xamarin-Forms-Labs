namespace XLabs.Forms.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using Platform.Device;
    using Platform.WinRT;
    using Xamarin.Forms;
    using Application = System.Windows.Application;

    /// <summary>
    /// Class FontManager.
    /// </summary>
    public partial class FontManager : IFontManager
    {
        /// <summary>
        /// The _display
        /// </summary>
        private readonly IDisplay display;

        private readonly FontService fontService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontManager"/> class.
        /// </summary>
        /// <param name="display">The display.</param>
        public FontManager(IDisplay display)
        {
            this.display = display;
            this.fontService = new FontService();
        }

        #region IFontManager Members

        /// <summary>
        /// Gets all available system fonts.
        /// </summary>
        /// <value>The available fonts.</value>
        public IEnumerable<string> AvailableFonts
        {
            get
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                // returning WinRT method call directly doesn't work so do NOT refactor!!!
                foreach (var fontName in this.fontService.GetFontNames())
                {
                    yield return fontName;
                }
            }
        }

        /// <summary>
        /// Gets height for the font.
        /// </summary>
        /// <param name="font">Font whose height is calculated.</param>
        /// <returns>Height of the font in inches.</returns>
        public double GetHeight(Font font)
        {
            var multiplier = Application.Current.Host.Content.ScaleFactor / 100d;
            return multiplier * font.GetHeight() / this.display.Ydpi;
        }

        #endregion


    }
}
