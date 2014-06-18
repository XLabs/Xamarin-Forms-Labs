#==========================================================
# This script must be run manually.
#
# This script prompts you for a NuGet package file (.nupkg) and then uploads it to the gallery. The project's .nupkg file should be in the same directory as the project's .dll/.exe file (typically bin\Debug or bin\Release).
#
# To run this script from inside Visual Studio, simply right-click on the file and choose "Run with PowerShell". If a command prompt just quickly appears and disappears,
#	then run the "UploadPackage-RunManually.bat" file instead, by right-clicking it and choosing "Run".
# 
# You may edit the values of the $sourceToUploadTo, $apiKey, and $pushOptions variables below to adjust the settings used to push the package to the gallery.
#
# If you have modified this script:
#	- if you uninstall the "Create New NuGet Package From Project After Each Build" package, this file will not be removed automatically; you will need to manually delete it.
#	- if you update the "Create New NuGet Package From Project After Each Build" package, this file will not be updated unless you provide the "-FileConflictAction Overwrite" parameter 
#		when installing. Also, if you do this then your custom changes will be lost. It might be easiest to backup the file, uninstall the package, delete the left-over file,
#		reinstall the package, and then re-apply your custom changes.
#==========================================================
$THIS_SCRIPTS_DIRECTORY = Split-Path $script:MyInvocation.MyCommand.Path

#################################################
# Users May Edit The Following Variables.
#################################################

# The NuGet gallery to upload to. If not provided, the DefaultPushSource in your NuGet.config file is used (typically nuget.org).
$sourceToUploadTo = ""

# The API Key to use to upload the package to the gallery. If not provided and a system-level one does not exist for the specified Source, you will be prompted for it.
$apiKey = ""

# Specify any additional NuGet Push options to pass to nuget.exe.
# Rather than specifying the -Source or -ApiKey here, use the variables above.
$pushOptions = ""

#################################################
# Do Not Edit Anything Past This Point (except to add the "-Verbose" flag to the end of the last line for troubleshooting purposes).
#################################################

# Add the Source and ApiKey to the Push Options if there were provided.
if (![string]::IsNullOrWhiteSpace($sourceToUploadTo)) { $pushOptions += " -Source ""$sourceToUploadTo"" " }
if (![string]::IsNullOrWhiteSpace($apiKey)) { $pushOptions += " -ApiKey ""$apiKey"" " }

# Create the new NuGet package.
& "$THIS_SCRIPTS_DIRECTORY\New-NuGetPackage.ps1" -PushOptions "$pushOptions"