@ECHO off

DEL ..\TestProjects\Nuget_AspNetCoreTargetNetFramework\AspNetCoreTargetNetFramework\Scripts\ServerObjects.* /Q
DEL ..\TestProjects\Nuget_AspNetCoreTargetNetFramework\AspNetCoreTargetNetFramework\Scripts\Home\HomeActions.* /Q

dotnet build ..\TestProjects\Nuget_AspNetCoreTargetNetFramework\AspNetCoreTargetNetFramework\AspNetCoreTargetNetFramework.csproj --force -v n

ECHO Build Complete!

if NOT "%1" == "1" PAUSE