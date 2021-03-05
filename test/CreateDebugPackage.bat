ECHO OFF

dotnet build "..\src\TypeRight.Attributes\TypeRight.Attributes.csproj" -c Release
dotnet build "..\src\TypeRight.Workspaces.VsixAdapter\TypeRight.Workspaces.VsixAdapter.csproj" -c Release
dotnet publish "..\src\TypeRight\TypeRight.csproj" /p:DeployOnBuild=true /p:PublishProfile=Release

rmdir /Q /S nugetpkgs
call nuget pack "..\DebugTypeRightNuspec.nuspec" -OutputDirectory nugetpkgs

for %%a in (nugetpkgs\*.nupkg) do call nuget add nugetpkgs\TypeRightDebug.1.0.3.nupkg -Source .\TestProjects\LocalNugetRepo