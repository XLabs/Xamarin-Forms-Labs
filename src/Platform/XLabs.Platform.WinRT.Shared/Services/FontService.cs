// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 01-02-2016
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="FontService.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System.Collections.Generic;

namespace XLabs.Platform.Services
{
    /// <summary>
    /// A FontService implementation for Windows Platforms.
    /// </summary>
    public class FontService
    {
        /// <summary>
        /// Gets the font names.
        /// </summary>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public IEnumerable<string> GetFontNames()
        {
            var fontList = new List<string>();
#if NETFX_CORE || WINDOWS_PHONE_APP
            var factory = new SharpDX.DirectWrite.Factory();
            var fontCollection = factory.GetSystemFontCollection(false);
            var familyCount = fontCollection.FontFamilyCount;

            for (int i = 0; i < familyCount; i++)
            {
                var fontFamily = fontCollection.GetFontFamily(i);
                var familyNames = fontFamily.FamilyNames;
                int index;

                if (!familyNames.FindLocaleName(System.Globalization.CultureInfo.CurrentCulture.Name, out index))
                {
                    familyNames.FindLocaleName("en-us", out index);
                }

                string name = familyNames.GetString(index);
                fontList.Add(name);
            }
#endif

            return fontList;
        }
    }
}
