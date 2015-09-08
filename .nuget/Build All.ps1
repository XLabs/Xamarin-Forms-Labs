##
##	Create Modular Xamarin Forms NuGet based on UI Technology
##  ==========================================================================================
##  
##  “XLabs - Platform” NuGet
##  •	Contents:
##      o	XLabs.Platform.dll (PCL - Xamarin Android, Xamarin iOS, Windows Phone 8)
##      o	XLabs.Platform.Droid.dll (MonoAndroid - Xamarin Android)
##      o	XLabs.Platform.iOS.dll (MonoIos - Xamarin iOS)
##      o	XLabs.Platform.WP.dll (WinPRT - Windows Phone 8)
##  •	Dependencies
##      o	"Xamarin.Forms" NuGet
##  
##  “XLabs - Services Caching” NuGet
##  •	Contents:
##      o	XLabs.Caching.SQLiteNet.dll (PCL - Xamarin Android, Xamarin iOS, Windows Phone 8)
##  •	Dependencies
##      o	“XLabs - Platform” NuGet
##      o	“SQLite.Net” NuGet
##  
##  “XLabs - Services Cryptography” NuGet
##  •	Contents:
##      o	XLabs.Cryptography.dll (PCL - Xamarin Android, Xamarin iOS, Windows Phone 8)
##  •	Dependencies
##      o	None
##  
##  “XLabs - Services IoC Autofac” NuGet
##  •	Contents:
##      o	XLabs.Services.Autofac.dll (PCL - Xamarin Android, Xamarin iOS, Windows Phone 8)
##  •	Dependencies
##      o	“Autofac” NuGet
##  
##  “XLabs - Services IoC Ninject” NuGet
##  •	Contents:
##      o	XLabs.Services.Ninject.dll (PCL - Xamarin Android, Xamarin iOS, Windows Phone 8)
##  •	Dependencies
##      o	“XLabs - Core” NuGet
##      o	“Ninject” NuGet
##
##  “XLabs - Services IoC SimpleInjector" NuGet
##  •	Contents:
##      o	XLabs.Services.SimpleInjector.dll (PCL - Xamarin Android, Xamarin iOS, Windows Phone 8)
##  •	Dependencies
##      o	“XLabs - Core” NuGet
##      o	“SimpleInjector” NuGet
##  
##  “XLabs - Services IoC TinyIOC" NuGet
##  •	Contents:
##      o	XLabs.Services.TinyIOC.dll (.NET 4.5)
##      o	XLabs.Services.TinyIOC.Droid.dll (MonoAndroid - Xamarin Android)
##      o	XLabs.Services.TinyIOC.iOS.dll (MonoIos - Xamarin iOS)
##      o	XLabs.Services.TinyIOC.WP8.dll (WinPRT - Windows Phone 8)
##  •	Dependencies
##      o	“XLabs - Core” NuGet
##  
##  “XLabs - Services Serialization JSON" NuGet
##  •	Contents:
##      o	XLabs.Services.Serialization.JsonNET.dll (PCL - Xamarin Android, Xamarin iOS, Windows Phone 8)
##  •	Dependencies
##      o	“XLabs - Core” NuGet
##      o	“Newtonsoft JSON” NuGet
##
##  “XLabs - Services Serialization ProtoBuf" NuGet
##  •	Contents:
##      o	XLabs.Services.Serialization.ProtoBuf.dll (PCL - Xamarin Android, Xamarin iOS, Windows Phone 8)
##  •	Dependencies
##      o	“XLabs - Core” NuGet
##      o	“ProtoBuf-net” NuGet
##  
##  “XLabs - Services Serialization ServiceStack" NuGet
##  •	Contents:
##      o	XLabs.Services.Serialization.ServiceStackV3.dll (.NET 4.5)
##      o	XLabs.Services.Serialization.ServiceStackV3.Droid.dll (MonoAndroid - Xamarin Android)
##      o	XLabs.Services.Serialization.ServiceStackV3.iOS.dll (MonoIos - Xamarin iOS)
##      o	XLabs.Services.Serialization.ServiceStackV3.WP8.dll (WinPRT - Windows Phone 8)
##  •	Dependencies
##      o	“XLabs - Core” NuGet
##      o	“XLabs - Serialization” NuGet
##
##  “XLabs - Caching” NuGet
##  •	Contents:
##      o	Xamarin.Forms..Labs.Caching.dll (PCL - Xamarin Android, Xamarin iOS, Windows Phone 8)
##      o	XLabs.CachingDroid.dll (MonoAndroid - Xamarin Android)
##      o	XLabs.CachingiOS.dll (MonoIos - Xamarin iOS)
##      o	XLabs.CachingWP.dll (WinPRT - Windows Phone 8)

[CmdletBinding()]
param( 
    [Parameter(Mandatory = $False)]
    [string] $version = $null,
    [Parameter(Mandatory = $False)]
    [string] $preRelease = $null,
    [Parameter(Mandatory = $False)]
    [bool] $versionUpdate = $true
)


function OutputCommandLineUsageHelp()
{
    Write-Host "Build all NuGet packages."
    Write-Host "============================"
    Write-Host ">E.g.: Build All.ps1"
    Write-Host ">E.g.: Build All.ps1 -PreRelease pre1"
    Write-Host ">E.g.: Build All.ps1 -Version 1.3.0"
    Write-Host ">E.g.: Build All.ps1 -Version 1.3.0 -PreRelease pre1"
}

function Pause ($Message="Press any key to continue...")
{
    Write-Host -NoNewLine $Message
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    Write-Host ""
}

try 
{
    ## Initialise
    ## ----------
    $originalBackground = $host.UI.RawUI.BackgroundColor
    $originalForeground = $host.UI.RawUI.ForegroundColor
    $originalLocation = Get-Location
    #$packages = @("Core", "Services Caching", "Services Cryptography", "Services IoC AutoFac", "Services IoC Ninject", "Services IoC SimpleInjector", "Services IoC TinyIOC", "Services Serialization JSON", "Services Serialization ProtoBuf", "Services Serialization ServiceStack", "Charting", "Services IoC Unity")  
    $packages = (Get-Item "$originalLocation\Definition\XLabs.*.NuSpec" | % { $_.BaseName })
    
    $host.UI.RawUI.BackgroundColor = [System.ConsoleColor]::Black
    $host.UI.RawUI.ForegroundColor = [System.ConsoleColor]::White
    
    Write-Host "Build All XLabs NuGet packages" -ForegroundColor White
    Write-Host "==================================" -ForegroundColor White

    Write-Host "Creating Packages folder" -ForegroundColor Yellow
    if (-Not (Test-Path .\Packages)) {
        mkdir Packages
    }

    ## NB - Cleanup destination package folder
    ## ---------------------------------------
    Write-Host "Clean destination folders..." -ForegroundColor Yellow
    Remove-Item ".\Packages\*.nupkg" -Recurse -Force -ErrorAction SilentlyContinue
    
    ## Spawn off individual build processes...
    ## ---------------------------------------
    Set-Location "$originalLocation\Definition" ## Adjust current working directory since scripts are using relative paths
    $packages | ForEach { & ".\Build.ps1" -package $_ -version $version -preRelease $preRelease -versionUpdate $versionUpdate }
    Write-Host "Build All - Done." -ForegroundColor Green
}
catch 
{
    $baseException = $_.Exception.GetBaseException()
    if ($_.Exception -ne $baseException)
    {
      Write-Host $baseException.Message -ForegroundColor Magenta
    }
    Write-Host $_.Exception.Message -ForegroundColor Magenta
    Pause
} 
finally 
{
    ## Restore original values
    $host.UI.RawUI.BackgroundColor = $originalBackground
    $host.UI.RawUI.ForegroundColor = $originalForeground
    Set-Location $originalLocation
}
Pause # For debugging purpose
