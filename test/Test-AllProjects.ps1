
Set-Location .\MsBuildTests

Write-Host "Testing skip Type Right - there should be no files after this one"
.\Test-AspNetCore net6.0 "-p:SkipTypeRight=true"
Read-Host "Press Any Key To continue"
.\Test-AspNetCore net7.0 "-p:SkipTypeRight=true"
Read-Host "Press Any Key To continue"

# Standard Tests
Write-Host "Running standard build in net6.0"
.\Test-AspNetCore net6.0
.\Test-ReplaceWithJs net6.0
Read-Host "Press Any Key To continue"

Write-Host "Running standard build in net7.0"
.\Test-AspNetCore net7.0
.\Test-ReplaceWithJs net7.0
Read-Host "Press Any Key To continue"


Set-Location ..\
