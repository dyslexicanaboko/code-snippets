using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

//https://stackoverflow.com/questions/826398/is-it-possible-to-dynamically-compile-and-execute-c-sharp-code-fragments
//https://stackoverflow.com/questions/50879342/roslyn-in-memory-compilation-cs0103-the-name-console-does-not-exist-in-the-c/50882172
namespace CSharpCompiler
{
    /// <summary>
    /// Example of how to take C# code in a string, compile it and use it live.
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // define source code, then parse it (to the type used for compilation)
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(@"
                using System;

                namespace CSharpCompiler
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

            var assemblyName = Path.GetRandomFileName();

            //Analyze and generate IL code from syntax tree
            var compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                //Write IL code into memory
                var result = compilation.Emit(ms);

                if (!result.Success)
                {
                    //Handle exceptions
                    var failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (var diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    // load this 'virtual' DLL so that we can use
                    ms.Seek(0, SeekOrigin.Begin);
                    
                    var assembly = Assembly.Load(ms.ToArray());

                    // create instance of the desired class and call the desired function
                    var type = assembly.GetType("CSharpCompiler.Writer");
                    
                    var obj = Activator.CreateInstance(type);
                    
                    type.InvokeMember("Write",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        obj,
                        new object[] { "Hello World" });
                }
            }

            Console.ReadLine();
        }
    }
}