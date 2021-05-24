<Query Kind="Program">
  <NuGetReference>EPPlus</NuGetReference>
  <Namespace>OfficeOpenXml</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

/*
	Creating excel file example. This is not a complete example. I have a better example
	in a different program.
*/
void Main()
{
	//https://github.com/EPPlusSoftware/EPPlus
	var lines = File.ReadAllLines(@"C:\Dump\blah.csv");
	
	CreateExcelWorkbook(@"C:\Dump\blah.xlsx", "DoesNotMatterRightNow", lines);
}

// Define other methods and classes here
public void CreateExcelWorkbook(string saveAs, string divisionName, string[] csvData)
{
	File.Delete(saveAs);
	
	ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

	using (var package = new ExcelPackage(new FileInfo(saveAs)))
	{
		var wb = package.Workbook;
		
		var sheet = wb.Worksheets.Add(divisionName);
		
		for (var r = 0; r < csvData.Length; r++)
		{
			var row = csvData[r];
			
			var cells = row.Split(',');
			
			for (var c = 0; c < cells.Length; c++)
			{
				var cell = cells[c];

				sheet.Cells[r + 1, c + 1].Value = cell;
			}
		}
		
		//This would have to be planned out no matter what
		var t = sheet.Tables.Add(new ExcelAddressBase(1,1,33,13), "Stats");
		
		sheet.Cells[1,1].Formula = "SUM(";
		
		package.Save();
	}
}

