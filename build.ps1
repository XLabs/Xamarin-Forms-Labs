Param(
    [Parameter(Position=1,Mandatory=0)]
    [string[]]$task_list = @(),

	[Parameter()]
    [string]$version = $null,
	
	[Parameter()]
    [string]$preRelease = $null,
	
	[Parameter()]
    [string]$configuration = "Release",
	
	[Parameter()]
	[bool]$versionUpdate = $false
)

$build_file = 'default.ps1'

# Properties for the psake build script
$properties = @{

    # Build configuration to use
    "configuration" = $configuration;

    # Version number to use if running the Publish build task.
    # This will be read from the command line args
    "version"       = $version;

    # Path to the solution file
    "solution"      = 'XLabs.sln';

    # Folder containing source code
    "source_folder" = '';

    # Folder to output deployable packages to. This folder should be ignored
    # from any source control, as we dont commit build artifacts to source
    # control
    "deploy_folder" = 'deploy';

	# List of projects to use when building NuGet Packages (Note: Not used for XLabs)
    "projects" = @(
	);
	
	# Is the Nuget package a pre-release version?
	"preRelease" = $preRelease;

	# Update the version numbers automatically
	"versionUpdate" = $versionUpdate
}

import-module .\packages\psake.4.4.2\tools\psake.psm1

invoke-psake $build_file $task_list -Properties $properties