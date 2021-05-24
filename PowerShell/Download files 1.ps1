#Download files in serial that follow a URL pattern
[string]$baseURI = "http://www.url.com/dir/571767/Big_571767_"
[string]$fileName = ""
[string]$uri = ""
[string]$target = "C:\Dump\"

$clnt = new-object System.Net.WebClient

for ($i=1; $i -le 1000; $i++)
{
	$fileName = [string]::Format("{0:0000}.jpg", $i)
	
	$uri = ($baseURI + $fileName)
		
	$uri

	$clnt.DownloadFile($uri, ($target + $fileName))
}