<Query Kind="Statements" />

string.Compare("x", "y", StringComparison.OrdinalIgnoreCase).Dump();
string.Compare("x", "x", StringComparison.OrdinalIgnoreCase).Dump();
string.Compare("y", "x", StringComparison.OrdinalIgnoreCase).Dump();
Console.WriteLine();
Console.WriteLine();
string.Compare("x", "y", true).Dump();
string.Compare("x", "x", true).Dump();
string.Compare("y", "x", true).Dump();
Console.WriteLine();
Console.WriteLine();
string.Compare("unknown ie", "uknown", StringComparison.OrdinalIgnoreCase).Dump();
string.Compare("unknown ie", "ie", StringComparison.OrdinalIgnoreCase).Dump();
string.Compare("unknown", "unknown ie", StringComparison.OrdinalIgnoreCase).Dump();
string.Compare("ie", "unknown ie", StringComparison.OrdinalIgnoreCase).Dump();
