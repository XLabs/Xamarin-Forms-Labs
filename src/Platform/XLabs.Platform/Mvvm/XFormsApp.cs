// ***********************************************************************
// Assembly         : XLabs.Platform
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

using System;
using System.Threading.Tasks;
using XLabs.Enums;

namespace XLabs.Platform.Mvvm
{
	/// <summary>
	/// Class XFormsApp.
	/// </summary>
	/// <typeparam name="TApp">The type of the application.</typeparam>
	public class XFormsApp<TApp> : IXFormsApp<TApp>
	{
		/// <summary>
		/// The _orientation
		/// </summary>
		private Orientation orientation;

		/// <summary>
		/// Initializes a new instance of the <see cref="XFormsApp{TApp}"/> class.
		/// </summary>
		public XFormsApp() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="XFormsApp{TApp}"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public XFormsApp(TApp context)
		{
			Init(context);
		}

		#region Properties

		/// <summary>
		/// Gets a value indicating whether this instance is initialized.
		/// </summary>
		/// <value><c>true</c> if this instance is initialized; otherwise, <c>false</c>.</value>
		public bool IsInitialized { get; private set; }

		/// <summary>
		/// Gets or sets the application data directory.
		/// </summary>
		/// <value>The application data directory</value>
		public string AppDataDirectory { get; set; }

		/// <summary>
		/// Gets or sets the application context.
		/// </summary>
		/// <value>The application context.</value>
		public TApp AppContext { get; set; }

		/// <summary>
		/// Gets or sets the orientation.
		/// </summary>
		/// <value>The orientation.</value>
		public Orientation Orientation 
		{ 
			get
			{
				return this.orientation;
			}

			protected set
			{
				this.orientation = value;
				Rotation.Invoke(this, this.orientation);
			}
		}

		/// <summary>
		/// Gets or sets the back press delegate.
		/// </summary>
		/// <value>The back press delegate.</value>
		public Func<Task<bool>> BackPressDelegate
		{
			get;
			set;
		}

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
		public EventHandler<EventArgs<Orientation>> Rotation { get; set; }

		/// <summary>
		/// Gets or sets the back press.
		/// </summary>
		/// <value>The back press.</value>
		public EventHandler<EventArgs> BackPress { get; set; } 

		#endregion Event Handlers

		#region Methods

		/// <summary>
		/// Initializes the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="initServices">Should initialize services.</param>
		public void Init(TApp context,bool initServices = true)
		{
			AppContext = context;

			OnInit(context,initServices);

			IsInitialized = true;
		}

		#endregion Methods

		#region Virtual Methods

		/// <summary>
		/// Initializes the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="initServices">Should initialize services.</param>
		protected virtual void OnInit(TApp context, bool initServices = true)
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

		/// <summary>
		/// Called when [error].
		/// </summary>
		protected virtual void OnBackPress()
		{
			RaiseOnBackPress();
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

		/// <summary>
		/// Raises the on back press.
		/// </summary>
		protected virtual void RaiseOnBackPress()
		{
			var handler = BackPress;

			if (handler != null)
			{
				handler(this, new EventArgs());
			}
		}

		#endregion Event Methods
	}
}