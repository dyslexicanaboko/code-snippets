<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

/*
	Quick way to build out complicated nested folder structures for windows via CMD
	Idea being you need the same nested structure in various root folders for testing
	or any purpose really. This script will make sure the folders don't already exist
	before trying to create them again.
*/
void Main()
{
	var env = EnvironmentStructure();

	GenerateBatchFile(env);
}

public void GenerateBatchFile(Tier root)
{
	var lst = new List<string>();
	
	BuildDirectoryList(lst, root, null);

	//lst.Dump();

	var sb = new StringBuilder();

	sb.Append(@"ECHO OFF

ECHO WARNING: If directories exist already, then errors might be raised, just ignore them.
ECHO          If you don't see anything that's a good thing.
ECHO.").AppendLine().AppendLine();

	foreach (var dir in lst)
	{
		sb.AppendLine($"IF NOT EXIST \"{dir}\" MKDIR \"{dir}\"");	
	}
	
	sb.AppendLine()
	.AppendLine("ECHO Finished")
	.AppendLine()
	.AppendLine("PAUSE");
	
	sb.ToString().Dump();
}

public void BuildDirectoryList(List<string> lst, Tier node, string dir)
{
	if (dir == null)
	{
		dir = node.Name;
	}
	else
	{
		dir = Path.Combine(dir, node.Name);
	}

	lst.Add(dir);
	
	foreach (var child in node.Children)
	{
		BuildDirectoryList(lst, child, dir);
	}
}

public Tier EnvironmentStructure()
{
	var fs = BaseStructure(false);
	var archive = BaseStructure(true);

	var dev = new Tier("Dev");
	dev.Nest(fs.Clone());
	dev.Nest(archive.Clone());

	var qa = new Tier("Qa");
	qa.Nest(fs.Clone());
	qa.Nest(archive.Clone());

	var uat = new Tier("Uat");
	uat.Nest(fs);
	uat.Nest(archive);

	var env = new Tier("Env");

	env.Nest(dev);
	env.Nest(qa);
	env.Nest(uat);
	
	return env;
}

public Tier BaseStructure(bool isArchive)
{
	var ta = new Tier("TopicA");
	ta.Nest(new Tier("import"));

	var staImport = new Tier("import");

	if (isArchive)
	{
		staImport.Nest(new Tier("exported"));
	}
	
	var tb = new Tier("TopicB");
	tb.Nest(staImport);
	tb.Nest(new Tier("export"));

	var taI = new Tier("import");
	var taE = new Tier("export");

	var tc = new Tier("TopicC");
	tc.Nest(taI);
	tc.Nest(taE);
	tc.Nest(new Tier("ProductAImport"));
	tc.Nest(new Tier("ProductBExport"));

	var td = new Tier("TopicD");
	td.Nest(new Tier("import"));
	td.Nest(new Tier("export"));

	var te = new Tier("TopicE");
	te.Nest(new Tier("import"));
	te.Nest(new Tier("export"));

	var root = isArchive ? "Archive" : "Application";

	var app = new Tier(root);
	app.Nest(ta);
	app.Nest(tb);
	app.Nest(ta);
	app.Nest(td);
	app.Nest(te);
	
	return app;
}

public class Tier
{
	public Tier(string name)
	{
		Name = name;
	}

	public string Name { get; set; }

	public Tier Nest(Tier child)
	{
		Children.Add(child);
		
		return this;
	}

	public List<Tier> Children { get; set; } = new List<Tier>();

	public Tier Clone()
	{
		// Using a cheap clone method so I don't have to break my brain over this
		var t = CloneJson(this);
		
		return t;
	}
}

// CREDIT: https://stackoverflow.com/a/78612/603807
/// <summary>
/// Perform a deep Copy of the object, using Json as a serialisation method. NOTE: Private members are not cloned using this method.
/// </summary>
/// <typeparam name="T">The type of object being copied.</typeparam>
/// <param name="source">The object instance to copy.</param>
/// <returns>The copied object.</returns>
public static T CloneJson<T>(T source)
{
	// Don't serialize a null object, simply return the default for that object
	if (Object.ReferenceEquals(source, null))
	{
		return default(T);
	}

	// initialize inner objects individually
	// for example in default constructor some list property initialized with some values,
	// but in 'source' these items are cleaned -
	// without ObjectCreationHandling.Replace default constructor values will be added to result
	var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

	return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
}