@ECHO off

DEL ..\TestProjects\Nuget_AspNetCore\TestAspNetCoreApp\Scripts\ServerObjects.* /Q
DEL ..\TestProjects\Nuget_AspNetCore\TestAspNetCoreApp\Scripts\CustomGroup.* /Q
DEL ..\TestProjects\Nuget_AspNetCore\TestAspNetCoreApp\Scripts\Home\Models.* /Q
DEL ..\TestProjects\Nuget_AspNetCore\TestAspNetCoreApp\Scripts\Home\HomeActions.* /Q

dotnet build ..\TestProjects\Nuget_AspNetCore\TestAspNetCoreApp\TestAspNetCoreApp.csproj --force -v n

ECHO Build Complete!

if NOT "%1" == "1" PAUSE