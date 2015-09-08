del /F /S /Q  build
msbuild XLabs.sln /property:Configuration=Release;platform=ARM
msbuild XLabs.sln /property:Configuration=Release;platform=iPhone
msbuild XLabs.sln /property:Configuration=Release;platform="Any CPU"