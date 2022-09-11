<Query Kind="Program">
  <NuGetReference>MongoDB.Driver</NuGetReference>
  <Namespace>MongoDB.Driver</Namespace>
  <Namespace>MongoDB.Bson</Namespace>
  <Namespace>MongoDB.Bson.Serialization</Namespace>
  <Namespace>MongoDB.Bson.Serialization.Attributes</Namespace>
  <Namespace>MongoDB.Bson.Serialization.IdGenerators</Namespace>
</Query>

//Store different types of objects in the same collection and differentiate between them
private const string CollectionName = "ObjectVersions";
public MongoClient GetClient() => new MongoClient("mongodb://localhost:27017");

private IMongoDatabase _db;
public IMongoDatabase GetDb()
{
	if (_db != null) return _db;
		
	_db = GetClient().GetDatabase("ScratchSpace");
	
	return _db;
}

public IMongoCollection<BsonDocument> GetBsonCollection() => GetDb().GetCollection<BsonDocument>(CollectionName);
public IMongoCollection<ObjectV1> GetObjectV1Collection() => GetDb().GetCollection<ObjectV1>(CollectionName);
public IMongoCollection<ObjectV2> GetObjectV2Collection() => GetDb().GetCollection<ObjectV2>(CollectionName);

void Main()
{
//	AddObjectV1();
//
//	AddObjectV2();

	var lst = GetAll(GetBsonCollection());

	Console.WriteLine("============================");
	Console.WriteLine("============================");
	Console.WriteLine("============================");
	
	lst.FindAll(x => x.TryGetElement("Type", out var element) && element.Value == 1).Dump();
	
	//These won't work because they are strong types
	//GetAll(GetObjectV1Collection());
	//GetAll(GetObjectV2Collection());
}

private List<T> GetAll<T>(IMongoCollection<T> collection)
{
	try
	{	        
		var f = Builders<T>.Filter.Empty;
	
		var q = collection.Find(f).ToList();
	
		q.Dump();
		
		return q;
	}
	catch (Exception ex)
	{
		//An exception will be thrown if the collection cannot be casted perfectly to T
		Console.WriteLine(ex.Message);
		
		return null;
	}
}

private void AddObjectV1()
{
	var c = GetObjectV1Collection();
	
	var obj = new ObjectV1
	{
		Type = ObjectType.V1,
		PropertyA = "A",
		PropertyB = "B",
	};

	c.InsertOne(obj);
}

private void AddObjectV2()
{
	var c = GetObjectV2Collection();
	
	var obj = new ObjectV2
	{
		Type = ObjectType.V2,
		PropertyA = "A",
		PropertyC = "C"
	};

	c.InsertOne(obj);
}

public enum ObjectType { V1 = 1, V2 = 2}

public class ObjectV1
{
	[BsonId(IdGenerator=typeof(GuidGenerator))]
	public Guid EntryId { get; set; }
	public ObjectType Type { get; set; }
	public string PropertyA { get; set; }
	public string PropertyB { get; set; }
}

public class ObjectV2
{
	[BsonId(IdGenerator = typeof(GuidGenerator))]
	public Guid EntryId { get; set; }
	public ObjectType Type { get; set; }
	public string PropertyA { get; set; }
	public string PropertyC { get; set; }
}
