<Query Kind="Statements" />

var lines = File.ReadAllLines(@"C:\Dump\delete3\projects.txt");

var arr = lines.Where(x => x.Contains(".csproj")).ToArray();

File.WriteAllLines(@"C:\Dump\delete3\projectsOnly.txt", arr);

Console.WriteLine("Finished");