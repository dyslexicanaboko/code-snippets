<Query Kind="Program" />

//https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/how-to-define-value-equality-for-a-type
void Main()
{
	var o1 = new Foo { Number = 1, Text = "A" };
	var o2 = new Foo { Number = 1, Text = "A" };

	Console.WriteLine("(o1 == o2)");
	
	(o1 == o2).Dump();

	Console.WriteLine();
	
	Console.WriteLine("o1.Equals(o2)");

	(o1.Equals(o2)).Dump();
}

// Define other methods and classes here
public class Foo
	: IEquatable<Foo>
{
	public int Number { get; set; }
	public string Text { get; set; }

	//Equals method from System.Object
	public override bool Equals(object obj)
	{
		Console.Write($"object.Equals -> ");

		return Equals(obj as Foo);
	}

	//IEquatable<T> implementation that is required for collections
	public bool Equals(Foo other)
	{
		var o = other;

		Console.WriteLine($"IEquatable.Equals ->\n\tthis.Text.Hc: {Text.GetHashCode()}\n\tother.Text.Hc: {o.Text.GetHashCode()}");

		var c = Number == o.Number && Text == o.Text;

		return c;
	}

	//Must implement GetHashCode() for sort operations to work properly
	public override int GetHashCode()
	{
		//Hashcode is a very sensitive subject and this is a poor implementation
		return Number.GetHashCode() + Text.GetHashCode();
	}

	//Checking for nulls otherwise using IEquatable<T>.Equals()
	public static bool operator ==(Foo lhs, Foo rhs)
	{
		// Check for null on left side.
		if (Object.ReferenceEquals(lhs, null))
		{
			if (Object.ReferenceEquals(rhs, null))
			{
				// null == null = true.
				return true;
			}

			// Only the left side is null.
			return false;
		}
		// Equals handles case of null on right side.
		return lhs.Equals(rhs);
	}

	//Does not equal implementation
	public static bool operator !=(Foo lhs, Foo rhs)
	{
		return !(lhs == rhs);
	}
}