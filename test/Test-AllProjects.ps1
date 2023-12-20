
Set-Location .\MsBuildTests

Write-Host "Testing skip Type Right - there should be no files after this one"
.\Test-AspNetCore net8.0 "-p:SkipTypeRight=true"
Write-Host "Check that files are still deleted and were not regenerated"
Read-Host "Press Any Key To continue"

# Standard Tests
Write-Host "Running standard build in net6.0"
.\Test-AspNetCore net6.0
.\Test-ReplaceWithJs net6.0
Write-Host "There should be no changes visible in source control and no errors from the build"
Read-Host "Press Any Key To continue"

Write-Host "Running standard build in net8.0"
.\Test-AspNetCore net8.0
.\Test-ReplaceWithJs net8.0
Write-Host "There should be no changes visible in source control and no errors from the build"


Set-Location ..\
