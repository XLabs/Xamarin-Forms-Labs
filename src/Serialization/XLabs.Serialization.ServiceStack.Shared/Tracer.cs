// ***********************************************************************
// <copyright file="Tracer.cs" company="XLabs">
//     Copyright © ServiceStack 2013 & XLabs
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace ServiceStack.Text
{
	/// <summary>
	/// Class Tracer.
	/// </summary>
	public class Tracer
    {
		/// <summary>
		/// The instance
		/// </summary>
		public static ITracer Instance = new NullTracer();

		/// <summary>
		/// Class NullTracer.
		/// </summary>
		public class NullTracer : ITracer
        {
			/// <summary>
			/// Writes the debug.
			/// </summary>
			/// <param name="error">The error.</param>
			public void WriteDebug(string error) { }

			/// <summary>
			/// Writes the debug.
			/// </summary>
			/// <param name="format">The format.</param>
			/// <param name="args">The arguments.</param>
			public void WriteDebug(string format, params object[] args) { }

			/// <summary>
			/// Writes the warning.
			/// </summary>
			/// <param name="warning">The warning.</param>
			public void WriteWarning(string warning) { }

			/// <summary>
			/// Writes the warning.
			/// </summary>
			/// <param name="format">The format.</param>
			/// <param name="args">The arguments.</param>
			public void WriteWarning(string format, params object[] args) { }

			/// <summary>
			/// Writes the error.
			/// </summary>
			/// <param name="ex">The ex.</param>
			public void WriteError(Exception ex) { }

			/// <summary>
			/// Writes the error.
			/// </summary>
			/// <param name="error">The error.</param>
			public void WriteError(string error) { }

			/// <summary>
			/// Writes the error.
			/// </summary>
			/// <param name="format">The format.</param>
			/// <param name="args">The arguments.</param>
			public void WriteError(string format, params object[] args) { }

        }

		/// <summary>
		/// Class ConsoleTracer.
		/// </summary>
		public class ConsoleTracer : ITracer
        {
			/// <summary>
			/// Writes the debug.
			/// </summary>
			/// <param name="error">The error.</param>
			public void WriteDebug(string error)
            {
#if NETFX_CORE
                System.Diagnostics.Debug.WriteLine(error);
#else
                Console.WriteLine(error);
#endif
            }

			/// <summary>
			/// Writes the debug.
			/// </summary>
			/// <param name="format">The format.</param>
			/// <param name="args">The arguments.</param>
			public void WriteDebug(string format, params object[] args)
            {
#if NETFX_CORE
                System.Diagnostics.Debug.WriteLine(format, args);
#else
                Console.WriteLine(format, args);
#endif
            }

			/// <summary>
			/// Writes the warning.
			/// </summary>
			/// <param name="warning">The warning.</param>
			public void WriteWarning(string warning)
            {
#if NETFX_CORE
                System.Diagnostics.Debug.WriteLine(warning);                
#else
                Console.WriteLine(warning);                
#endif
            }

			/// <summary>
			/// Writes the warning.
			/// </summary>
			/// <param name="format">The format.</param>
			/// <param name="args">The arguments.</param>
			public void WriteWarning(string format, params object[] args)
            {
#if NETFX_CORE
                System.Diagnostics.Debug.WriteLine(format, args);
#else
                Console.WriteLine(format, args);
#endif
            }

			/// <summary>
			/// Writes the error.
			/// </summary>
			/// <param name="ex">The ex.</param>
			public void WriteError(Exception ex)
            {
#if NETFX_CORE
                System.Diagnostics.Debug.WriteLine(ex);
#else
                Console.WriteLine(ex);
#endif
            }

			/// <summary>
			/// Writes the error.
			/// </summary>
			/// <param name="error">The error.</param>
			public void WriteError(string error)
            {
#if NETFX_CORE
                System.Diagnostics.Debug.WriteLine(error);
#else
                Console.WriteLine(error);
#endif
            }

			/// <summary>
			/// Writes the error.
			/// </summary>
			/// <param name="format">The format.</param>
			/// <param name="args">The arguments.</param>
			public void WriteError(string format, params object[] args)
            {
#if NETFX_CORE
                System.Diagnostics.Debug.WriteLine(format, args);
#else
                Console.WriteLine(format, args);
#endif
            }
        }
    }
}