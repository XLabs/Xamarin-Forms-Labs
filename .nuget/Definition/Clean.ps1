param($package) 

if ($package -eq $null)
{
    Write-Host "Clean a NuGet build output."
    Write-Host "============================"
    Write-Host "Usage: Clean.ps1 ""<NuGet Name>"""
    Write-Host ">E.g.: Clean.ps1 ""Core"""
    return
}

try 
{
    ## Initialise
    ## ----------
    $configuration = "Release"
    $basePath = Get-Location
    $pathToNuGetLib = [System.IO.Path]::GetFullPath( "$basePath\$package" )
    
    $originalBackground = $host.UI.RawUI.BackgroundColor
    $originalForeground = $host.UI.RawUI.ForegroundColor
    
    $host.UI.RawUI.BackgroundColor = [System.ConsoleColor]::Black
    $host.UI.RawUI.ForegroundColor = [System.ConsoleColor]::White

    Write-Host "Clean NuGet build outputs for: $package" -ForegroundColor White
    Write-Host "==============================" -ForegroundColor White

    ## NB - Cleanup destination folders
    ## --------------------------------
    Write-Host "Clean destination folders..." -ForegroundColor Yellow
    ##Remove-Item "$pathToNuGetLib\*" -Exclude *.NuSpec -Recurse -Force -ErrorAction SilentlyContinue
    Remove-Item "$basePath\*.NuPkg" -Recurse -Force -ErrorAction SilentlyContinue

    Write-Host "Clean operation done." -ForegroundColor Green
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
    $host.UI.RawUI.BackgroundColor = $originalBackground
    $host.UI.RawUI.ForegroundColor = $originalForeground
}
#Pause # For debugging purposes