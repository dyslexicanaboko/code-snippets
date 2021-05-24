<Query Kind="Program" />

void Main()
{
	string alpha = "1-877-633-BEES";
	
	string converted = GetActualPhoneNumber(alpha);

	Console.WriteLine($"Original : {alpha}");
	Console.WriteLine($"Converted: {converted}");
}

public string GetActualPhoneNumber(string alphaNumber)
{
	//Upper case just so I don"t have to deal with case
	List<string> lstOld = alphaNumber
		.ToUpper()
		.ToCharArray()
		.Select(x => x.ToString())
		.ToList();
	
	var lstNew = new List<string>(lstOld.Count);
	
	var r = new Regex("^[a-zA-Z0-9]*$");
	
	foreach (string c in lstOld)
	{
		if(r.IsMatch(c))
			lstNew.Add(AlphaToNumber(c).ToString());
		else
			lstNew.Add(c);
	}
	
	return string.Join("", lstNew);
}

// Define other methods and classes here
public int AlphaToNumber(string alphaDigit)
{
	int digit = 0;
	
	switch (alphaDigit)
	{
		case "A":
		case "B":
		case "C":
			digit = 2;
			break;

		case "D":
		case "E":
		case "F":
			digit = 3;
			break;

		case "G":
		case "H":
		case "I":
			digit = 4;
			break;

		case "J":
		case "K":
		case "L":
			digit = 5;
			break;

		case "M":
		case "N":
		case "O":
			digit = 6;
			break;

		case "P":
		case "Q":
		case "R":
		case "S":
			digit = 7;
			break;

		case "T":
		case "U":
		case "V":
			digit = 8;
			break;

		case "W":
		case "X":
		case "Y":
		case "Z":
			digit = 9;
			break;
			
		default:
			digit = Convert.ToInt32(alphaDigit);
			break;
	}

	return digit;
}