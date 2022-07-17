<Query Kind="Program">
  <NuGetReference>EPPlus</NuGetReference>
  <Namespace>OfficeOpenXml.Table</Namespace>
  <Namespace>OfficeOpenXml</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	StaticExample();
}

//This is an inflexible example where the schema is known and essentially hardcoded
//I want to create a way to do this dynamically
public void StaticExample()
{
	object[,] rowData =
	{
	   //Excel
	   //1  2    3             4  5  6   7
	   //Index
	   //0  1    2             3  4  5   6
		{0, "A", "07/01/2022", 0, 0, 10, 100},
		{1, "B", "07/02/2022", 1, 1, 20, 100},
		{2, "C", "07/03/2022", 2, 2, 30, 100},
		{3, "D", "07/04/2022", 3, 3, 40, 100},
		{4, "E", "07/05/2022", 4, 4, 50, 100}
	};

	var rd = new ReportData
	{
		Headers = new List<string>
		{
					   //I E
			"Integer", //0 1
			"String",  //1 2
			"DateTime",//2 3
			"Dollars", //3 4
			"Percent", //4 5
			"Part",    //5 6
			"Whole",   //6 7
			"Delta %"  //7 8 Dynamically created and calculated - not part of Row Data
		},
		RowData = rowData,
		SheetName = "Example"
	};

	SaveAsExcel(rd, @"C:\Dump\Example.xlsx");
	
	Console.WriteLine($"Finished @ {DateTime.Now}");
}

private void WriteTableHeader(ExcelWorksheet sheet, IList<string> headers)
{
	for (var c = 0; c < headers.Count; c++)
	{
		sheet.Cells[1, c + 1].Value = headers[c];
	}
}

private ExcelRange Cell(ExcelWorksheet sheet, int row, int column, object value = null)
{
	var c = sheet.Cells[row, column];

	if (value == null) return c;

	c.Value = value;

	return c;
}

//https://github.com/pruiz/EPPlus/blob/master/EPPlus/Style/ExcelNumberFormat.cs
private ExcelRange CellNumberFormat(ExcelRange cell, string format)
{
	cell.Style.Numberformat.Format = format;

	return cell;
}

private ExcelRange CellAsAccounting(ExcelWorksheet sheet, int row, int column, object value = null)
{
	var c = Cell(sheet, row, column, value);

	//https://stackoverflow.com/a/50308895/603807
	c = CellNumberFormat(c, "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-");

	return c;
}

private ExcelRange CellAsNumber(ExcelWorksheet sheet, int row, int column, object value = null)
{
	var c = Cell(sheet, row, column, value);

	c = CellNumberFormat(c, "#,##0");

	return c;
}

private ExcelRange CellAsPercent(ExcelWorksheet sheet, int row, int column, object value = null)
{
	var c = Cell(sheet, row, column, value);

	c = CellNumberFormat(c, "0.00%");

	return c;
}

private ExcelRange CellAsDate(ExcelWorksheet sheet, int row, int column, object value = null)
{
	var c = Cell(sheet, row, column, value);

	c = CellNumberFormat(c, "mm-dd-yy");

	return c;
}

private void WriteTableBody(ExcelWorksheet sheet, object[,] rowData)
{
	//Index value
	for (var r = 0; r < rowData.GetLength(0); r++)
	{
		//Offsetting:
		//  +1 for Excel row
		//  +1 for skipping Header row
		var r0 = r + 2;

		//E 1
		CellAsNumber(sheet, r0, 1, rowData[r, 0]);
		//E 2
		Cell(sheet, r0, 2, rowData[r, 1]);
		//E 3
		CellAsDate(sheet, r0, 3, rowData[r, 2]);
		//E 4
		CellAsAccounting(sheet, r0, 4, rowData[r, 3]);
		//E 5
		CellAsPercent(sheet, r0, 5, rowData[r, 4]);
		//E 6
		var c6 = CellAsNumber(sheet, r0, 6, rowData[r, 5]);
		//E 7
		var c7 = CellAsNumber(sheet, r0, 7, rowData[r, 6]);

		//E 8
		//Calculated column that references two other columns
		var c8 = CellAsPercent(sheet, r0, 8);
		c8.Formula = SafeDivideFormula(c6, c7);
	}
}

//This doesn't work perfectly
//TODO: Find the largest piece of data per column and size it according to that cell.
private void AutoSizeColumns(ExcelTable table, int row, int columnStart)
{
	var sheet = table.WorkSheet;

	for (var column = columnStart; column <= table.Columns.Count; column++)
	{
		var c = Cell(sheet, row, column);

		c.AutoFitColumns();
	}
}

private string SafeDivideFormula(ExcelRange numerator, ExcelRange denominator)
{
	var n = numerator.Address;
	var d = denominator.Address;

	var f = $"IF({d}=0, 0, {n}/{d})";

	return f;
}

private ExcelRange SumVerticalCellRange(ExcelWorksheet sheet, int resultRow, int resultColumn, int tableFirstRow)
{
	var tableLastRow = resultRow - 1;

	var c = resultColumn;

	var cellStart = Cell(sheet, tableFirstRow, c);
	var cellEnd = Cell(sheet, tableLastRow, c);

	var cellSum = CellAsNumber(sheet, resultRow, c);
	cellSum.Formula = $"SUM({cellStart.Address}:{cellEnd.Address})";

	return cellSum;
}

private ExcelRange WriteKeyValue(ExcelWorksheet sheet, int row, int column, string key, object value)
{
	var cellKey = Cell(sheet, row, column, key);
	cellKey.Style.Font.Bold = true;

	var c = Cell(sheet, row, column + 1, value);

	return c;
}

private void SaveAsExcel(ReportData reportData, string saveAs)
{
	var d = reportData.RowData;
	var columns = reportData.Headers.Count; //Header count will cover calculated and row data
	var rowTableFirst = 2; //Header is always 1
	var rowTableLast = d.GetLength(0) + 1;
	var rowFormula = rowTableLast + 1; //Right underneath table
	var rowSummary = rowFormula + 2; //Spacing away from formula row

	//TODO: For now delete the file before continuing. Eventually needs to be re-used
	File.Delete(saveAs);

	ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

	using (var package = new ExcelPackage(new FileInfo(saveAs)))
	{
		var wb = package.Workbook;

		var sheet = wb.Worksheets.Add(reportData.SheetName);

		WriteTableHeader(sheet, reportData.Headers);

		WriteTableBody(sheet, d);

		//Change range into table
		var t = sheet.Tables.Add(new ExcelAddressBase(1, 1, rowTableLast, columns), "ExampleTable");

		t.TableStyle = TableStyles.Medium16;

		AutoSizeColumns(t, 1, 1);

		//Example of getting the sum of all cells in a column and placing them in a formula row
		//This needs work because there is a way to make it part of the actual table
		SumVerticalCellRange(sheet, rowFormula, 1, rowTableFirst);

		//Example of writing key / value pairs after a table is produced
		WriteKeyValue(sheet, rowSummary + 1, 1, "Columns", columns);
		WriteKeyValue(sheet, rowSummary + 2, 1, "Rows", d.GetLength(0));
		
		package.Save();
	}
}

public class ReportData
{
	public List<string> Headers { get; set; }

	public object[,] RowData { get; set; }

	public string SheetName { get; set; }
}