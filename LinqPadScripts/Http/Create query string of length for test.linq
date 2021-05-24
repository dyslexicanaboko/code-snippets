<Query Kind="Statements" />

var sb = new StringBuilder();

var qspLength = 5000;

sb.Append("qspTest=");

var characters = qspLength - sb.Length;

for (int i = 0; i < characters; i++)
{
	sb.Append("a");
}

sb.Length.Dump();
sb.ToString().Dump();