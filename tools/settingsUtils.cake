#addin "Cake.Json"

public class SettingsUtils
{
	public static Settings LoadSettings(ICakeContext context, string settingsFile)
	{
		context.Information("Loading Settings: {0}", settingsFile);
		if (!context.FileExists(settingsFile))
		{
			context.Error("Settings File Does Not Exist");
			return null;
		}

		var obj = context.DeserializeJsonFromFile<Settings>(settingsFile);

		return obj;
	}
}

public class Settings
{
	public VersionSettings Version {get;set;}
	public BuildSettings Build {get;set;}
	public NuGetSettings NuGet {get;set;}

	public void Display(ICakeContext context)
	{
		context.Information("Settings:");
		Version.Display(context);
		Build.Display(context);
		NuGet.Display(context);
	}
}

public class VersionSettings
{
	public VersionSettings()
	{
		LoadFrom = "VersionFile";
	}

	public string VersionFile {get;set;}
	public string AssemblyInfoFile {get;set;}
	public bool LoadFromGit {get;set;}
	public string LoadFrom {get;set;}
	public bool AutoIncrementVersion {get;set;}
	public string NamespaceBase {get;set;}

	public void Display(ICakeContext context)
	{
		context.Information("Version Settings:");
		context.Information("\tVersion File: {0}", VersionFile);
		context.Information("\tAssemblyInfo File: {0}", AssemblyInfoFile);
		context.Information("\tLoad From: {0}", LoadFrom);
		context.Information("\tAutoIncrement Version: {0}", AutoIncrementVersion);
		context.Information("\tNamespace Base: {0}", NamespaceBase);
	}
}

public class BuildSettings
{
	public BuildSettings()
	{
		SourcePath = "./source";
		SolutionFileSpec = "*.sln";
		TreatWarningsAsErrors = false;
		NugetConfigPath = "./.nuget/NuGet.Config";
		EnableXamarinIOS = false;
	}

	public string SourcePath {get;set;}
	public string SolutionFileSpec {get;set;}
	public bool TreatWarningsAsErrors {get;set;}
	public string NugetConfigPath {get;set;}

	public bool EnableXamarinIOS {get;set;}
	public string MacAgentIPAddress {get;set;}
	public string MacAgentUserName {get;set;}
	public string MacAgentUserPassword {get;set;}

	public string SolutionFilePath {
		get {
			if (SolutionFileSpec.Contains("/")) return SolutionFileSpec;

			return string.Format("{0}{1}{2}", SourcePath, SolutionFileSpec.Contains("*") ? "/**/" : "", SolutionFileSpec);
		}
	}

	public void Display(ICakeContext context)
	{
		context.Information("Build Settings:");
		context.Information("\tSource Path: {0}", SourcePath);
		context.Information("\tSolution File Spec: {0}", SolutionFileSpec);
		context.Information("\tSolution File Path: {0}", SolutionFilePath);
		context.Information("\tTreat Warnings As Errors: {0}", TreatWarningsAsErrors);
		context.Information("\tNuget Config Path: {0}", NugetConfigPath);

		context.Information("\tEnable Xamarin IOS: {0}", EnableXamarinIOS);
		context.Information("\tMac Agent IP Address: {0}", MacAgentIPAddress);
		context.Information("\tMac Agent User Name: {0}", MacAgentUserName);
		//context.Information("\tMac Agent User Password: {0}", MacAgentUserPassword);
	}
}

public class NuGetSettings
{
	public NuGetSettings()
	{
		NuSpecPath = "./nuspec";
		NuGetConfig = "./.nuget/NuGet.Config";
		ArtifactsPath = "artifacts/packages";
		UpdateVersion = false;
		VersionDependencyForLibrary = VersionDependencyTypes.none;
	}

	public string NuGetConfig {get;set;}
	public string FeedUrl {get;set;}
	public string FeedApiKey {get;set;}
	public string NuSpecPath {get;set;}
	public string ArtifactsPath {get;set;}
	public bool UpdateVersion {get;set;}
	public VersionDependencyTypes VersionDependencyForLibrary {get;set;}

	public string NuSpecFileSpec {
		get {
			return string.Format("{0}/**/*.nuspec", NuSpecPath);
		}
	}

	public string NuGetPackagesSpec {
		get {
			return string.Format("{0}/*.nupkg", ArtifactsPath);
		}
	}

	public void Display(ICakeContext context)
	{
		context.Information("NuGet Settings:");
		context.Information("\tNuGet Config: {0}", NuGetConfig);
		context.Information("\tFeed Url: {0}", FeedUrl);
		//context.Information("\tFeed API Key: {0}", FeedApiKey);
		context.Information("\tNuSpec Path: {0}", NuSpecPath);
		context.Information("\tNuSpec File Spec: {0}", NuSpecFileSpec);
		context.Information("\tArtifacts Path: {0}", ArtifactsPath);
		context.Information("\tNuGet Packages Spec: {0}", NuGetPackagesSpec);
		context.Information("\tUpdate Version: {0}", UpdateVersion);
		context.Information("\tForce Version Match: {0}", VersionDependencyForLibrary);
	}
}

public class VersionDependencyTypes
{
	public string Value { get; set; }
	public VersionDependencyTypes(string value)
	{
		Value = value;
	}

	public static implicit operator string(VersionDependencyTypes x) {return x.Value;}
	public static implicit operator VersionDependencyTypes(String text) {return new VersionDependencyTypes(text);}

	public static VersionDependencyTypes none = new VersionDependencyTypes("none");
	public static VersionDependencyTypes exact = new VersionDependencyTypes("exact");
	public static VersionDependencyTypes greaterthan = new VersionDependencyTypes("greaterthan");
	public static VersionDependencyTypes greaterthanorequal = new VersionDependencyTypes("greaterthanorequal");
	public static VersionDependencyTypes lessthan = new VersionDependencyTypes("lessthan");
}