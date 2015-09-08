// ***********************************************************************
// Assembly         : XLabs.Sample.Droid
// Author           : Shawn Anderson
// Created          : 06-16-2014
//
// Last Modified By : Sami Kallio
// Last Modified On : 27-08-2015
// ***********************************************************************
// <copyright file="MainActivity.cs" company="">
//     Copyright (c) 2014 . All rights reserved.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace XLabs.Sample.Droid
{
    using System.IO;
    using Android.App;
    using Android.Content.PM;
    using Android.OS;
    using Caching;
    using Caching.SQLite;
    using Forms;
    using Forms.Services;
    using Ioc;
    using Platform.Device;
    using Platform.Mvvm;
    using Platform.Services;
    using Platform.Services.Email;
    using Platform.Services.Media;
    using Serialization;
    using Serialization.ServiceStack;
    using SQLite.Net;
    using SQLite.Net.Platform.XamarinAndroid;
    using Xamarin.Forms;

    /// <summary>
    /// Class MainActivity.
    /// </summary>
    [Activity(Label = "XLabs.Sample.Droid", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MainActivity : XFormsApplicationDroid
    {
        /// <summary>
        /// Called when [create].
        /// </summary>
        /// <param name="bundle">The bundle.</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat) 
            {
                Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true); 
            }

            XLabs.Forms.Controls.HybridWebViewRenderer.GetWebViewClientDelegate = r => new CustomClient(r);
            XLabs.Forms.Controls.HybridWebViewRenderer.GetWebChromeClientDelegate = r => new CustomChromeClient();

            if (!Resolver.IsSet)
            {
                this.SetIoc();
            }
            else
            {
                var app = Resolver.Resolve<IXFormsApp>() as IXFormsApp<XFormsApplicationDroid>;
                if (app != null) app.AppContext = this;
            }

            Forms.Init(this, bundle);

            Forms.ViewInitialized += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.View.StyleId))
                {
                    e.NativeView.ContentDescription = e.View.StyleId;
                }
            };

            this.LoadApplication(new App());
        }

        /// <summary>
        /// Sets the IoC.
        /// </summary>
        private void SetIoc()
        {
            var resolverContainer = new SimpleContainer();

            var app = new XFormsAppDroid();

            app.Init(this);

            var documents = app.AppDataDirectory;
            var pathToDatabase = Path.Combine(documents, "xforms.db");

            resolverContainer.Register<IDevice>(t => AndroidDevice.CurrentDevice)
                .Register<IDisplay>(t => t.Resolve<IDevice>().Display)
                .Register<IFontManager>(t => new FontManager(t.Resolve<IDisplay>()))
                //.Register<IJsonSerializer, Services.Serialization.JsonNET.JsonSerializer>()
                .Register<IJsonSerializer, JsonSerializer>()
                .Register<IEmailService, EmailService>()
                .Register<IMediaPicker, MediaPicker>()
                .Register<ITextToSpeechService, TextToSpeechService>()
                .Register<IDependencyContainer>(resolverContainer)
                .Register<IXFormsApp>(app)
                .Register<ISecureStorage>(t => new KeyVaultStorage(t.Resolve<IDevice>().Id.ToCharArray()))
                .Register<ISimpleCache>(
                    t => new SQLiteSimpleCache(new SQLitePlatformAndroid(),
                        new SQLiteConnectionString(pathToDatabase, true), t.Resolve<IJsonSerializer>()));


            Resolver.SetResolver(resolverContainer.GetResolver());
        }
    }
}


