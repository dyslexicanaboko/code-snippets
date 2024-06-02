<Query Kind="Program">
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
</Query>

//https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/how-to-define-value-equality-for-a-type
void Main()
{
	Demo4();
}

private void Demo4()
{
	var o1 = new Foo2 { Number = 1, Text = "b" };
	var o2 = new Foo2 { Number = 1, Text = "A" };

	var dict = new Dictionary<Foo2, int>(new Foo2EqualityComparer()) { { o1, 1 }, { o2, 1 } };
	dict.Dump();

	//Locates Foo2 because the Dictionary was explicitly told how to do so
	dict.Remove(new Foo2 { Number = 1, Text = "A" });

	dict.Dump();
}

private void Demo3()
{
	var o1 = new Foo2 { Number = 1, Text = "b" };
	var o2 = new Foo2 { Number = 1, Text = "A" };

	var dict = new Dictionary<Foo2, int>() { { o1, 1 }, { o2, 1} };
	dict.Dump();

	//Cannot locate Foo2 because Dictionaries don't function like lists and must be given an IEqualityComparer<T> to perform comparisons
	dict.Remove(new Foo2 { Number = 1, Text = "A" });

	dict.Dump();
}

private void Demo2()
{
	var o1 = new Foo2 { Number = 1, Text = "b" };
	var o2 = new Foo2 { Number = 1, Text = "A" };

	var lst = new List<Foo2> { o1, o2 };
	lst.Dump();

	//Cannot locate Foo2
	lst.Remove(new Foo2 { Number = 1, Text = "A" });

	lst.Dump();
}

private void Demo()
{
	var o1 = new Foo { Number = 1, Text = "b" };
	var o2 = new Foo { Number = 1, Text = "A" };

	var lst = new List<Foo> { o1, o2 };
	lst.Dump();

	//Foo located due to IEquality being implemented - this is specific to lists only!
	lst.Remove(new Foo { Number = 1, Text = "A" });

	lst.Dump();
}

public class Foo2
{
	public int Number { get; set; }
	public string Text { get; set; }
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

public class Foo2EqualityComparer : IEqualityComparer<Foo2>
{
	public bool Equals(Foo2 lhs, Foo2 rhs)
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
		return Foo2.Equals(lhs, rhs);
	}

	public int GetHashCode(Foo2 obj)
	{
		return obj.Number.GetHashCode() + obj.Text.GetHashCode();
	}
}