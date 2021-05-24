#http://delivery.qmags.com/d/?pub=IEEESM&upid=12651&fl=others%2fIEEESM%2fSPEC_20070701_Jul_2007.pdf
#http://delivery.qmags.com/d/?pub=IEEESM&upid=12651&fl=others%2fIEEESM%2fSPEC_20070801_Aug_2007.pdf
#http://delivery.qmags.com/d/?pub=IEEESM&upid=12651&fl=others%2fIEEESM%2fSPEC_20071001_Oct_2007.pdf
#Base URL      : http://delivery.qmags.com/d/?pub=IEEESM&upid=12651&fl=others%2fIEEESM%2fSPEC_
#Change Part Ex: 20071001_Oct_2007.pdf 
#Change Part   : [yyyy][mm]01_[MMM]_yyyy.pdf 

[string]$baseURI = "http://delivery.qmags.com/d/?pub=IEEESM&upid=12651&fl=others%2fIEEESM%2fSPEC_"
[string]$fileName = ""
[string]$uri = ""
$target = "C:\Dump\"
$year = 2007
$month = 10
$dtmBase = New-Object System.DateTime($year, $month, 01)
$dtmEnd = New-Object System.DateTime(2011, 11, 01)
$clnt = new-object System.Net.WebClient

while ($dtmBase -le $dtmEnd)
{
	#$fileName = [string]::Format("{0:00}_D_S_{1:0000}.jpg", $prefix, $pageNo)
	#                                                         [yyyy][MM]01_[MMM]_yyyy.pdf
	$fileName = [string]::Format("{0}.pdf", $dtmBase.ToString("yyyyMM01_MMM_yyyy"))
	
	$uri = $baseURI + $fileName
		
	$uri
	
	$clnt.DownloadString($uri).Substring(0, 0)

	$clnt.DownloadFile($uri, ($target + $fileName))
	
	$dtmBase = $dtmBase.AddMonths(1);
}
