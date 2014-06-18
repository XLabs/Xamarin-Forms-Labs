// ***********************************************************************
// Assembly         : XForms.Toolkit
// Author           : Shawn Anderson
// Created          : 06-17-2014
//
// Last Modified By : Shawn Anderson
// Last Modified On : 06-17-2014
// ***********************************************************************
// <copyright file="XFormsApp.cs" company="">
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

namespace XForms.Toolkit.Mvvm
{
	/// <summary>
	/// Class XFormsApp.
	/// </summary>
	/// <typeparam name="TApp">The type of the t application.</typeparam>
	public class XFormsApp<TApp> : IXFormsApp<TApp>
	{
		#region Properties
		/// <summary>
		/// Gets a value indicating whether this instance is initialized.
		/// </summary>
		/// <value><c>true</c> if this instance is initialized; otherwise, <c>false</c>.</value>
		public bool IsInitialized { get; private set; }

		/// <summary>
		/// Gets or sets the application context.
		/// </summary>
		/// <value>The application context.</value>
		public TApp AppContext { get; set; }
		#endregion Properties

		#region Event Handlers
		/// <summary>
		/// Gets or sets the initialize.
		/// </summary>
		/// <value>The initialize.</value>
		public EventHandler<EventArgs> Initialize { get; set; }
		/// <summary>
		/// Gets or sets the on startup.
		/// </summary>
		/// <value>The on startup.</value>
		public EventHandler<EventArgs> Startup { get; set; }
		/// <summary>
		/// Gets or sets the on close.
		/// </summary>
		/// <value>The on close.</value>
		public EventHandler<EventArgs> Closing { get; set; }
		/// <summary>
		/// Gets or sets the on suspend.
		/// </summary>
		/// <value>The on suspend.</value>
		public EventHandler<EventArgs> Suspended { get; set; }
		/// <summary>
		/// Gets or sets the on resume.
		/// </summary>
		/// <value>The on resume.</value>
		public EventHandler<EventArgs> Resumed { get; set; }
		/// <summary>
		/// Gets or sets the on error.
		/// </summary>
		/// <value>The on error.</value>
		public EventHandler<EventArgs> Error { get; set; }
		/// <summary>
		/// Gets or sets the on rotation.
		/// </summary>
		/// <value>The on rotation.</value>
		public EventHandler<EventArgs> Rotation { get; set; }
		#endregion Event Handlers

		#region Methods
		/// <summary>
		/// Initializes the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Init(TApp context)
		{
			AppContext = context;

			OnInit(context);

			IsInitialized = true;
		}
		#endregion Methods

		#region Virtual Methods
		/// <summary>
		/// Initializes the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		protected virtual void OnInit(TApp context)
		{
			RaiseOnInitialize();
		}

		/// <summary>
		/// Called when [startup].
		/// </summary>
		protected virtual void OnStartup()
		{
			RaiseOnStartUp();
		}

		/// <summary>
		/// Called when [closing].
		/// </summary>
		protected virtual void OnClosing()
		{
			RaiseOnClosing();
		}

		/// <summary>
		/// Called when [suspended].
		/// </summary>
		protected virtual void OnSuspended()
		{
			RaiseOnSuspeded();
		}

		/// <summary>
		/// Called when [resumed].
		/// </summary>
		protected virtual void OnResumed()
		{
			RaiseOnResumed();
		}

		/// <summary>
		/// Called when [error].
		/// </summary>
		/// <param name="ex">The exception.</param>
		protected virtual void OnError(Exception ex)
		{
			RaiseOnError(ex);
		}
		#endregion Virtual Mehods

		#region Event Methods
		/// <summary>
		/// Called when [initialize].
		/// </summary>
		protected virtual void RaiseOnInitialize()
		{
			var handler = Initialize;

			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}

		/// <summary>
		/// Called when [start up].
		/// </summary>
		protected virtual void RaiseOnStartUp()
		{
			var handler = Startup;

			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}

		/// <summary>
		/// Called when [closing].
		/// </summary>
		protected virtual void RaiseOnClosing()
		{
			var handler = Closing;

			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}

		/// <summary>
		/// Called when [suspeded].
		/// </summary>
		protected virtual void RaiseOnSuspeded()
		{
			var handler = Suspended;

			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}

		/// <summary>
		/// Called when [resumed].
		/// </summary>
		protected virtual void RaiseOnResumed()
		{
			var handler = Resumed;

			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}

		/// <summary>
		/// Called when [error].
		/// </summary>
		/// <param name="e">The exception.</param>
		protected virtual void RaiseOnError(Exception e)
		{
			var handler = Error;

			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}
		#endregion Event Methods
	}
}
