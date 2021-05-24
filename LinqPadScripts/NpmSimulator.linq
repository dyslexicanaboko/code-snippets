<Query Kind="Program" />

void Main()
{
	var npm = new NpmIsTerrible();
	
	npm.Start();
}

// Define other methods and classes here
public class NpmIsTerrible
{
	private string _cd = @"C:\";
	private bool _keepRunning = true;
	
	public void Start()
	{
		Console.WriteLine("Welcome to NPM simulator! Bringing you the same terrible experience as the same NPM, but SIMULATED!");
		
		PrintCd();

		while (_keepRunning)
		{
			NewCommand();
		}
	}
	
	public void Cli(string input)
	{
		string cmd;
		List<string> tokens;

		if (string.IsNullOrWhiteSpace(input))
		{
			cmd = null;
			
			tokens = new List<string>();
		}
		else
		{
			tokens = input.Split(' ').ToList();
			
			//Get the root command
			cmd = tokens[0].ToLower();
			
			//Remove the root command, don't need it anymore
			tokens.RemoveAt(0);
			
			//Remove excess whitesapce so it isn't interpretted unecessarily
			tokens.RemoveAll(x => string.IsNullOrWhiteSpace(x));
		}
		
		switch (cmd)
		{
			case null:
				PrintCd();
				break;

			case "cd":
				ChangeDirectory(tokens);
				break;

			case "npm":
				CliNpm(tokens);
				break;

			case "exit":
			case "quit":
				Console.WriteLine("NPM failed to exit correctly.");
				
				_keepRunning = false;
				break;

			default:
				Console.WriteLine("Command not recognized or supported. The developers leave them out for fun sometimes.");
				break;
		}
	}

	private void NewCommand()
	{
		var input = Console.ReadLine();

		PrintCd();

		Console.WriteLine(input);

		Cli(input);
	}

	private void ChangeDirectory(List<string> commands)
	{
		var input = string.Join(string.Empty, commands);
		
		//Remove any quotations because they won't be used anyhow
		input = input.Replace("\"", string.Empty);
		
		_cd = input;
	}

	private void PrintCd()
	{
		Console.Write($"{_cd}>"); 
	}
	
	private void CliNpm(List<string> commands)
	{
		var cmd = commands[0];
		
		switch (cmd)
		{
			case "install":
				Console.WriteLine("Nahh");
				break;
		}
	}
}