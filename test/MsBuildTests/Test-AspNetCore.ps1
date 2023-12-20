$tfm = $args[0]
$addlArgs = $args[1]

Remove-Item ..\TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\ServerObjects.*
Remove-Item ..\TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\CustomGroup.*
Remove-Item ..\TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\Home\Models.*
Remove-Item ..\TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\Home\HomeActions.*

Remove-Item $ENV:userprofile\.nuget\packages\typerightdebug -Recurse
..\nuget install TypeRightDebug -DependencyVersion Highest -SolutionDirectory ..\TestProjects\AspNetCore

dotnet build ..\TestProjects\AspNetCore\TestAspNetCoreApp\TestAspNetCoreApp.csproj --disable-build-servers --force -v n -f $tfm $addlArgs

Write-Host "Build Complete!"