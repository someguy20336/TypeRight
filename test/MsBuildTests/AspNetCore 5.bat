@ECHO off

DEL ..\TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\ServerObjects.* /Q
DEL ..\TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\CustomGroup.* /Q
DEL ..\TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\Home\Models.* /Q
DEL ..\TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\Home\HomeActions.* /Q

..\nuget install TypeRightDebug -DependencyVersion Highest -SolutionDirectory ..\TestProjects\AspNetCore

dotnet build ..\TestProjects\AspNetCore\TestAspNetCoreApp\TestAspNetCoreApp.csproj --force -v n

ECHO Build Complete!

if NOT "%1" == "1" PAUSE