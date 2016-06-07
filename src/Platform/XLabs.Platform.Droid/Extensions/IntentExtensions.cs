// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : Tim Klingeleers
// Last Modified On : 03-27-2016
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
using Android.Support.V4.Content;

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
        public static void AddAttachments(this Intent intent, IEnumerable<string> attachments, IEnumerable<string> fileProviders)
        {
            if (attachments == null || !attachments.Any())
            {
                Android.Util.Log.Info("Intent.AddAttachments", "No attachments to attach.");
                return;
            } 

            if ((fileProviders == null || !fileProviders.Any()) &&
                Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
            {
                Android.Util.Log.Warn("Intent.AddAttachments", "Android M's permission model requires attachments to be added with a FileProvider. Add one or more providers to the constructor.");
            }

            IList<IParcelable> uris = new List<IParcelable>();

            foreach (var attachment in attachments)
            {
                var file = new Java.IO.File(attachment);

                if (!file.Exists())
                {
                    Android.Util.Log.Warn("Intent.AddAttachments", "Unable to attach file '{0}', because it doesn't exist.", attachment);
                    continue; // Go to the next file
                }

                Uri fileUri = null;

                if (fileProviders != null && fileProviders.Any())
                {
                    foreach (var fileProvider in fileProviders)
                    {
                        try
                        {
                            fileUri = FileProvider.GetUriForFile(Android.App.Application.Context, fileProvider, file);
                        }
                        catch (Java.Lang.IllegalArgumentException)
                        {
                            // FileProvider can't deal with the file, go to the next if there is one
                            continue;
                        }

                        if (fileUri != null)
                        {
                            break;
                        }
                    }
                }
                else
                { // There is no fileprovider added, try normal procedure
                    fileUri = Uri.FromFile(file);
                }

                if (fileUri != null)
                {
                    uris.Add(fileUri);
                }
                else
                {
                    //TODO: what to do if there are no providers that can deal with the file?
                    Android.Util.Log.Warn("Intent.AddAttachments", "Unable to attach file '{0}', because there is no FileProvider that can deal with the file.", attachment);
                }
            }

            intent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
        }
    }
}
