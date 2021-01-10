ECHO OFF
call CreateDebugPackage

cd /d ".\MsBuildTests"
for %%a in (*.bat) do call "%%a" 1

Pause