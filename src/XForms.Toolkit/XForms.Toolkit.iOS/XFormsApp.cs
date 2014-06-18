// ***********************************************************************
// Assembly         : XForms.Toolkit.iOS
// Author           : Shawn Anderson
// Created          : 06-17-2014
//
// Last Modified By : Sami Kallio
// Last Modified On : 06-18-2014
// ***********************************************************************
// <copyright file="XFormsAppiOS.cs" company="">
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
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XForms.Toolkit.Mvvm;

namespace XForms.Toolkit.iOS
{
	/// <summary>
	/// Class XFormsApplicationDelegate.
	/// </summary>
	public class XFormsApplicationDelegate : UIApplicationDelegate
	{
		/// <summary>
		/// Gets or sets the finished launching event.
		/// </summary>
		/// <value>The finished launching event.</value>
		public EventHandler<EventArgs> FinishedLaunchingEvent { get; set; }
		/// <summary>
		/// Gets or sets the will terminate event.
		/// </summary>
		/// <value>The will terminate event.</value>
		public EventHandler<EventArgs> WillTerminateEvent { get; set; }
		/// <summary>
		/// Gets or sets the on activated event.
		/// </summary>
		/// <value>The on activated event.</value>
		public EventHandler<EventArgs> OnActivatedEvent { get; set; }
		/// <summary>
		/// Gets or sets the did enter background event.
		/// </summary>
		/// <value>The did enter background event.</value>
		public EventHandler<EventArgs> DidEnterBackgroundEvent { get; set; }
		/// <summary>
		/// Gets or sets the will enter foreground event.
		/// </summary>
		/// <value>The will enter foreground event.</value>
		public EventHandler<EventArgs> WillEnterForegroundEvent { get; set; }

		/// <summary>
		/// Finished the launching.
		/// </summary>
		/// <param name="application">The application.</param>
        /// <remarks>Deprecated function</remarks>
		public override void FinishedLaunching(UIApplication application)
		{
			var handler = FinishedLaunchingEvent;
			if (handler != null) { handler(this, new EventArgs()); }
		}

		/// <summary>
		/// Finished the launching.
		/// </summary>
		/// <param name="app">The application.</param>
		/// <param name="options">The options.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			var handler = FinishedLaunchingEvent;
			if (handler != null) { handler(this, new EventArgs()); }

			return true;
		}	

		/// <summary>
		/// Wills the terminate.
		/// </summary>
		/// <param name="application">The application.</param>
		public override void WillTerminate(UIApplication application)
		{
			var handler = WillTerminateEvent;
			if (handler != null) { handler(this, new EventArgs()); }
		}
		/// <summary>
		/// Called when [activated].
		/// </summary>
		/// <param name="application">The application.</param>
		public override void OnActivated(UIApplication application)
		{
			var handler = OnActivatedEvent;
			if (handler != null) { handler(this, new EventArgs()); }
		}

		/// <summary>
		/// Dids the enter background.
		/// </summary>
		/// <param name="application">The application.</param>
		public override void DidEnterBackground(UIApplication application)
		{
			var handler = DidEnterBackgroundEvent;
			if (handler != null) { handler(this, new EventArgs()); }
		}

		/// <summary>
		/// Wills the enter foreground.
		/// </summary>
		/// <param name="application">The application.</param>
		public override void WillEnterForeground(UIApplication application)
		{
			var handler = WillEnterForegroundEvent;
			if (handler != null) { handler(this, new EventArgs()); }
		}
	}

	/// <summary>
	/// Class XFormsAppiOS.
	/// </summary>
	public class XFormsAppiOS : XFormsApp<XFormsApplicationDelegate>
	{
		/// <summary>
		/// Called when [initialize].
		/// </summary>
		/// <param name="app">The application.</param>
		protected override void OnInit(XFormsApplicationDelegate app)
		{
			this.AppContext.FinishedLaunchingEvent += (o, e) => { this.OnStartup(); };
			this.AppContext.WillTerminateEvent += (o, e) => { this.OnClosing(); };
			this.AppContext.DidEnterBackgroundEvent += (o, e) => { this.OnSuspended(); };
			this.AppContext.WillEnterForegroundEvent += (o, e) => { this.OnResumed(); };

			base.OnInit(app);
		}
	}
}