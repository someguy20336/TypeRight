@ECHO off

DEL ..\TestProjects\AspNetCoreTargetNetFramework\AspNetCoreTargetNetFramework\Scripts\ServerObjects.* /Q
DEL ..\TestProjects\AspNetCoreTargetNetFramework\AspNetCoreTargetNetFramework\Scripts\Home\HomeActions.* /Q

..\nuget install TypeRightDebug -DependencyVersion Highest -SolutionDirectory ..\TestProjects\AspNetCoreTargetNetFramework
dotnet build ..\TestProjects\AspNetCoreTargetNetFramework\AspNetCoreTargetNetFramework\AspNetCoreTargetNetFramework.csproj --force -v n -p:SkipTypeRight=true

ECHO Build Complete!

if NOT "%1" == "1" PAUSE