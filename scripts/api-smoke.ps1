param(
  [string]$Base = "http://localhost:5080"
)

$ErrorActionPreference = 'Stop'

Write-Host "== Token ==" -ForegroundColor Cyan
$login  = @{ userName = "admin"; password = "wsei" } | ConvertTo-Json
$token  = (Invoke-RestMethod "$base/api/security/token" -Method POST -ContentType "application/json" -Body $login).access_token
$H      = @{ Authorization = "Bearer $token" }

Write-Host "`n== Create student ==" -ForegroundColor Cyan
$s = @{ email = "john@uni.edu"; firstName = "John"; lastName = "Doe" } | ConvertTo-Json
Invoke-RestMethod "$base/api/students" -Method POST -Headers $H -ContentType "application/json" -Body $s | Out-Null

Write-Host "`n== Create course ==" -ForegroundColor Cyan
$c = @{ title = "Distributed Systems"; credits = 6 } | ConvertTo-Json
Invoke-RestMethod "$base/api/courses" -Method POST -Headers $H -ContentType "application/json" -Body $c | Out-Null

$students = Invoke-RestMethod "$base/api/students" -Headers $H
$courses  = Invoke-RestMethod "$base/api/courses"  -Headers $H

$sid = [int]($students | Select-Object -Last 1 | Select-Object -ExpandProperty id)
$cid = [int]($courses  | Select-Object -Last 1 | Select-Object -ExpandProperty id)

Write-Host "`nIDs: student=$sid, course=$cid" -ForegroundColor DarkGray

Write-Host "`n== Enroll ==" -ForegroundColor Cyan
$enr = @{ studentId = $sid; courseId = $cid; grade = 5 } | ConvertTo-Json -Compress
try {
  Invoke-RestMethod "$base/api/enrollments" -Method POST -Headers $H -ContentType "application/json" -Body $enr
} catch {
  $resp = $_.Exception.Response
  if ($resp) {
    $reader = New-Object IO.StreamReader($resp.GetResponseStream())
    Write-Host ($reader.ReadToEnd())
  } else { throw }
}

Write-Host "`n== Lists ==" -ForegroundColor Cyan
Invoke-RestMethod "$base/api/students"    -Headers $H
Invoke-RestMethod "$base/api/courses"     -Headers $H
Invoke-RestMethod "$base/api/enrollments" -Headers $H
