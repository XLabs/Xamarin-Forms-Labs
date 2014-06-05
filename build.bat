@echo Off
SETLOCAL

set EnableNuGetPackageRestore=true 
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Build.proj  /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Detailed /nr:false 

ENDLOCAL
