<Query Kind="Program">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.Emit</Namespace>
</Query>

void Main()
{
	// define source code, then parse it (to the type used for compilation)
	SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(@"
                using System;

                namespace RoslynCompileSample
                {
                    public class Writer
                    {
                        public void Write(string message)
                        {
                            Console.WriteLine(message);
                        }
                    }
                }");

	// define other necessary objects for compilation
	var objectAssemblyPath = typeof(object).Assembly.Location;

	//Get the current path of where the "Object" class was loaded from. This is how other DLLs of the same version can be loaded.
	var assemblyPath = Path.GetDirectoryName(objectAssemblyPath);

	MetadataReference[] references = {
		MetadataReference.CreateFromFile(objectAssemblyPath),
		MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
		MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Console.dll")),
		MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll"))
	};
	
	// define other necessary objects for compilation
	string assemblyName = Path.GetRandomFileName();
	
	// analyse and generate IL code from syntax tree
	CSharpCompilation compilation = CSharpCompilation.Create(
		assemblyName,
		syntaxTrees: new[] { syntaxTree },
		references: references,
		options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

	using (var ms = new MemoryStream())
	{
		// write IL code into memory
		EmitResult result = compilation.Emit(ms);

		if (!result.Success)
		{
			// handle exceptions
			IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
				diagnostic.IsWarningAsError ||
				diagnostic.Severity == DiagnosticSeverity.Error);

			foreach (Diagnostic diagnostic in failures)
			{
				Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
			}
		}
		else
		{
			// load this 'virtual' DLL so that we can use
			ms.Seek(0, SeekOrigin.Begin);
			Assembly assembly = Assembly.Load(ms.ToArray());

			// create instance of the desired class and call the desired function
			Type type = assembly.GetType("RoslynCompileSample.Writer");
			object obj = Activator.CreateInstance(type);
			type.InvokeMember("Write",
				BindingFlags.Default | BindingFlags.InvokeMethod,
				null,
				obj,
				new object[] { "Hello World" });
		}
	}

	Console.ReadLine();
}

// You can define other methods, fields, classes and namespaces here