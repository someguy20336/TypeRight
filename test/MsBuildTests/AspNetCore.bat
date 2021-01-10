@ECHO off

DEL ..\TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\ServerObjects.* /Q
DEL ..\TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\CustomGroup.* /Q
DEL ..\TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\Home\Models.* /Q
DEL ..\TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\Home\HomeActions.* /Q

dotnet build ..\TestProjects\AspNetCore\TestProject.sln --force -v n

ECHO Build Complete!

if NOT "%1" == "1" PAUSE