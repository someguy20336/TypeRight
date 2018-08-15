@ECHO off

REM Set up visual studio variables *DJW With a copy of VS 2015 only, this doesn't work
REM if "%FrameworkDir%"=="" call SetVSVars.cmd

SET VisualStudioVersion=14.0
SET MsBuildDir=%ProgramFiles(x86)%\MSBuild\%VisualStudioVersion%\Bin
SET MsBuildDir=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin

"%MsBuildDir%\msbuild" master.proj /property:VisualStudioVersion=%VisualStudioVersion% /target:Build /fileLogger

ECHO Build Complete!
PAUSE