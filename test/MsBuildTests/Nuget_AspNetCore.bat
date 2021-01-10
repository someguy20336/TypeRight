@ECHO off

REM Set up visual studio variables *DJW With a copy of VS 2015 only, this doesn't work
REM if "%FrameworkDir%"=="" call SetVSVars.cmd

dotnet build ..\TestProjects\Nuget_AspNetCore\TestAspNetCoreApp\TestAspNetCoreApp.csproj --force -v n

ECHO Build Complete!
PAUSE