<Query Kind="Statements" />

Process[] pname = Process.GetProcessesByName("notepad");

if (pname.Length == 0)
	Console.WriteLine("It's not running");
else
	Console.WriteLine("It's running you have to wait");
	
	
	