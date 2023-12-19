ECHO OFF

dotnet build "..\src\TypeRight.Attributes\TypeRight.Attributes.csproj" -c Release
dotnet build "..\src\TypeRight.Workspaces.VsixAdapter\TypeRight.Workspaces.VsixAdapter.csproj" -c Release
dotnet publish "..\src\TypeRight.Build\TypeRight.Build.csproj" -c Release

rmdir /S /Q %UserProfile%\.nuget\packages\typerightdebug
rmdir /S /Q .\TestProjects\LocalNugetRepo
rmdir /Q /S nugetpkgs
call nuget pack "..\TypeRightNuspec.nuspec" -OutputDirectory nugetpkgs -properties id=TypeRightDebug;vers=1.0.3

for %%a in (nugetpkgs\*.nupkg) do call nuget add nugetpkgs\TypeRightDebug.1.0.3.nupkg -Source .\TestProjects\LocalNugetRepo