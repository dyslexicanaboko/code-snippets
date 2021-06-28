Clear-Host

#MFP = OK to be missing from prod servers (Dev, QA, UAT, Prod)

$arr = @(
# Version 130
# Installing SQL Server 2016 makes these DLLs show up
 "C:\Program Files (x86)\Microsoft SQL Server\130\SDK\Assemblies\Microsoft.SQLServer.ManagedDTS.dll"
,"C:\Program Files (x86)\Microsoft SQL Server\130\SDK\Assemblies\Microsoft.SqlServer.Dts.Design.dll"
,"C:\Program Files (x86)\Microsoft SQL Server\130\SDK\Assemblies\Microsoft.SqlServer.DTSPipelineWrap.dll"
,"C:\Program Files (x86)\Microsoft SQL Server\130\SDK\Assemblies\Microsoft.SqlServer.DTSRuntimeWrap.dll"
,"C:\Program Files (x86)\Microsoft SQL Server\130\SDK\Assemblies\Microsoft.SqlServer.PipelineHost.dll"
,"C:\Program Files (x86)\Microsoft SQL Server\130\SDK\Assemblies\Microsoft.SqlServer.ServiceBrokerEnum.dll" #MFP
,"C:\Program Files (x86)\Microsoft SQL Server\130\SDK\Assemblies\Microsoft.SqlServer.Smo.dll" #MFP
,"C:\Program Files (x86)\Microsoft SQL Server\130\SDK\Assemblies\Microsoft.SqlServer.SqlEnum.dll" #MFP

# Version 140
#,"C:\Program Files (x86)\Microsoft SQL Server\140\SDK\Assemblies\Microsoft.SQLServer.ManagedDTS.dll"
#,"C:\Program Files (x86)\Microsoft SQL Server\140\SDK\Assemblies\Microsoft.SqlServer.Dts.Design.dll"

# Version 13
,"C:\Windows\assembly\GAC_MSIL\Microsoft.SqlServer.ConnectionInfo\13.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.ConnectionInfo.dll"
,"C:\Windows\assembly\GAC_MSIL\Microsoft.SqlServer.Management.IntegrationServicesEnum\13.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.Management.IntegrationServicesEnum.dll"
,"C:\Windows\assembly\GAC_MSIL\Microsoft.SqlServer.Management.IntegrationServices\13.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.Management.IntegrationServices.dll"
,"C:\Windows\assembly\GAC_MSIL\Microsoft.SqlServer.Management.Sdk.Sfc\13.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.Management.Sdk.Sfc.dll"
,"C:\Windows\assembly\GAC_MSIL\Microsoft.SqlServer.Management.SmoMetadataProvider\13.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.Management.SmoMetadataProvider.dll"
,"C:\Windows\assembly\GAC_MSIL\Microsoft.SqlServer.Smo\13.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.Smo.dll"
,"C:\Windows\assembly\GAC_MSIL\Microsoft.SqlServer.ServiceBrokerEnum\13.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.ServiceBrokerEnum.dll"
,"C:\Windows\assembly\GAC_MSIL\Microsoft.SqlServer.SqlEnum\13.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.SqlEnum.dll"

# Version 14
,"C:\Windows\assembly\GAC_MSIL\Microsoft.SqlServer.Management.IntegrationServicesEnum\14.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.Management.IntegrationServicesEnum.dll"
,"C:\Windows\assembly\GAC_MSIL\Microsoft.SqlServer.Management.IntegrationServices\14.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.Management.IntegrationServices.dll"
,"C:\Windows\assembly\GAC_MSIL\Microsoft.SqlServer.Management.Sdk.Sfc\14.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.Management.Sdk.Sfc.dll"
,"C:\Windows\assembly\GAC_MSIL\Microsoft.SqlServer.Management.SmoMetadataProvider\14.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.Management.SmoMetadataProvider.dll"
,"C:\Windows\assembly\GAC_MSIL\Microsoft.SqlServer.Smo\14.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.Smo.dll"

# Version 14
,"C:\WINDOWS\Microsoft.Net\assembly\GAC_MSIL\Microsoft.SQLServer.ManagedDTS\v4.0_14.0.0.0__89845dcd8080cc91\Microsoft.SQLServer.ManagedDTS.dll"
,"C:\WINDOWS\Microsoft.Net\assembly\GAC_MSIL\Microsoft.SqlServer.Dts.Design\v4.0_14.0.0.0__89845dcd8080cc91\Microsoft.SqlServer.Dts.Design.dll"

# Custom DLLs
)

$arrItems = New-Object -TypeName "System.Collections.ArrayList"

foreach($a in $arr)
{
    $o = @{};

    $o.Path = $a
    $o.Exists = Test-Path $a -PathType Leaf

    $devNull = $arrItems.Add($o);
}

$arrItems | ForEach-Object {[PSCustomObject]$_} | Format-Table Path,Exists -AutoSize