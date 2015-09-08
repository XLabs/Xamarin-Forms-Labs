
using System;

namespace ServiceStack.Text
{
	public static class Env
	{
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

		public static decimal ServiceStackVersion = 3.960m;

		public static bool IsUnix { get; set; }

		public static bool IsMono { get; set; }

		public static bool IsMonoTouch { get; set; }

		public static bool IsWinRT { get; set; }

		public static bool SupportsExpressions { get; set; }

		public static bool SupportsEmit { get; set; }

		public static string ServerUserAgent { get; set; }
	}
}