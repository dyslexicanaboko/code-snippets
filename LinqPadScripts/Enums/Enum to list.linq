<Query Kind="Program" />

void Main()
{
	//GetEnumArray<SqlDbType>().Dump();

	//GetEnumDictionary<SqlDbType>(true).Dump();
	
	GetEnumListItems<SqlDbType>().Dump();
}

public T[] GetEnumArray<T>() 
	where T : struct, IConvertible
{
	if (!typeof(T).IsEnum)
		throw new ArgumentException("T must be an enumerated type");
		
	var t = typeof(T);
	
	var arr = (T[])Enum.GetValues(t);

	return arr;
}

public Dictionary<string, T> GetEnumDictionary<T>(bool? keyIsLowerCase = null)
	where T : struct, IConvertible
{
	if (!typeof(T).IsEnum)
		throw new ArgumentException("T must be an enumerated type");

	var t = typeof(T);

	var names = Enum.GetNames(t);

	if (keyIsLowerCase.HasValue)
	{
		Func<string, string> f;

		if (keyIsLowerCase.Value)
        {
            f = s => s.ToLower();
        }
		else
		{
			f = s => s.ToUpper();
		}

		names = names.Select(x => f(x)).ToArray();
	}

	var values = (T[])Enum.GetValues(t);

	var dict = new Dictionary<string, T>(names.Length);

	for (var i = 0; i < names.Length; i++)
	{
		dict.Add(names[i], values[i]);
	}
	
	return dict;
}

public Dictionary<string, int> GetEnumListItems<T>()
	where T : struct, IConvertible
{
	if (!typeof(T).IsEnum)
		throw new ArgumentException("T must be an enumerated type");

	var t = typeof(T);

	var names = Enum.GetNames(t);

	var values = (T[])Enum.GetValues(t);

	var dict = new Dictionary<string, int>(names.Length);

	for (var i = 0; i < names.Length; i++)
	{
		dict.Add(names[i], Convert.ToInt32(values[i]));
	}

	return dict;
}