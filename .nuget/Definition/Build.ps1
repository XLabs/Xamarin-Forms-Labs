[CmdletBinding()]
param( 
	[Parameter(Mandatory = $True, ValueFromPipeline = $True)]
	[string] $package,  
	[Parameter(Mandatory = $False)]
	[string] $version = $null,
	[Parameter(Mandatory = $False)]
	[string] $preRelease = $null
)

function OutputCommandLineUsageHelp()
{
	Write-Host "Create a NuGet build output."
	Write-Host "============================"
	Write-Host "Usage: Build.ps1 ""<NuGet Package Name>"" [-PreRelease <PreReleaseVersion>] [-Version <VersionNumber>]"
	Write-Host ">E.g.: Build.ps1 ""XLabs.Platform"""
	Write-Host ">E.g.: Build.ps1 ""XLabs.Platform"" -PreRelease pre1"
	Write-Host ">E.g.: Build.ps1 ""XLabs.Platform"" -Version 1.3.0"
	Write-Host ">E.g.: Build.ps1 ""XLabs.Platform"" -Version 1.3.0 -PreRelease pre1"
}

function Pause( $Message="Press any key to continue..." )
{
	Write-Host -NoNewLine $Message
	$null = $Host.UI.RawUI.ReadKey( "NoEcho,IncludeKeyDown" )
	Write-Host ""
}

function ChangeNuSpecVersion( [string] $nuSpecFilePath, [string] $version="0.0.0.0", [string] $nuSpecFilePathTmp = $null )
{
	Write-Host "Dynamically setting NuSpec version: $Version" -ForegroundColor Yellow
	
	# Get full path or save operation fails when launched in standalone powershell
	$nuSpecFile = Get-Item $nuSpecFilePath | Select-Object -First 1
	
	# Bring the XML Linq namespace in
	[Reflection.Assembly]::LoadWithPartialName( “System.Xml.Linq” ) | Out-Null
	
	# Update the XML document with the new version
	$xDoc = [System.Xml.Linq.XDocument]::Load( $nuSpecFile.FullName )
	$versionNode = $xDoc.Descendants( "version" ) | Select-Object -First 1
	if ($versionNode -ne $null)
	{
		$versionNode.SetValue($version)
	}
	
	# Update the XML document dependencies with the new version
	$dependencies = $xDoc.Descendants( "dependency" )
	foreach( $dependency in $dependencies )
	{
		$idAttribute = $dependency.Attributes( "id" ) | Select-Object -First 1
		if ( $idAttribute -ne $null )
		{
			if ( $idAttribute.Value -like "XLabs.*" )
			{
				$dependency.SetAttributeValue( "version", "[$version]" )
			}
		}
	}
	
	# Save file
	if ($nuSpecFilePathTmp -ne $null) 
	{ 
		Write-Host "Creating a temporary NuSpec file with the new version" 
		$xDoc.Save( $nuSpecFilePathTmp  )
	} else {
		$xDoc.Save( $nuSpecFile.FullName )
	}
}

function CopyMaintainingSubDirectories( $basePath, $includes, $targetBasePath )
{
	$basePathLength = [System.IO.Path]::GetFullPath( $basePath ).Length - 1
	$filesToCopy = Get-ChildItem "$basePath\" -Include $includes -Recurse
	#$filesToCopy | Write-Host -ForegroundColor DarkGray # Debug.Print
	foreach( $file in $filesToCopy )
	{ 
		$targetDirectory = Join-Path $targetBasePath $file.Directory.FullName.Substring( $basePathLength )
		if ( (Test-Path $targetDirectory) -ne $true)
		{ 
			[System.IO.Directory]::CreateDirectory( $targetDirectory ) | Out-Null
		}
		Copy-Item $file -Destination $targetDirectory
	}
}

## Validate input parameters
## -------------------------
if ( $package -eq $null )
{
	OutputCommandLineUsageHelp
	return
}

try 
{
	## Initialise
	## ----------
	$basePath = Get-Location
	$pathToNuGetPackager = [System.IO.Path]::GetFullPath( "$basePath\..\NuGet.exe" )
	$pathToNuGetPackageOutput = [System.IO.Path]::GetFullPath( "$basePath\..\Packages" )
	$originalBackground = $host.UI.RawUI.BackgroundColor
	$originalForeground = $host.UI.RawUI.ForegroundColor
	
	$host.UI.RawUI.BackgroundColor = [System.ConsoleColor]::Black
	$host.UI.RawUI.ForegroundColor = [System.ConsoleColor]::White

	Write-Host "Build NuGet packages for: " -ForegroundColor White -NoNewLine
	Write-Host $package -ForegroundColor Cyan
	Write-Host "=========================" -ForegroundColor White

	# Do we have a version or do we need to dynamically load it?
	if ( [System.String]::IsNullOrEmpty( $version ) -ne $true )
	{
		Write-Host "Option: /Version:$version" -ForegroundColor Yellow

		$productVersion = $version
	} else {
		## Update Package Version
		## ----------------------    
		## Before building NuGet package, extract CSLA Version number and update .NuSpec to automate versioning of .NuSpec document
		## - JH: Not sure if I should get direct from source code file or from file version of compiled library instead.
		## - JH: Going with product version in assembly for now
		$pathToBin = [System.IO.Path]::GetFullPath( "$basePath\..\..\build" )
		$pathToAssembly = "$pathToBin\XLabs.Platform.dll"
		$cslaAssembly = (Get-ChildItem $pathToAssembly -ErrorAction SilentlyContinue | Select-Object -First 1)
		if ($cslaAssembly -eq $null) {
			Write-Host "Failed to load version information from $pathToAssembly" -ForegroundColor Red
			exit
		}

		## - JH: If $preRelease is specified, then append it with a dash following the 3rd component of the quad-dotted-version number
		##       Refer: http://docs.nuget.org/docs/Reference/Versioning 
		$productVersion = [System.String]::Format( "{0}.{1}.{2}", $cslaAssembly.VersionInfo.ProductMajorPart, $cslaAssembly.VersionInfo.ProductMinorPart, $cslaAssembly.VersionInfo.ProductBuildPart )
	}

	# Is this a prelease?
	if ( [System.String]::IsNullOrEmpty( $preRelease ) -ne $true )
	{
		Write-Host "Option: /PreRelease:$preRelease" -ForegroundColor Yellow
		$productVersion = [System.String]::Format( "{0}-{1}", $productVersion, $preRelease )
	}

	ChangeNuSpecVersion "$basePath\$package.NuSpec" $productVersion "$basePath\$package.tmp.NuSpec"
	
	## Launch NuGet.exe to build package
	Write-Host "Build NuGet package: $package..." -ForegroundColor Yellow
	& $pathToNuGetPackager pack "$basePath\$package.tmp.NuSpec" -Symbols

	Remove-Item "$basePath\$package.tmp.NuSpec"
	
	## Publish package to Gallery using API Key
	## JH - TODO

	## Move NuGet package to Package Release folder"
	Write-Host "Move package to ..\Packages folder..." -ForegroundColor Yellow
	Move-Item "*.nupkg" -Destination $pathToNuGetPackageOutput -Force
	
	## Cleanup after ourselves
	## JH - TODO

	Write-Host "Done." -ForegroundColor Green
}
catch 
{
	$baseException = $_.Exception.GetBaseException()
	if ( $_.Exception -ne $baseException )
	{
	  Write-Host $baseException.Message -ForegroundColor Magenta
	}
	Write-Host $_.Exception.Message -ForegroundColor Magenta
	Pause
} 
finally 
{
	$host.UI.RawUI.BackgroundColor = $originalBackground
	$host.UI.RawUI.ForegroundColor = $originalForeground
}
#Pause # For debugging purposes