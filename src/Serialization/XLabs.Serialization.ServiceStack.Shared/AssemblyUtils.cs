// ***********************************************************************
// <copyright file="AssemblyUtils.cs" company="XLabs">
//     Copyright © ServiceStack 2013 & XLabs
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using ServiceStack.Common.Support;

namespace ServiceStack.Text
{
	/// <summary>
	/// Utils to load types
	/// </summary>
	public static class AssemblyUtils
    {
		/// <summary>
		/// The file URI
		/// </summary>
		private const string FileUri = "file:///";
		/// <summary>
		/// The DLL ext
		/// </summary>
		private const string DllExt = "dll";
		/// <summary>
		/// The executable ext
		/// </summary>
		private const string ExeExt = "dll";
		/// <summary>
		/// The URI seperator
		/// </summary>
		private const char UriSeperator = '/';

		/// <summary>
		/// The type cache
		/// </summary>
		private static Dictionary<string, Type> TypeCache = new Dictionary<string, Type>();

#if !XBOX
		/// <summary>
		/// Find the type from the name supplied
		/// </summary>
		/// <param name="typeName">[typeName] or [typeName, assemblyName]</param>
		/// <returns>Type.</returns>
		public static Type FindType(string typeName)
        {
            Type type = null;
            if (TypeCache.TryGetValue(typeName, out type)) return type;

#if !SILVERLIGHT || WINDOWS_PHONE
            type = Type.GetType(typeName);
#endif
            if (type == null)
            {
                var typeDef = new AssemblyTypeDefinition(typeName);
                type = !string.IsNullOrEmpty(typeDef.AssemblyName) 
                    ? FindType(typeDef.TypeName, typeDef.AssemblyName) 
                    : FindTypeFromLoadedAssemblies(typeDef.TypeName);
            }

            Dictionary<string, Type> snapshot, newCache;
            do
            {
                snapshot = TypeCache;
                newCache = new Dictionary<string, Type>(TypeCache);
                newCache[typeName] = type;

            } while (!ReferenceEquals(
                Interlocked.CompareExchange(ref TypeCache, newCache, snapshot), snapshot));

            return type;
        }
#endif

#if !XBOX


		/// <summary>
		/// The top-most interface of the given type, if any.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>Type.</returns>
		public static Type MainInterface<T>() 
        {
			var t = typeof(T);
#if NETFX_CORE
    		if (t.GetTypeInfo().BaseType == typeof(object)) {
				// on Windows, this can be just "t.GetInterfaces()" but Mono doesn't return in order.
				var interfaceType = t.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(i => !t.GetTypeInfo().ImplementedInterfaces.Any(i2 => i2.GetTypeInfo().ImplementedInterfaces.Contains(i)));
				if (interfaceType != null) return interfaceType;
			}
#else
    		if (t.BaseType == typeof(object)) {
				// on Windows, this can be just "t.GetInterfaces()" but Mono doesn't return in order.
				var interfaceType = t.GetInterfaces().FirstOrDefault(i => !t.GetInterfaces().Any(i2 => i2.GetInterfaces().Contains(i)));
				if (interfaceType != null) return interfaceType;
			}
#endif
			return t; // not safe to use interface, as it might be a superclass's one.
		}

		/// <summary>
		/// Find type if it exists
		/// </summary>
		/// <param name="typeName">Name of the type.</param>
		/// <param name="assemblyName">Name of the assembly.</param>
		/// <returns>The type if it exists</returns>
		public static Type FindType(string typeName, string assemblyName)
        {
            var type = FindTypeFromLoadedAssemblies(typeName);
            if (type != null)
            {
                return type;
            }

#if !NETFX_CORE
            var binPath = GetAssemblyBinPath(Assembly.GetExecutingAssembly());
            Assembly assembly = null;
            var assemblyDllPath = binPath + String.Format("{0}.{1}", assemblyName, DllExt);
            if (File.Exists(assemblyDllPath))
            {
                assembly = LoadAssembly(assemblyDllPath);
            }
            var assemblyExePath = binPath + String.Format("{0}.{1}", assemblyName, ExeExt);
            if (File.Exists(assemblyExePath))
            {
                assembly = LoadAssembly(assemblyExePath);
            }
            return assembly != null ? assembly.GetType(typeName) : null;
#else
            return null;
#endif
        }
#endif

#if NETFX_CORE
        private sealed class AppDomain
        {
            public static AppDomain CurrentDomain { get; private set; }
            public static Assembly[] cacheObj = null;
 
            static AppDomain()
            {
                CurrentDomain = new AppDomain();
            }
 
            public Assembly[] GetAssemblies()
            {
                return cacheObj ?? GetAssemblyListAsync().Result.ToArray();
            }
 
            private async System.Threading.Tasks.Task<IEnumerable<Assembly>> GetAssemblyListAsync()
            {
                var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
 
                List<Assembly> assemblies = new List<Assembly>();
                foreach (Windows.Storage.StorageFile file in await folder.GetFilesAsync())
                {
                    if (file.FileType == ".dll" || file.FileType == ".exe")
                    {
                        try
                        {
                            var filename = file.Name.Substring(0, file.Name.Length - file.FileType.Length);
                            AssemblyName name = new AssemblyName() { Name = filename };
                            Assembly asm = Assembly.Load(name);
                            assemblies.Add(asm);
                        }
                        catch (Exception)
                        {
                            // Invalid WinRT assembly!
                        }
                    }
                }

                cacheObj = assemblies.ToArray();
 
                return cacheObj;
            }
        }
#endif

#if !XBOX
		/// <summary>
		/// Finds the type from loaded assemblies.
		/// </summary>
		/// <param name="typeName">Name of the type.</param>
		/// <returns>Type.</returns>
		public static Type FindTypeFromLoadedAssemblies(string typeName)
        {
#if SILVERLIGHT4
        	var assemblies = ((dynamic) AppDomain.CurrentDomain).GetAssemblies() as Assembly[];
#else
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
#endif
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }
#endif

#if !SILVERLIGHT
        private static Assembly LoadAssembly(string assemblyPath)
		{
			return Assembly.LoadFrom(assemblyPath);
		}
#elif NETFX_CORE
        private static Assembly LoadAssembly(string assemblyPath)
        {
            return Assembly.Load(new AssemblyName(assemblyPath));
        }
#elif WINDOWS_PHONE
		/// <summary>
		/// Loads the assembly.
		/// </summary>
		/// <param name="assemblyPath">The assembly path.</param>
		/// <returns>Assembly.</returns>
		private static Assembly LoadAssembly(string assemblyPath)
        {
            return Assembly.LoadFrom(assemblyPath);
        }
#else
        private static Assembly LoadAssembly(string assemblyPath)
        {
            var sri = System.Windows.Application.GetResourceStream(new Uri(assemblyPath, UriKind.Relative));
            var myPart = new System.Windows.AssemblyPart();
            var assembly = myPart.Load(sri.Stream);
            return assembly;
        }
#endif

#if !XBOX
		/// <summary>
		/// Gets the assembly bin path.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <returns>System.String.</returns>
		public static string GetAssemblyBinPath(Assembly assembly)
        {
#if WINDOWS_PHONE
            var codeBase = assembly.GetName().CodeBase;
#elif NETFX_CORE
            var codeBase = assembly.GetName().FullName;
#elif SILVERLIGHT
            var codeBase = assembly.FullName;
#else
            var codeBase = assembly.CodeBase;
#endif

            var binPathPos = codeBase.LastIndexOf(UriSeperator);
            var assemblyPath = codeBase.Substring(0, binPathPos + 1);
            if (assemblyPath.StartsWith(FileUri, StringComparison.OrdinalIgnoreCase))
            {
                assemblyPath = assemblyPath.Remove(0, FileUri.Length);
            }
            return assemblyPath;
        }
#endif

#if !SILVERLIGHT
		static readonly Regex versionRegEx = new Regex(", Version=[^\\]]+", RegexOptions.Compiled);
#else
		/// <summary>
		/// The version reg ex
		/// </summary>
		static readonly Regex versionRegEx = new Regex(", Version=[^\\]]+");
#endif
		/// <summary>
		/// To the type string.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.String.</returns>
		public static string ToTypeString(this Type type)
        {
            return versionRegEx.Replace(type.AssemblyQualifiedName, "");
        }

		/// <summary>
		/// Writes the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.String.</returns>
		public static string WriteType(Type type)
        {
            return type.ToTypeString();
        }
    }
}