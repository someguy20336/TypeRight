$tfm = $args[0]

Remove-Item ..\TestProjects\ReplaceWithJsExample\ReplaceWithJsExample\Scripts\ServerObjects.*
Remove-Item ..\TestProjects\ReplaceWithJsExample\ReplaceWithJsExample\Scripts\CustomGroup.*
Remove-Item ..\TestProjects\ReplaceWithJsExample\ReplaceWithJsExample\Scripts\Home\Models.*
Remove-Item ..\TestProjects\ReplaceWithJsExample\ReplaceWithJsExample\Scripts\Home\HomeActions.*

Remove-Item $ENV:userprofile\.nuget\packages\typerightdebug -Recurse
..\nuget install TypeRightDebug -DependencyVersion Highest -SolutionDirectory ..\TestProjects\ReplaceWithJsExample

dotnet build ..\TestProjects\ReplaceWithJsExample\ReplaceWithJsExample\ReplaceWithJsExample.csproj --force -v n -f $tfm

Write-Host Build Complete!