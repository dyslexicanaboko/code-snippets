<Query Kind="Program">
  <NuGetReference>StackExchange.Redis</NuGetReference>
  <Namespace>StackExchange.Redis</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

//https://docs.redis.com/latest/rs/references/client_references/client_csharp/
//https://www.c-sharpcorner.com/UploadFile/2cc834/using-redis-cache-with-C-Sharp/
//https://redis.com/redis-enterprise/data-structures/
static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(
			new ConfigurationOptions
			{
				EndPoints = { "localhost:6379" }
			});

void Main()
{
	HashEntryExample();
}

private void StringExample()
{
	var db = redis.GetDatabase();

	db.StringSet("Key", "Value01");

	db.StringGet("Key").Dump();
}

private void HashEntryExample()
{
	var db = redis.GetDatabase();

	var key = "HashEntryKey01";

	//Initial add
	db.HashSet(key, new HashEntry[]
	{
		new HashEntry("Id01", "Value01"),
		new HashEntry("Id02", "Value02"),
		new HashEntry("Id03", "Value03")
	});

	//Get
	db.HashGet(key, "Id03").Dump();

	//Append
	db.HashSet(key, new HashEntry[]
	{
		new HashEntry("Id04", "Value04")
	});
	
	//Review all
	db.HashGetAll(key).Dump();
}

private void HashEntryExample2()
{
	var db = redis.GetDatabase();

	var key = "HashEntryKey02";

	//Initial add
	db.HashSet(key, new HashEntry[]
	{
		new HashEntry("Id01", new DummyObject { Name = "Value01" })
	});

	//Get
	db.HashGet(key, "Id03").Dump();

	//Append
	db.HashSet(key, new HashEntry[]
	{
		new HashEntry("Id04", "Value04")
	});

	//Review all
	db.HashGetAll(key).Dump();
}

public class DummyObject
{ 
	public string Name { get; set; }
}

