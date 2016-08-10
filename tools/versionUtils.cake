#addin "Cake.Json"
#addin "Cake.FileHelpers"

#tool nuget:?package=GitVersion.CommandLine

public class VersionUtils
{
	public static VersionInfo LoadVersion(ICakeContext context, Settings settings)
	{
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

		VersionInfo verInfo = null;
		
		if (!string.IsNullOrEmpty(settings.Version.VersionFile) && context.FileExists(settings.Version.VersionFile))
		{
			verInfo = LoadVersionFromJson(context, settings.Version.VersionFile);
		}
		
		if (verInfo == null && !string.IsNullOrEmpty(settings.Version.AssemblyInfoFile) && context.FileExists(settings.Version.AssemblyInfoFile))
		{
			verInfo = LoadVersionFromAssemblyInfo(context, settings.Version.AssemblyInfoFile);
		}
		
		if (verInfo == null && settings.Version.LoadFromGit)
		{
			verInfo = LoadVersionFromGit(context);
		}
		
		if (verInfo != null)
		{
			verInfo.CakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();
		}
		
		return verInfo;
	}
	
	private static VersionInfo LoadVersionFromJson(ICakeContext context, string versionFile)
	{
		context.Information("Loading Version Info From File: {0}", versionFile);
		if (!context.FileExists(versionFile))
		{
			context.Error("Version File Does Not Exist");
			return null;
		}
		
		var obj = context.DeserializeJsonFromFile<VersionInfo>(versionFile);
		
		return obj;
	}
	
	private static VersionInfo LoadVersionFromAssemblyInfo(ICakeContext context, string assemblyInfoFile)
	{
		context.Information("Fetching Version Info from AssemblyInfo File: {0}", assemblyInfoFile);
		
		if (!context.FileExists(assemblyInfoFile))
		{
			context.Error("AssemblyInfo file does not exist");
			return null;
		}

		try {
			var assemblyInfo = context.ParseAssemblyInfo(assemblyInfoFile);
			var v = Version.Parse(assemblyInfo.AssemblyVersion);
			
			var verInfo = new VersionInfo {
				Major = v.Major,
				Minor = v.Minor,
				Build = v.Build,
				Semantic = assemblyInfo.AssemblyInformationalVersion,
				Milestone = string.Concat("v", v.ToString())
			};
			
			return verInfo;
		}
		catch {}
		
		return null;
	}
	
	private static VersionInfo LoadVersionFromGit(ICakeContext context)
	{
		context.Information("Fetching Verson Infop from Git");

		try {
			GitVersion assertedVersions = context.GitVersion(new GitVersionSettings
			{
				OutputType = GitVersionOutput.Json,
			});

			var verInfo = new VersionInfo {
				Major = assertedVersions.Major,
				Minor = assertedVersions.Minor,
				Build = assertedVersions.Patch,
				Semantic = assertedVersions.LegacySemVerPadded,
				Milestone = string.Concat("v", assertedVersions.MajorMinorPatch)
			};

			context.Information("Calculated Semantic Version: {0}", verInfo.Semantic);

			return verInfo;
		} catch {}

		return null;
	}
	
	public static void UpdateVersion(ICakeContext context, Settings settings, VersionInfo verInfo)
	{
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

		if (!string.IsNullOrEmpty(settings.Version.VersionFile) && context.FileExists(settings.Version.VersionFile))
		{	
			context.Information("Updating Version File {0}", settings.Version.VersionFile);
			
			context.SerializeJsonToFile(settings.Version.VersionFile, verInfo);
		}
		
		if (!string.IsNullOrEmpty(settings.Version.AssemblyInfoFile) && context.FileExists(settings.Version.AssemblyInfoFile))
		{
			context.Information("Updating Assembly Info File {0}", settings.Version.AssemblyInfoFile);
			
			context.ReplaceRegexInFiles(settings.Version.AssemblyInfoFile, "AssemblyVersion\\(.*\\)", string.Format("AssemblyVersion(\"{0}\")", verInfo.ToString(false)));
			context.ReplaceRegexInFiles(settings.Version.AssemblyInfoFile, "AssemblyFileVersion\\(.*\\)", string.Format("AssemblyFileVersion(\"{0}\")", verInfo.ToString(false)));
		}
	}

	public static void UpdateNuSpecVersion(ICakeContext context, Settings settings, VersionInfo verInfo, FilePath nuspecFile)
	{
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

		var xpq = string.Format("/n:package/n:metadata/n:version");
		
		context.Information("\tUpdating Version in Nuspec File {0} to {1}", nuspecFile, verInfo.ToString());
		
		try {
			context.XmlPoke(nuspecFile, xpq, verInfo.ToString(), new XmlPokeSettings {
				PreserveWhitespace = true
				, Namespaces = new Dictionary<string, string> {
					 { /* Prefix */ "n", /* URI */ "http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"}
				 }
			});
		} catch {} // Its ok to throw these away as it most likely means the file didn't exist or the XPath didn't find any nodes
	}
	
	public static void UpdateNuSpecVersionDependency(ICakeContext context, Settings settings, VersionInfo verInfo, FilePath nuspecFile)
	{
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

		if (string.IsNullOrEmpty(settings.Version.NamespaceBase)) return;
		
		var xpq = string.Format("/n:package/n:metadata/n:dependencies//n:dependency[starts-with(@id, '{0}')]/@version", settings.Version.NamespaceBase);
		var replacementStr = verInfo.ToString();

		switch (settings.NuGet.VersionDependencyForLibrary)
		{
			case VersionDependencyTypes.none: break;
			case VersionDependencyTypes.exact: replacementStr = string.Format("[{0}]", replacementStr); break;
			case VersionDependencyTypes.greaterthan: replacementStr = string.Format("(,{0})", replacementStr); break;
			case VersionDependencyTypes.greaterthanorequal: replacementStr = string.Format("(,{0}]", replacementStr); break;
			case VersionDependencyTypes.lessthan: replacementStr = string.Format("({0},)", replacementStr); break;
		}
		
		context.Information("\tUpdating Version for {0} Namespace Assemblies in Nuspec File {1} to {2}", settings.Version.NamespaceBase, nuspecFile, replacementStr);
		
		try {
			context.XmlPoke(nuspecFile, xpq, replacementStr, new XmlPokeSettings {
				PreserveWhitespace = true
				, Namespaces = new Dictionary<string, string> {
					 { /* Prefix */ "n", /* URI */ "http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"}
				 }
			});
		} catch {} // Its ok to throw these away as it most likely means the file didn't exist or the XPath didn't find any nodes
	}
}

public class VersionInfo
{
	[Newtonsoft.Json.JsonProperty("major")]
	public int Major {get;set;}
	[Newtonsoft.Json.JsonProperty("minor")]
	public int Minor {get;set;}
	[Newtonsoft.Json.JsonProperty("build")]
	public int Build {get;set;}
	[Newtonsoft.Json.JsonProperty("preRelease")]
	public int? PreRelease {get;set;}
	[Newtonsoft.Json.JsonProperty("releaseNotes")]
	public string[] ReleaseNotes {get;set;}

	[Newtonsoft.Json.JsonIgnore]
	public string Semantic {get;set;}
	[Newtonsoft.Json.JsonIgnore]
	public string Milestone {get;set;}
	[Newtonsoft.Json.JsonIgnore]
	public string CakeVersion {get;set;}
	
	[Newtonsoft.Json.JsonIgnore]
	public bool IsPreRelease { get { return PreRelease != null && PreRelease != 0; } }

	public new string ToString(bool includePreRelease = true) 
	{ 
		var str = string.Format("{0:#0}.{1:#0}.{2:#0}", Major, Minor, Build);
		if (IsPreRelease && includePreRelease) str += string.Format("-pre{0:00}", PreRelease);

		return str; 
	}
	
	public void Display(ICakeContext context)
	{
		context.Information("Version:");
		context.Information("\tMajor: {0}", Major);
		context.Information("\tMinor: {0}", Minor);
		context.Information("\tBuild: {0}", Build);
		context.Information("\tIs PreRelease: {0}", IsPreRelease);
		context.Information("\tPreRelease: {0}", PreRelease);
		context.Information("\tSemantic: {0}", Semantic);
		context.Information("\tMilestone: {0}", Milestone);
		context.Information("\tCake Version: {0}", CakeVersion);
		
		if (ReleaseNotes != null) context.Information("\tRelease Notes: {0}", ReleaseNotes);
	}
}