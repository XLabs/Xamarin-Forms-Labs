// ***********************************************************************
// Assembly         : XLabs.Forms.WinUniversal
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="XFormsApp.cs" company="XLabs Team">
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

using System.Diagnostics.CodeAnalysis;
using Windows.Storage;
using Xamarin.Forms;
using XLabs.Platform.Mvvm;

namespace XLabs.Forms
{

    /// <summary>
    ///     The XLabs Windows Universal Application.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "Reviewed. Suppression is OK here.")]
    public class XFormsAppWin : XFormsApp<Windows.UI.Xaml.Application>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="XFormsAppWin" /> class.
        /// </summary>
        public XFormsAppWin()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XFormsAppWin" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        public XFormsAppWin(Windows.UI.Xaml.Application application)
            : base(application)
        {			
        }

        /// <summary>
        ///     Raises the back press.
        /// </summary>
        public void RaiseBackPress()
        {
            OnBackPress();
        }

        /// <summary>
        /// Raises the startup event.
        /// </summary>
        public void RaiseStartup()
        {
            OnStartup();
        }

        /// <summary>
        /// Raises the closing event.
        /// </summary>
        public void RaiseClosing()
        {
            OnClosing();
        }

        //// <summary>
        ////     Sets the orientation.
        //// </summary>
        //// <param name="orientation">The orientation.</param>
        //public void SetOrientation(PageOrientation orientation)
        //{
        //    Orientation = orientation.ToOrientation();
        //}

        /// <summary>
        ///     Initializes the specified context.
        /// </summary>
        /// <param name="app">The native application.</param>
        /// <param name="initServices">Should initialize services.</param>
        protected override void OnInit(Windows.UI.Xaml.Application app,bool initServices = true)
        {
            
            //AppContext.Startup += (o, e) => OnStartup();
            //AppContext.Exit += (o, e) => OnClosing();
            app.UnhandledException += (o, e) => OnError(e.Exception);
            AppDataDirectory = ApplicationData.Current.LocalFolder.Path;
            
            app.Resuming += (o, e) => OnResumed();
            app.Suspending += (o, e) => OnSuspended();			

            if (initServices) 
            {
                if (initServices)
                {
                    DependencyService.Register<XLabs.Platform.Services.TextToSpeechService>();
                    DependencyService.Register<XLabs.Platform.Services.Geolocation.Geolocator>();
                    //DependencyService.Register<MediaPicker>();
                    DependencyService.Register<XLabs.Platform.Services.Media.SoundService>();
                    //DependencyService.Register<XLabs.Platform.Services.Email.EmailService>();
                    //DependencyService.Register<FileManager>();
                    //DependencyService.Register<WindowsPhoneDevice>();
                }
            }

            base.OnInit(app);
        }
    }
}