del /F /S /Q  build
msbuild XLabs.sln /property:Configuration=Release;platform=ARM;Targets=Clean
msbuild XLabs.sln /property:Configuration=Release;platform=iPhone;Targets=Clean
msbuild XLabs.sln /property:Configuration=Release;platform="Any CPU";Targets=Clean
msbuild src\Serialization\XLabs.Serialization.AspNet\XLabs.Serialization.AspNet.sln /property:Configuration=Release;platform="Any CPU";Targets=Clean
msbuild XLabs.sln /property:Configuration=Release;platform=ARM
msbuild XLabs.sln /property:Configuration=Release;platform=iPhone
msbuild XLabs.sln /property:Configuration=Release;platform="Any CPU"
msbuild src\Serialization\XLabs.Serialization.AspNet\XLabs.Serialization.AspNet.sln /property:Configuration=Release;platform="Any CPU"