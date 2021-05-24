<Query Kind="Program" />

void Main()
{
	var lst = new List<Box>
	{
		new Box(1, 1, 1),
		new Box(1, 1, 1),
		new Box(1, 1, 1),
		new Box(1, 1, 1),
		new Box(1, 1, 1)
	};
	
	//Demonstration that the hash code for each object is fairly 
	//random and won't help you for getting a distinct list
	lst.ForEach(x => Console.WriteLine(x.GetHashCode()));
	
	//Demonstration that if your EqualityComparer is setup correctly
	//then you will get a distinct list
	lst = lst
		.Distinct(new BoxEqualityComparer())
		.ToList();
	
	lst.Dump();
}

public class Box
{
	public Box(int h, int l, int w)
	{
		this.Height = h;
		this.Length = l;
		this.Width = w;
	}

	public int Height { get; set; }
	public int Length { get; set; }
	public int Width { get; set; }

	public override String ToString()
	{
		return String.Format("({0}, {1}, {2})", Height, Length, Width);
	}
}

public class BoxEqualityComparer 
	: EqualityComparer<Box>
{
	public override bool Equals(Box b1, Box b2)
	{
		if (b2 == null && b1 == null)
			return true;
		else if (b1 == null || b2 == null)
			return false;
		else if (b1.Height == b2.Height && b1.Length == b2.Length
							&& b1.Width == b2.Width)
			return true;
		else
			return false;
	}

	public override int GetHashCode(Box bx)
	{
		#region This works
		//In this example each component of the box object are being XOR'd together
		int hCode = bx.Height ^ bx.Length ^ bx.Width;
		
		//The hashcode of an integer, is that same integer
		return hCode.GetHashCode();
		#endregion

		#region This won't work
		//Comment the above lines and uncomment this line below if you want to see Distinct() not work
		//return bx.GetHashCode();
		#endregion
	}
}