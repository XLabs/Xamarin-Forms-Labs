// ***********************************************************************
// Assembly         : XForms.Toolkit.Sample.Droid
// Author           : Shawn Anderson
// Created          : 06-16-2014
//
// Last Modified By : Shawn Anderson
// Last Modified On : 06-16-2014
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
using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using XForms.Toolkit.Droid;
using XForms.Toolkit.Mvvm;
using XForms.Toolkit.Services;
using XForms.Toolkit.Services.Serialization;


namespace XForms.Toolkit.Sample.Droid
{
	/// <summary>
	/// Class MainActivity.
	/// </summary>
	[Activity (Label = "XForms.Toolkit.Sample.Droid", MainLauncher = true)]
	public class MainActivity : XFormsApplicationDroid
	{
		/// <summary>
		/// Indicated if the application has been initialized
		/// </summary>
        private static bool _initialized;

		/// <summary>
		/// Called when [create].
		/// </summary>
		/// <param name="bundle">The bundle.</param>
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate(bundle);

            if (!_initialized)
            {
                SetIoc();
            }

			Xamarin.Forms.Forms.Init(this, bundle);

			App.Init();

			SetPage(App.GetMainPage());
		}

		/// <summary>
		/// Sets the ioc.
		/// </summary>
        private void SetIoc()
        {
            var resolverContainer = new SimpleContainer();

			var app = new XFormsAppDriod();

			app.Init(this);

			resolverContainer.Register<IDevice>(t => AndroidDevice.CurrentDevice)
				.Register<IJsonSerializer, Services.Serialization.ServiceStackV3.JsonSerializer>()
				.Register<IDependencyContainer>(resolverContainer)
				.Register<IXFormsApp>(app);

            Resolver.SetResolver(resolverContainer.GetResolver());

            _initialized = true;
        }
	}
}


