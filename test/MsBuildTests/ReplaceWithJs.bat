@ECHO off

DEL ..\TestProjects\ReplaceWithJsExample\ReplaceWithJsExample\Scripts\ServerObjects.* /Q
DEL ..\TestProjects\ReplaceWithJsExample\ReplaceWithJsExample\Scripts\CustomGroup.* /Q
DEL ..\TestProjects\ReplaceWithJsExample\ReplaceWithJsExample\Scripts\Home\Models.* /Q
DEL ..\TestProjects\ReplaceWithJsExample\ReplaceWithJsExample\Scripts\Home\HomeActions.* /Q

..\nuget install TypeRightDebug -DependencyVersion Highest -SolutionDirectory ..\TestProjects\ReplaceWithJsExample

dotnet build ..\TestProjects\ReplaceWithJsExample\ReplaceWithJsExample\ReplaceWithJsExample.csproj --force -v n

ECHO Build Complete!

if NOT "%1" == "1" PAUSE