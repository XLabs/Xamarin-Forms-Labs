##
##	Publish All Packages
##  ====================
##  Remeber the initial once off setting of your API Key
##  
##    .\NuGet SetApiKey Your-API-Key
##

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
    
    $basePath = Get-Location
    $pathToNuGetPackager = [System.IO.Path]::GetFullPath( "$basePath\NuGet.exe" )
    $pathToNuGetPackageOutput = [System.IO.Path]::GetFullPath( "$basePath\Packages" )
    
    $host.UI.RawUI.BackgroundColor = [System.ConsoleColor]::Black
    $host.UI.RawUI.ForegroundColor = [System.ConsoleColor]::White
    
    Write-Host "Publish all XLabs NuGet packages" -ForegroundColor White
    Write-Host "====================================" -ForegroundColor White
    
    ## Get list of packages (without ".symbols.") from Packages folder
    ## ---------------------------------------------------------------
    Write-Host "Get list of packages..." -ForegroundColor Yellow
    $packages = Get-ChildItem $pathToNuGetPackageOutput -Exclude "*.symbols.*"
    
    ## Spawn off individual publish processes...
    ## -----------------------------------------
    Write-Host "Publishing packages..." -ForegroundColor Yellow
    $packages | ForEach { & $pathToNuGetPackager Push "$_" }
    Write-Host "Publish all done." -ForegroundColor Green
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
Pause # For debugging purposes