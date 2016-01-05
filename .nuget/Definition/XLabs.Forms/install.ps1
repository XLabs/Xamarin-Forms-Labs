param($installPath, $toolsPath, $package)

$DTE.ItemOperations.Navigate("https://github.com/XLabs/Xamarin-Forms-Labs/wiki?" + $package.Id + "=" + $package.Version)