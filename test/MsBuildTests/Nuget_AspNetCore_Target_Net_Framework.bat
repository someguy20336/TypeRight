@ECHO off

DEL ..\TestProjects\Nuget_AspNetCoreTargetNetFramework\AspNetCoreTargetNetFramework\Scripts\ServerObjects.* /Q
DEL ..\TestProjects\Nuget_AspNetCoreTargetNetFramework\AspNetCoreTargetNetFramework\Scripts\Home\HomeActions.* /Q

nuget install TypeRightDebug -DependencyVersion Highest -SolutionDirectory ..\TestProjects\Nuget_AspNetCoreTargetNetFramework
dotnet build ..\TestProjects\Nuget_AspNetCoreTargetNetFramework\AspNetCoreTargetNetFramework\AspNetCoreTargetNetFramework.csproj --force -v n

ECHO Build Complete!

if NOT "%1" == "1" PAUSE