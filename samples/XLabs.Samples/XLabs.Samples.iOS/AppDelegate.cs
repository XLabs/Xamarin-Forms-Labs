// ***********************************************************************
// Assembly         : XLabs.Sample.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
//
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="AppDelegate.cs" company="XLabs Team">
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

using Foundation;
using SQLite.Net;
using SQLite.Net.Platform.XamarinIOS;
using System.IO;
using UIKit;
using XLabs.Caching.SQLite;
using XLabs.Forms;
using XLabs.Forms.Controls;
using XLabs.Forms.Services;
using XLabs.Platform.Device;
using XLabs.Platform.Mvvm;
using XLabs.Platform.Services;
using XLabs.Platform.Services.Email;
using XLabs.Platform.Services.Media;

namespace XLabs.Samples.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : XFormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            this.SetIoc();

            HybridWebViewRenderer.CopyBundleDirectory("HTML");

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new XLabs.Samples.App());

            Xamarin.Forms.Forms.ViewInitialized += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.View.StyleId))
                {
                    e.NativeView.AccessibilityIdentifier = e.View.StyleId;
                }
            };

            return base.FinishedLaunching(app, options);
        }

        /// <summary>
        /// Sets the IoC.
        /// </summary>
        private void SetIoc()
        {
            var resolverContainer = new global::XLabs.Ioc.SimpleContainer();

            var app = new XFormsAppiOS();
            app.Init(this);

            var documents = app.AppDataDirectory;
            var pathToDatabase = Path.Combine(documents, "xforms.db");

            resolverContainer.Register<IDevice>(t => AppleDevice.CurrentDevice)
                .Register<IDisplay>(t => t.Resolve<IDevice>().Display)
                .Register<IFontManager>(t => new FontManager(t.Resolve<IDisplay>()))
                .Register<XLabs.Serialization.IJsonSerializer, XLabs.Serialization.JsonNET.JsonSerializer>()
                //.Register<IJsonSerializer, Services.Serialization.SystemJsonSerializer>()
                .Register<ITextToSpeechService, TextToSpeechService>()
                .Register<IEmailService, EmailService>()
                .Register<IMediaPicker, MediaPicker>()
                .Register<IXFormsApp>(app)
                .Register<ISecureStorage, SecureStorage>()
                .Register<global::XLabs.Ioc.IDependencyContainer>(t => resolverContainer)
                .Register<global::XLabs.Caching.ICacheProvider>(
                    t => new SQLiteSimpleCache(new SQLitePlatformIOS(),
                        new SQLiteConnectionString(pathToDatabase, true), t.Resolve<global::XLabs.Serialization.IJsonSerializer>()));

            XLabs.Ioc.Resolver.SetResolver(resolverContainer.GetResolver());
        }
    }
}
