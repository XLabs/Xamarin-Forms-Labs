using System;

namespace ServiceStack.Text
{
    public class Tracer
    {
        public static ITracer Instance = new NullTracer();

        public class NullTracer : ITracer
        {
            public void WriteDebug(string error) { }

            public void WriteDebug(string format, params object[] args) { }
            
            public void WriteWarning(string warning) { }

            public void WriteWarning(string format, params object[] args) { }

            public void WriteError(Exception ex) { }

            public void WriteError(string error) { }

            public void WriteError(string format, params object[] args) { }

        }

        public class ConsoleTracer : ITracer
        {
            public void WriteDebug(string error)
            {
#if NETFX_CORE
                System.Diagnostics.Debug.WriteLine(error);
#else
                Console.WriteLine(error);
#endif
            }

            public void WriteDebug(string format, params object[] args)
            {
#if NETFX_CORE
                System.Diagnostics.Debug.WriteLine(format, args);
#else
                Console.WriteLine(format, args);
#endif
            }

            public void WriteWarning(string warning)
            {
#if NETFX_CORE
                System.Diagnostics.Debug.WriteLine(warning);                
#else
                Console.WriteLine(warning);                
#endif
            }

            public void WriteWarning(string format, params object[] args)
            {
#if NETFX_CORE
                System.Diagnostics.Debug.WriteLine(format, args);
#else
                Console.WriteLine(format, args);
#endif
            }

            public void WriteError(Exception ex)
            {
#if NETFX_CORE
                System.Diagnostics.Debug.WriteLine(ex);
#else
                Console.WriteLine(ex);
#endif
            }

            public void WriteError(string error)
            {
#if NETFX_CORE
                System.Diagnostics.Debug.WriteLine(error);
#else
                Console.WriteLine(error);
#endif
            }

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