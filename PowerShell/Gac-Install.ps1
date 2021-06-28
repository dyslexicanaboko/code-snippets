#https://superuser.com/questions/749243/detect-if-powershell-is-running-as-administrator
$isAdmin = ([Security.Principal.WindowsPrincipal] `
  [Security.Principal.WindowsIdentity]::GetCurrent() `
).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if($isAdmin -eq $false)
{
    Write-Host "You have to run this script as Administrator or it just won't work!" -ForegroundColor Red

    return;
}

#$strDllPath = "G:\FS.ETL\Hca.Ssis.Package.Dlls\"
$strDllPath = "C:\Dev\Hca.Ssis.Package.Dlls\"

#Note that you should be running PowerShell as an Administrator
[System.Reflection.Assembly]::Load("System.EnterpriseServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")            
$publish = New-Object System.EnterpriseServices.Internal.Publish            


$arr = @(
    "DLL1.dll",
    "DLL2.dll",
    "DLL3.dll",
    "Newtonsoft.Json.dll",
    "Renci.SshNet.dll"
)

$timeStamp = Get-Date -Format "dddd MM/dd/yyyy HH:mm:ss"

write-host "`n"
write-host "======================================================`n"
Write-Host ("Last installed on:" + $timeStamp) -ForegroundColor Magenta
write-host "======================================================`n"

foreach($d in $arr)
{
    $p = ($strDllPath + $d);

    $p
    
    $publish.GacInstall($p);
}

#If installing into the GAC on a server hosting web applications in IIS, you need to restart IIS for the applications to pick up the change.
#Uncomment the next line if necessary...
#iisreset

#The content below is only for development purposes - not required at all on the server when deploying
write-host "`n"
Write-Host 'Copying "DLL" to DTS Tasks folder for development purposes' -ForegroundColor Magenta
Write-Host 'If you get a "Sharing violation" error - close all visual studio instances and try again.' -ForegroundColor Green
write-host "`n"

$customTask = "C:\Dev\Hca.Ssis.Package.Dlls\Hca.SSIS.Component.ExecuteCatalogPackageTask.dll";

# Depending on the SQL Server version you are using, the destination folder will change
$arrDestinations = @(
    "130",
    "140"
)

foreach($d in $arrDestinations)
{
    $destination = "C:\Program Files (x86)\Microsoft SQL Server\" + $d + "\DTS\Tasks\";

    Write-Host "Copied custom task to -> " + $destination

    Copy-Item $customTask -Destination $destination
}

Write-Host "`n`nEnsure that the list below shows that that the DLLs are in the GAC" -ForegroundColor Red

$arrCheck = @(
# Check if those same DLLs installed above are found here
)

$arrItems = New-Object -TypeName "System.Collections.ArrayList"

foreach($a in $arrCheck)
{
    $o = @{};

    $o.Path = $a
    $o.Exists = Test-Path $a -PathType Leaf

    $devNull = $arrItems.Add($o);
}

$arrItems | ForEach-Object {[PSCustomObject]$_} | Format-Table Path,Exists -AutoSize