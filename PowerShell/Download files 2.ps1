#01/01_D_S_0001.jpg
#05/431_D_S_1348.jpg
[string]$baseURI = "http://c26.cache.pictage.com/servlet/PIS?load=/home/image/proxydb/T/H/TH048/2010/910126/"
[string]$fileName = ""
[string]$uri = ""
$target = "C:\Dump\"
#/02/131_D_S_0442.jpg
#/03/270_D_S_0711.jpg
#/04/207_D_S_0917.jpg
#/05/1_D_S_0918.jpg
$folder = 5 #XX
$prefix = 1 #XX
$pageNo = 918 #XXXX
#200_D_S_0910.jpg
#Create end of URL
#$uri = $baseURI + [string]::Format("{0:00}/{1:00}_D_S_{2:0000}.jpg", $folder, $prefix, $pageNo)

#$uri

$clnt = new-object System.Net.WebClient

for ($i=1; $i -le 510; $i++)
{
	$fileName = [string]::Format("{0:00}_D_S_{1:0000}.jpg", $prefix, $pageNo)
	
	$uri = $baseURI + [string]::Format("{0:00}/{1}", $folder, $fileName)
		
	$uri

	$clnt.DownloadFile($uri, ($target + $fileName))
	
	$prefix++
	$pageNo++
}
