<Query Kind="Program" />

void Main()
{
	//Known issue: European names that have multiple parts to the last name are not split correctly
	//Example: "Ana de Armas" or "Ana de la Reguera"
	var arr = new[] {
		"First Last",
		"First Middle Last"
	};

	//arr.Dump();

	var lst = new List<Person>(arr.Length);

	foreach (var a in arr)
	{
		var t = a.Split(' ');
		
		var p = new Person();
		
		p.First = t[0];

		if (t.Length == 2)
		{
			p.Last = t[1];
		}
		else
		{
			p.Middle = t[1];
			p.Last = t[2];
		}
		
		lst.Add(p);
	}

	foreach (var p in lst)
	{
		var m = p.Middle == null ? "NULL" : "N'" + p.Middle + "'";

		Console.WriteLine($"EXEC dbo.AddPerson @firstName = N'{p.First}', @middleName = {m}, @lastName = N'{p.Last}', @genderId = 0, @userId = 1;");
	}
}

public class Person
{
	public string First { get; set; }
	public string Middle { get; set; }
	public string Last { get; set; }
}