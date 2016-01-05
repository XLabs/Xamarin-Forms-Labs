param($installPath, $toolsPath, $package)

$DTE.ItemOperations.Navigate("https://github.com/XLabs/Xamarin-Forms-Labs/wiki/IOC?" + $package.Id + "=" + $package.Version)