$isAdmin = ([Security.Principal.WindowsPrincipal] `
  [Security.Principal.WindowsIdentity]::GetCurrent() `
).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if($isAdmin -eq $false)
{
    Write-Host "You have to run this script as Administrator or it just won't work!" -ForegroundColor Red

    return;
}

$strDllPath = "C:\Dev\YourDllLocation\"

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
Write-Host ("Last uninstalled on: " + $timeStamp) -ForegroundColor Yellow
write-host "======================================================`n"
write-host "`n"

foreach($d in $arr)
{
    $p = ($strDllPath + $d);

    $p
    
	#https://docs.microsoft.com/en-us/dotnet/api/system.enterpriseservices.internal.publish.gacremove?view=netframework-4.7.2
    $publish.GacRemove($p);
}

#If installing into the GAC on a server hosting web applications in IIS, you need to restart IIS for the applications to pick up the change.
#Uncomment the next line if necessary...
#iisreset


#Sometimes the DLLs won't be uninstalled for various reasons, including but not limited to - having Visual Studio open.
Write-Host "Sometimes the DLLs won't be uninstalled for various reasons, including but not limited to - having Visual Studio open." -BackgroundColor Yellow -ForegroundColor Black
Write-Host "Ensure that the list below shows that none of the files are found!" -ForegroundColor Red

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