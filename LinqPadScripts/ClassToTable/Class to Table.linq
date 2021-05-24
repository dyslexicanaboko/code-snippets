<Query Kind="Program">
  <Namespace>Microsoft.CSharp</Namespace>
  <Namespace>System.CodeDom</Namespace>
</Query>

string BASE_PATH;

//Extract from simple class creator, for quick prototyping
void Main()
{
	BASE_PATH = Path.GetDirectoryName(Util.CurrentQueryPath);

	LoadFromFile();

//	ClassToTable(new Target() 
//	{
//		DllFullPath = @"C:\PathToDll\Library1.dll",
//		FqdnOfClass = "FQDNOfTargetClass",
//		OnlyReflectPropertiesWithAttributes = false
//	});
}

public void LoadFromFile()
{
	var targets = LoadClassFile(Path.Combine(BASE_PATH, "Classes.txt"));

	targets.ForEach(ClassToTable);
}

public List<Target> LoadClassFile(string fullFilePath)
{
	var lst = File.ReadAllLines(fullFilePath)
	.Select(x =>
	{
		var arr = x.Split('|');

		var t = new Target()
		{
			DllFullPath = arr[0],
			FqdnOfClass = arr[1],
			FriendlyTitle = arr[2]
		};
		
		return t;
	})
	.ToList();
	
	return lst;
}

public void ClassToTable(Target target)
{
	var dto = new DtoGenerator(target.DllFullPath);
	
	var asm = dto.GetClassProperties(target.FqdnOfClass, target.OnlyReflectPropertiesWithAttributes);

	Console.WriteLine($"{target.FriendlyTitle} = {asm.Classes.First().FullName}");
	
	asm.Classes[0].Properties.Dump();
}

public class DtoGenerator
{
	public string AssemblyPath { get; set; }
	public string FileName { get; set; }
	public string BasePath { get; private set; }
	public Assembly AssemblyReference { get; private set; }
	private CSharpCodeProvider _compiler;

	public DtoGenerator(string assemblyPath)
	{
		AssemblyPath = assemblyPath;
		BasePath = Path.GetDirectoryName(assemblyPath);
		FileName = Path.GetFileName(assemblyPath);
		AssemblyReference = Assembly.LoadFile(AssemblyPath);
		_compiler = new CSharpCodeProvider();
	}

	public Type PrintClassName(string className)
	{
		Type t = AssemblyReference.GetType(className, false, false);

		if (t != null)
		{
			//Console.WriteLine(t.FullName);
		}
		else
			Console.WriteLine("The Class Named: [" + className + "] could not be found (null returned).");

		return t;
	}

	public AssemblyInfo GetClassProperties(string className, bool onlyReflectPropertiesWithAttributes)
	{
		AssemblyInfo asm = new AssemblyInfo();

		asm.Name = FileName;

		Type t = PrintClassName(className);

		if (t == null)
			return asm;

		ClassInfo cInfo = asm.AddClass(className);

		foreach (System.Reflection.PropertyInfo pi in t.GetProperties())
		{
			//Console.WriteLine(pi.Name);
			if(onlyReflectPropertiesWithAttributes && !pi.CustomAttributes.Any()) continue;

			var p = new PropertyInfo
			{
				Name = pi.Name,
				Type = pi.PropertyType,
				IsSerializable = pi.PropertyType.IsDefined(typeof(SerializableAttribute), false)
			};

			SetIsNullable(p);
			
			SetTypeName(p);

			cInfo.Properties.Add(p);
		}

		return asm;
	}

	public string MakeDTO(string className)
	{
		Type t = PrintClassName(className);

		if (t == null)
			return "Type cannot be null";

		StringBuilder sbC = new StringBuilder();
		StringBuilder sbT = new StringBuilder();

		string strClassName = t.Name + "DTO";

		sbC.AppendLine("[DataContract]");
		sbC.Append("public class ").AppendLine(strClassName);
		sbC.AppendLine("{");

		sbT.Append("public ").Append(strClassName).Append(" Translate(").Append(t.Name).AppendLine(" obj)");
		sbT.AppendLine("{");
		sbT.Append(strClassName).Append(" dto = new ").Append(strClassName).AppendLine("();");
		sbT.AppendLine();

		Type tp;

		foreach (System.Reflection.PropertyInfo pi in t.GetProperties())
		{
			Console.WriteLine(pi.Name);

			tp = pi.PropertyType;

			sbC.AppendLine("[DataMember]");
			sbC.Append("public ").Append(GetTypeAsString(tp));

			//TODO: This is totally optional. Convert "System.Nullable<T>" to "T?" Where T is a generic type

			sbC.Append(" ").Append(pi.Name).AppendLine(" { get; set; } ");

			sbT.Append("dto.").Append(pi.Name).Append(" = obj.").Append(pi.Name).AppendLine(";");
		}

		sbC.AppendLine();

		sbT.AppendLine("return dto;");
		sbT.AppendLine("}");

		sbC.Append(sbT);
		sbC.AppendLine("}");

		//using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Path.Combine(BasePath, t.Name + ".cs"), false))
		//{
		//    sw.Write(sbC.ToString());
		//}

		return sbC.ToString();
	}

	public void SetIsNullable(PropertyInfo extractedProperty)
	{
		var p = extractedProperty;

		var type = p.Type;

		if (type.IsGenericType &&
			type.GetGenericTypeDefinition() == typeof(Nullable<>))
		{
			var actualType = type.GetGenericArguments()[0];
			
			p.IsNullable = true;
			
			p.Type = actualType;
		}
	}

	public void SetTypeName(PropertyInfo extractedProperty)
	{
		var p = extractedProperty;
				
		p.TypeName = GetTypeAsString(p.Type);
	}

	public string GetTypeAsString(Type target)
	{
		return _compiler.GetTypeOutput(new CodeTypeReference(target));
	}

	public class AssemblyInfo
	{
		public AssemblyInfo()
		{
			Classes = new List<ClassInfo>();
		}

		public string Name { get; set; }

		public List<ClassInfo> Classes { get; set; }

		public ClassInfo AddClass(string fullyQualifiedClassName)
		{
			ClassInfo info = new ClassInfo { FullName = fullyQualifiedClassName };

			Classes.Add(info);

			return info;
		}

		public void AddClass(ClassInfo info)
		{
			Classes.Add(info);
		}
	}

	public class ClassInfo
	{
		public ClassInfo()
		{
			Properties = new List<PropertyInfo>();
		}

		public string FullName { get; set; }

		public string Name { get; set; }

		public string Namespace { get; set; }

		public List<PropertyInfo> Properties { get; set; }
	}

	public class PropertyInfo
	{
		public string Name { get; set; }
		public Type Type { get; set; }
		public string TypeName { get; set; }
		public bool IsNullable { get; set; }
		public bool IsSerializable { get; set; }
		public bool IsCollection { get; set; }

		public override string ToString()
		{
			return Name + " " + TypeName;
		}
	}
}

public class Target
{
	public string DllFullPath { get; set; }

	public string FqdnOfClass { get; set; }
	
	public string FriendlyTitle { get; set; }
	
	public bool OnlyReflectPropertiesWithAttributes { get; set; } = true;
}