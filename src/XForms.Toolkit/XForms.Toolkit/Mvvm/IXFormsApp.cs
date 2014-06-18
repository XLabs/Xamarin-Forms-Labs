// ***********************************************************************
// Assembly         : XForms.Toolkit
// Author           : Shawn Anderson
// Created          : 06-17-2014
//
// Last Modified By : Shawn Anderson
// Last Modified On : 06-17-2014
// ***********************************************************************
// <copyright file="IXFormsApp.cs" company="">
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
	/// Interface IXFormsApp
	/// </summary>
	public interface IXFormsApp
	{
		#region Properties
		/// <summary>
		/// Gets a value indicating whether this instance is initialized.
		/// </summary>
		/// <value><c>true</c> if this instance is initialized; otherwise, <c>false</c>.</value>
		bool IsInitialized { get; }
		#endregion Properties

		#region Event Handlers
		/// <summary>
		/// Gets or sets the initialize.
		/// </summary>
		/// <value>The initialize.</value>
		EventHandler<EventArgs> Initialize { get; set; }
		/// <summary>
		/// Gets or sets the on startup.
		/// </summary>
		/// <value>The on startup.</value>
		EventHandler<EventArgs> Startup { get; set; }
		/// <summary>
		/// Gets or sets the on close.
		/// </summary>
		/// <value>The on close.</value>
		EventHandler<EventArgs> Closing { get; set; }
		/// <summary>
		/// Gets or sets the on suspend.
		/// </summary>
		/// <value>The on suspend.</value>
		EventHandler<EventArgs> Suspended { get; set; }
		/// <summary>
		/// Gets or sets the on resume.
		/// </summary>
		/// <value>The on resume.</value>
		EventHandler<EventArgs> Resumed { get; set; }
		/// <summary>
		/// Gets or sets the on error.
		/// </summary>
		/// <value>The on error.</value>
		EventHandler<EventArgs> Error { get; set; }
		/// <summary>
		/// Gets or sets the on rotation.
		/// </summary>
		/// <value>The on rotation.</value>
		EventHandler<EventArgs> Rotation { get; set; }
		#endregion Event Handlers

		#region Methods
		#endregion Methods
	}

	/// <summary>
	/// Interface IXFormsApp
	/// </summary>
	/// <typeparam name="TApp">The type of the t application.</typeparam>
	public interface IXFormsApp<TApp> : IXFormsApp
	{
		#region Properties
		/// <summary>
		/// Gets or sets the application context.
		/// </summary>
		/// <value>The application context.</value>
		TApp AppContext { get; set; }
		#endregion Properties

		#region Methods
		/// <summary>
		/// Initializes the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		void Init(TApp context);
		#endregion Methods
	}
}
