<Query Kind="Program">
  <NuGetReference>MongoDB.Driver</NuGetReference>
  <Namespace>MongoDB.Driver</Namespace>
  <Namespace>MongoDB.Bson</Namespace>
  <Namespace>MongoDB.Bson.Serialization</Namespace>
  <Namespace>MongoDB.Bson.Serialization.Attributes</Namespace>
</Query>

//Pascal recommends cockroach db
void Main()
{
	//BasicTest();
	
	InsertSomeCrap();
}

public void BasicTest()
{
	var dbClient = new MongoClient("mongodb://localhost:27017");

	var database = dbClient.GetDatabase("ScratchSpace");
	var collection = database.GetCollection<BsonDocument>("Numbers");

	var document = new BsonDocument
	{
		{ "number", 777 }
	};

	collection.InsertOne(document);
}

public void InsertSomeCrap()
{
	var sc = new SomeCrap
	{
		Numbers = GetList(),
		TwoDArray = TwoDimArray()
	};

	var doc = sc.ToBsonDocument();

	//var document = new BsonDocument
	//{
	//	{ "number", 777 }
	//};

	var collection = GetCollectionNumbers();

	collection.InsertOne(doc);
}

public class SomeCrap
{ 
	public string DoNotCare { get; set; }
	
	public List<int> Numbers { get; set; }
	
	public int[,] TwoDArray { get; set; }
}

public MongoClient GetClient() => new MongoClient("mongodb://localhost:27017");

public IMongoDatabase GetDb() => GetClient().GetDatabase("ScratchSpace");

public IMongoCollection<BsonDocument> GetCollectionNumbers() => GetDb().GetCollection<BsonDocument>("Numbers");

public List<int> GetList()
{
	var length = 10;

	var lst = new List<int>(length);

	for (var i = 0; i < length; i++)
	{
		lst.Add(i);
	}

	return lst;
}

public int[,] TwoDimArray()
{
	var r = 10;
	var c = 10;
	
	var tda = new int[r, c];
	
	for (int i = 0; i < r; i++)
	{
		for (int j = 0; j < c; j++)
		{
			tda[i, j] = j;
		}
	}
	
	tda.Dump();
	
	return tda;
}
