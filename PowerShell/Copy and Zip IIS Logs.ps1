Add-Type -Path "C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.IO.Compression.FileSystem.dll"

[string]$source = "C:\inetpub\logs\LogFiles\W3SVC1\";
[string]$destination = "E:\Dump\A\";
[string]$destinationZipFile = "E:\Dump\IIS_Logs.zip";
[DateTime]$dtm = "2016-08-09 23:59:59";

if( -not (Test-Path -Path $destination))
{
	New-Item -Path $destination -ItemType Directory;
}
#else
#{
#	Remove-Item -Path $destination;
#}

$arr = Get-ChildItem -Path $source | Where-Object {$_.LastWriteTime -gt $dtm}
$target = ""

foreach($s in $arr)
{
	$target = ($destination + $s.Name);
	
	if(-not ([System.IO.File]::Exists($target)))
	{
		[System.IO.File]::Copy(($source + $s.Name), $target, $false);
	}
}

[IO.Compression.ZipFile]::CreateFromDirectory($destination, $destinationZipFile);
