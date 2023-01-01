<Query Kind="Program" />

void Main()
{
	//https://learn.microsoft.com/en-us/dotnet/api/system.array?redirectedfrom=MSDN&view=net-7.0
	//Max size of any array is 2GB or 2,147,483,648
	//int32 Max =                     2,147,483,647
	//32 bits is 32/8 = 4 bytes
	//Therefore, 2,147,483,648/4 = 536,870,912 max integery array elements

	GC.Collect();

	//var arr = new int[2147483591]; //2147483592 breaks it, 2147483591 is max, but why?
	//var arr = new long[2147483591]; //Same as int32!
	var arr = new double[2147483591]; //Same as int32!

	//There is a difference of 57 between 2GB and 2147483591, how is that significant?

	arr.Length.Dump();	
}
