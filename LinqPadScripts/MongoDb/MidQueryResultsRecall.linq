<Query Kind="Program">
  <NuGetReference>MongoDB.Driver</NuGetReference>
  <Namespace>MongoDB.Driver</Namespace>
  <Namespace>MongoDB.Bson</Namespace>
  <Namespace>MongoDB.Bson.Serialization</Namespace>
  <Namespace>MongoDB.Bson.Serialization.Attributes</Namespace>
  <Namespace>MongoDB.Bson.Serialization.IdGenerators</Namespace>
</Query>

public MongoClient GetClient() => new MongoClient("mongodb://localhost:27017");

public IMongoDatabase GetDb() => GetClient().GetDatabase("ScratchSpace");

public IMongoCollection<BsonDocument> GetCollectionBottlesOfBeer() => GetDb().GetCollection<BsonDocument>("BottlesOfBeer");

private IMongoCollection<BsonDocument> _collection;

void Main()
{
	_collection = GetCollectionBottlesOfBeer();

	var bc = GetBeerCounter();

	var nextStage = bc.StageId + 1;

	GoToTheBar(bc, 10);

	SaveProgress(bc, nextStage);
}

public BeerCounter GetBeerCounter()
{
	var c = _collection;

	var f = Builders<BsonDocument>.Filter.Eq("_id", "BeerCounter");

	var qDoc = c.Find(f).SingleOrDefault();

	BeerCounter bc = null;

	if (qDoc != null)
	{
		Console.WriteLine("Counter recalled");
		
		bc = BsonSerializer.Deserialize<BeerCounter>(qDoc);

		return bc;
	}

	Console.WriteLine("Created new counter");

	bc = new BeerCounter();

	var docBc = bc.ToBsonDocument();

	c.InsertOne(docBc);

	return bc;
}

public void SaveProgress(BeerCounter counter, int stageId)
{
	Console.WriteLine($"Saving count {counter.Bottles}");

	var c = _collection;

	var f = Builders<BsonDocument>.Filter.Eq("_id", "BeerCounter");

	var qDoc = c.Find(f).SingleOrDefault();
	
	counter.StageId = stageId;
	
	var doc = counter.ToBsonDocument();
	
	c.ReplaceOne(f, doc);
}

public void GoToTheBar(BeerCounter counter, int beersToDrink)
{
	Console.WriteLine($"You had {counter.Bottles} already.");
	Console.WriteLine("Getting sloshed");

	for (var i = 0; i < beersToDrink; i++)
	{
		counter.Drink();
	}
}

public class BeerCounter
{
	[BsonId(IdGenerator=typeof(StringObjectIdGenerator))]
	public string BeerCounterId { get; set; } = "BeerCounter";
	public int StageId { get; set; }
	public long Bottles { get; private set; }
	
	public void Drink() => Bottles++;
}
