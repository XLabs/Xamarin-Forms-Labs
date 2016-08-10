// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IntentExtensions.cs" company="XLabs Team">
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
using System.Linq;
using Android.Content;
using Android.Net;
using Android.OS;

namespace XLabs.Platform
{
    /// <summary>
    /// Class IntentExtensions.
    /// </summary>
    public static class IntentExtensions
  {
        /// <summary>
        /// Adds the attachments.
        /// </summary>
        /// <param name="intent">The intent.</param>
        /// <param name="attachments">The attachments.</param>
    public static void AddAttachments(this Intent intent, IEnumerable<string> attachments)
    {
      if (attachments == null || !attachments.Any())
      {
        Android.Util.Log.Warn("Intent.AddAttachments", "No attachments to attach.");
        return;
      }

      IList<IParcelable> uris = new List<IParcelable>();
      foreach (var attachment in attachments)
      {
        //convert from paths to Android friendly Parcelable Uri's
        var file = new Java.IO.File(attachment);
        if (file.Exists()) 
        {
          Uri u = Uri.FromFile(file);
          uris.Add(u);
        } else {
            Android.Util.Log.Warn("Intent.AddAttachments", "Unable to attach file '{0}', because it doesn't exist.", attachment);
        }
      }

      intent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
    }
  }
}
