#This is for test renaming a list of files
ls "*.pdf" -Force -Name | % { New-Object System.String($_).Replace(".pdf", ".htm") }

#This is for renaming a list of files
ls "*.pdf" -Force -Name | % { Rename-Item $_ New-Object System.String($_).Replace(".pdf", ".htm") }

#example found here: http://halr9000.com/article/426
   1: $path = "\\server\share\folder"
   2: dir $path -Recurse -Include *_ | % {
   3:   Rename-Item $_ $_.name.Substring(0,$_.name.length-1)
   4: }