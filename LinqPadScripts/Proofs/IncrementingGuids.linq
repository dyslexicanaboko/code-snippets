<Query Kind="Program">
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Buffers.Binary</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
</Query>

// Creates a new guid based on the value in the string.  The value is made up
// of hex digits speared by the dash ("-"). The string may begin and end with
// brackets ("{", "}").
//
// The string must be of the form dddddddd-dddd-dddd-dddd-dddddddddddd. where
// d is a hex digit. (That is 8 hex digits, followed by 4, then 4, then 4,
// then 12) such as: "CA761232-ED42-11CE-BACD-00AA0057B223"

void Main()
{
	//         A        B    C    D    E                A          B     C     D     E
	var min = "00000000-0000-0000-0000-000000000000"; //0         -0    -0    -0    -0
	var max = "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"; //4294967295-65535-65535-65535-281474976710655
	var ex1 = "67624A31-850B-4525-B4BF-778D20D47076"; //1734494769-34059-17701-46271-131448024887414
	var sg = ex1;
	
	var ig = new IncrementGuid(sg);
	ig.HexGuid.Dump();
	ig.LongGuidString.Dump();
	//ig.Add(281474976710655 - 131448024887414 + 1).Dump();
	//ig.Add(1).Dump();
	//ig.Add(2).Dump();
	//ig.Add(3).Dump();
	//ig.Add(4).Dump();

	var lst = new List<Guid>(100);

	for (int i = lst.Capacity - 1; i >= 0; i--)
	{
		lst.Add(ig.Add(1000));
	}

	lst.OrderBy(x => x).ToList().Dump();
}

public class IncrementGuid
{
	//     Hex                                    Decimal (Int64)
	//     A        B    C    D    E              A          B     C     D     E
	//Min: 00000000-0000-0000-0000-000000000000 //0         -0    -0    -0    -0
	//Max: FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF //4294967295-65535-65535-65535-281474976710655
	private const long MaxA = 4294967295;
	private const long MaxB = 65535;
	private const long MaxC = 65535;
	private const long MaxD = 65535;
	private const long MaxE = 281474976710655;

	private readonly string _guidString;
	private readonly long[] _longSegments;
	private Segment _a;
	private Segment _b;
	private Segment _c;
	private Segment _d;
	private Segment _e;

	public string LongGuidString { get; private set; }
	public Guid HexGuid { get; private set; }

	public IncrementGuid(string guid)
	{
		_guidString = guid;

		_longSegments = guid
			.Split('-')
			.Select(x => Int64.Parse(x, System.Globalization.NumberStyles.HexNumber))
			.ToArray();

		//Break it down for easy access
		_a = new Segment(_longSegments[0], MaxA, 8);
		_b = new Segment(_longSegments[1], MaxB, 4);
		_c = new Segment(_longSegments[2], MaxC, 4);
		_d = new Segment(_longSegments[3], MaxD, 4);
		_e = new Segment(_longSegments[4], MaxE, 12);

		//Setup the relationships between the segments
		_b.Left = _a;
		_c.Left = _b;
		_d.Left = _c;
		_e.Left = _d;

		LongGuidString = FormatAsGuidString(_longSegments);

		HexGuid = new Guid(_guidString);
	}

	private string FormatAsGuidString<T>(IEnumerable<T> segments) => string.Join('-', segments);

	public Guid Add(long number)
	{
		TryAdd(_e, number);

		var str = GetHexGuidString(_a, _b, _c, _d, _e);

		//Console.WriteLine(str);

		return new Guid(str);
	}

	private void TryAdd(Segment segment, long number)
	{
		if(segment == null) throw new Exception("Guid overflow - cannot increment further, max reached.");
		
		var sum = segment.Value + number;
		
		//If this Segment is not larger than max then it can be incremented.
		if (segment.Max > sum)
		{
			segment.Value = sum;
			
			return;
		}

		//If the segment is larger than or equal to max, then it is has to be reset to zero
		//after reducing the number by the current value and we attempt to increment the 
		//next segment.
		var remainder =  sum - segment.Max; //Calculate the remainder
		
		//Console.WriteLine(remainder);
		
		segment.Value = 0; //Reset to zero
		
		TryAdd(segment.Left, remainder);
	}

	private string GetHexGuidString(params Segment[] segments)
	{
		var arr = segments
			.Select(s => string.Format("{0:X" + s.HexLength + "}", s.Value))
			.ToArray();

		var str = FormatAsGuidString(arr);

		return str;
	}

	private class Segment
	{
		public Segment(long value, long max, int hexLength)
		{
			Value = value;
			
			Max = max;
			
			HexLength = hexLength;
		}

		public int HexLength { get; set; }
		
		public long Max { get; set; }
		
		public long Value { get; set; }

		public Segment Left { get; set; }
	}
}

//Didn't work at all
private void BytesToDecimal()
{
	//8-4-4-4-12, 4-2-2-2-6
	var g1 = new Guid("67624A31-850B-4525-B4BF-778D20D47076");

	//16 bytes
	var b = g1.ToByteArray();

	b.Dump();

	//2 bytes per element
	var dA = Sum(b, 0, 4);  // 0,  1,  2,  3
	var dB = Sum(b, 4, 2);  // 4,  5,  
	var dC = Sum(b, 6, 2);  // 6,  7
	var dD = Sum(b, 8, 2);  // 8,  9, 
	var dE = Sum(b, 10, 6);  //10, 11, 12, 13, 14, 15
}

private int Sum(byte[] arr, int index, int length)
{
	int s = 0;
	
	var l = index + length;
	
	int p = length;
	
	for (int i = index; i < l; i++)
	{
		s += arr[i] * (int)Math.Pow(10, p);
		
		Console.WriteLine($"{i} - {arr[i]} - P: {p} - Sub: {s}");

		p--;
	}
	
	Console.WriteLine($"Segment: {s}");
	
	return s;
}

private void Blah()
{
	var g1 = new Guid("67624A31-850B-4525-B4BF-778D20D47076");
	var g2 = new Guid("4C80646F-7836-4B0B-852F-76B880EC6AEC");
	var g3 = new Guid("11B165B0-D9DB-4A50-A197-A92DA7888CA3");

	var lst = new List<Guid> { g1, g2, g3 };

	lst.OrderBy(x => x).ToList().Dump();

	g1.CompareTo(g2).Dump();
}

/*
private readonly int _a;
private readonly short _b;
private readonly short _c;
private readonly byte _d;
private readonly byte _e;
private readonly byte _f;
private readonly byte _g;
private readonly byte _h;
private readonly byte _i;
private readonly byte _j;
private readonly byte _k;
*/

private static int GetResult(uint me, uint them) => me < them ? -1 : 1;

public int CompareTo(GuidInc left, GuidInc right)
{
	if (left._a != right._a)
	{
		return GetResult((uint)right._a, (uint)left._a);
	}

	if (left._b != right._b)
	{
		return GetResult((uint)right._b, (uint)left._b);
	}

	if (left._c != right._c)
	{
		return GetResult((uint)right._c, (uint)left._c);
	}

	if (left._d != right._d)
	{
		return GetResult(right._d, left._d);
	}

	if (left._e != right._e)
	{
		return GetResult(right._e, left._e);
	}

	if (left._f != right._f)
	{
		return GetResult(right._f, left._f);
	}

	if (left._g != right._g)
	{
		return GetResult(right._g, left._g);
	}

	if (left._h != right._h)
	{
		return GetResult(right._h, left._h);
	}

	if (left._i != right._i)
	{
		return GetResult(right._i, left._i);
	}

	if (left._j != right._j)
	{
		return GetResult(right._j, left._j);
	}

	if (left._k != right._k)
	{
		return GetResult(right._k, left._k);
	}

	return 0;
}

public class GuidInc
{
	public int _a;  
	public short _b;
	public short _c;
	public byte _d; 
	public byte _e; 
	public byte _f; 
	public byte _g; 
	public byte _h; 
	public byte _i; 
	public byte _j; 
	public byte _k; 

	public GuidInc(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
	{
		_a = (int)a;
		_b = (short)b;
		_c = (short)c;
		_d = d;
		_e = e;
		_f = f;
		_g = g;
		_h = h;
		_i = i;
		_j = j;
		_k = k;
	}
}
