<Query Kind="Program" />

void Main()
{
	//var classes = GetTypesInEditor()
	//	.Where(x =>
	//		x == typeof(TargetClass1) ||
	//		x == typeof(TargetClass2) ||
	//		x == typeof(TargetClass3))
	//	.OrderByDescending(x => x.GetProperties().Length)
	//	.ToList();
	
	//Provide a list of types to produce a matrix of class and propries
	var arr = GetClassUnits(new List<Type>());
	
	//arr.Dump();
	
	var sortedArr = ColumnTiering(arr);
	
	sortedArr.Dump();
}

private string[,] ColumnTiering(List<ClassUnit> classUnits)
{
	var lstTokens = classUnits
		.SelectMany(x => x.Properties)
		.Distinct()
		.OrderBy(x => x)
		.ToList();
	
	var lstClassUnitsDistributed = classUnits.Select(x => new ClassUnit { ClassName = x.ClassName }).ToList();

	//Iterate through the tokens and distribute accordingly
	foreach (var token in lstTokens)
	{
		//For all properties in every class
		for (var i = 0; i < lstClassUnitsDistributed.Count; i++)
		{
			var properties = classUnits[i].Properties;
			var cuDist = lstClassUnitsDistributed[i];
			
			//Provide a spacer where a class does not contain a property
			var entry = properties.Contains(token) ? token : "--";
			
			cuDist.Properties.Add(entry);
		}
	}

	//Change back to 2D array for display purposes
	var newArr = GetClassPropertyMatrix(lstClassUnitsDistributed);

	return newArr;
}

#region First attempt that partially worked
/*
private string[,] ColumnMatchingViaLinkedList(string[,] propertiesByClass)
{
	var arr = propertiesByClass;
	
	var columns = arr.GetLength(1);
	var rows = arr.GetLength(0);
	
	var lstClassUnits = new List<ClassUnitLl>(columns);
	
	//Convert to class unit
	for (int c = 0; c < columns; c++)
	{
		var cls = new ClassUnitLl();
		cls.ClassName = arr[0, c];
		cls.Properties = new LinkedList<string>();

		for (int r = 1; r < rows; r++)
		{
			cls.Properties.AddLast(arr[r, c]);
		}
		
		lstClassUnits.Add(cls);
	}
	
	var main = lstClassUnits.First();
	
	var otherClasses = lstClassUnits.Skip(1);
	
	LinkedListNode<string> lastKnown = null;
	
	//Compare each class to one another
	foreach (var cu in otherClasses)
	{
		//For all properties in the largest class (main class)
		foreach (var p in main.Properties)
		{
			var node = cu.Properties.Find(p);
			
			if (node != null)
			{
				lastKnown = node;
				
				continue;
			}

			if (lastKnown == null)
			{
				cu.Properties.AddFirst("--");
			}
			else
			{
				cu.Properties.AddAfter(lastKnown, "--");
			}
		}
		
		lastKnown = null;
	}
	
	var newArr = GetClassPropertyMatrix(lstClassUnits);
	
	return newArr;
}

public class ClassUnitLl
{
	public string ClassName { get; set; }

	public LinkedList<string> Properties { get; set; }
}
*/
#endregion

private List<ClassUnit> GetClassUnits(List<Type> classes)
{
	var maxProperties = classes.Max(x => x.GetProperties().Length);

	var lst = new List<ClassUnit>(classes.Count);

	for (int c = 0; c < classes.Count; c++)
	{
		var cls = classes[c];

		var cu = new ClassUnit { ClassName = cls.Name }; 

		cu.Properties = cls
			.GetProperties()
			.Select(x => x.Name)
			.OrderBy(x => x)
			.ToList();
			
		lst.Add(cu);
	}
	
	return lst;
}

private string[,] GetClassPropertyMatrix(List<ClassUnit> classUnits)
{
	var maxProperties = classUnits.Max(x => x.Properties.Count) + 1;

	var arr = new string[maxProperties, classUnits.Count]; //Room for header

	for (int c = 0; c < classUnits.Count; c++)
	{
		var cls = classUnits[c];

		arr[0, c] = cls.ClassName;

		var r = 1;

		foreach (var p in cls.Properties)
		{
			arr[r, c] = p;
			
			r++;
		}
	}

	return arr;
}

//Get all types defined in this editor except the linqpad stock types
private List<Type> GetTypesInEditor()
{
	var t = UserQuery.QueryInstance.GetType();

	var classes = t
		.Assembly
		.GetTypes()
		.Where(x =>
			x != typeof(LINQPadJsonNodeExtensions) &&
			x != typeof(UserQuery)
		).ToList();
		
	return classes;
}

public class ClassUnit
{ 
	public string ClassName { get; set; }
	
	public List<string> Properties { get; set; } = new List<string>();
}
