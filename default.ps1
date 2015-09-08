properties {
    $projects = $null
    $configuration = "Release"
    $source_folder = $null
    $solution = $null
	$build_meta = $null
	$preRelease = $null
}

Task Default -Depends Build

Task RestorePackages {
	Exec { nuget restore $solution -PackagesDirectory .\packages }
}

Task Publish -Depends Package {
    $version = getVersionBase
    
	exec { & ".\.nuget\Push All.ps1"}	
}

Task Package -Depends {
    $version = getVersionBase
    
	exec { & ".\.nuget\Build All.ps1" "-version $version -preRelease $preRelease"}
}

Task Test -Depends Build {
#    Get-ChildItem $source_folder -Recurse -Include *Tests.csproj | % {
#        Exec { & ".\packages\xunit.runner.console.2.0.0\tools\xunit.console.exe" "$($_.DirectoryName)\bin\$configuration\$($_.BaseName).dll" }
#    }
}

Task Build -Depends Clean,Set-Versions,RestorePackages {
    Exec { msbuild "$solution" /t:Build /p:Configuration=$configuration } 
}

Task Clean {
    Exec { msbuild "$solution" /t:Clean /p:Configuration=$configuration } 
	exec { & ".\.nuget\Clean All.ps1"}
}

Task Set-Versions {
    $version = getVersionBase

	if ($build_meta) {
        "##teamcity[buildNumber '$version+$build_meta']" | Write-Host
    } else {
		"##teamcity[buildNumber '$version']" | Write-Host
	}

    Get-ChildItem -Recurse -Force | Where-Object { $_.Name -eq "AssemblyInfo.cs" } | ForEach-Object {
        (Get-Content $_.FullName) | ForEach-Object {
            ($_ -replace 'AssemblyVersion\(.*\)', ('AssemblyVersion("' + $version + '")')) -replace 'AssemblyFileVersion\(.*\)', ('AssemblyFileVersion("' + $version + '")')
        } | Set-Content $_.FullName -Encoding UTF8
    }    
}

function getVersionBase {
    $versionInfo = (Get-Content "version.json") -join "`n" | ConvertFrom-Json
    "$($versionInfo.major).$($versionInfo.minor).$($versionInfo.patch)";    
}