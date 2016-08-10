// ***********************************************************************
// <copyright file="Env.cs" company="XLabs">
//     Copyright © ServiceStack 2013 & XLabs
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace ServiceStack.Text
{
	/// <summary>
	/// Class Env.
	/// </summary>
	public static class Env
	{
		/// <summary>
		/// Initializes static members of the <see cref="Env"/> class.
		/// </summary>
		static Env()
		{
		    string platformName = null;

#if NETFX_CORE
            IsWinRT = true;
            platformName = "WinRT";
#else
            var platform = (int)Environment.OSVersion.Platform;
			IsUnix = (platform == 4) || (platform == 6) || (platform == 128);
		    platformName = Environment.OSVersion.Platform.ToString();
#endif

            IsMono = AssemblyUtils.FindType("Mono.Runtime") != null;

            IsMonoTouch = AssemblyUtils.FindType("MonoTouch.Foundation.NSObject") != null;

            IsWinRT = AssemblyUtils.FindType("Windows.ApplicationModel") != null;

			SupportsExpressions = SupportsEmit = !IsMonoTouch;

            ServerUserAgent = "ServiceStack/" +
                ServiceStackVersion + " "
                + platformName
                + (IsMono ? "/Mono" : "/.NET")
                + (IsMonoTouch ? " MonoTouch" : "")
                + (IsWinRT ? ".NET WinRT" : "");
		}

		/// <summary>
		/// The service stack version
		/// </summary>
		public static decimal ServiceStackVersion = 3.960m;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is unix.
		/// </summary>
		/// <value><c>true</c> if this instance is unix; otherwise, <c>false</c>.</value>
		public static bool IsUnix { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is mono.
		/// </summary>
		/// <value><c>true</c> if this instance is mono; otherwise, <c>false</c>.</value>
		public static bool IsMono { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is mono touch.
		/// </summary>
		/// <value><c>true</c> if this instance is mono touch; otherwise, <c>false</c>.</value>
		public static bool IsMonoTouch { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is win rt.
		/// </summary>
		/// <value><c>true</c> if this instance is win rt; otherwise, <c>false</c>.</value>
		public static bool IsWinRT { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [supports expressions].
		/// </summary>
		/// <value><c>true</c> if [supports expressions]; otherwise, <c>false</c>.</value>
		public static bool SupportsExpressions { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [supports emit].
		/// </summary>
		/// <value><c>true</c> if [supports emit]; otherwise, <c>false</c>.</value>
		public static bool SupportsEmit { get; set; }

		/// <summary>
		/// Gets or sets the server user agent.
		/// </summary>
		/// <value>The server user agent.</value>
		public static string ServerUserAgent { get; set; }
	}
}