param($installPath, $toolsPath, $package)

$DTE.ItemOperations.Navigate("https://github.com/XLabs/Xamarin-Forms-Labs/wiki/Charting?" + $package.Id + "=" + $package.Version)