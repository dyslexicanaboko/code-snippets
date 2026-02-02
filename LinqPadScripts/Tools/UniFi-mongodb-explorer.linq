<Query Kind="Program">
  <NuGetReference Version="2.27.0">MongoDB.Driver</NuGetReference>
  <Namespace>MongoDB.Driver</Namespace>
  <Namespace>MongoDB.Bson</Namespace>
</Query>

/*
UniFi Network application uses MongoDB version 3.6.23, which is wire version 6
Required driver: MongoDB.Driver:2.27.0

Database: ace
Collection: admin
	There is a single document
Property: x_shadow

Replace the property value with:
$6$OzJJ0heL$XyD5qt4pviLieuj8CMFbnSc9VYvxDyzYpH7dHC8wmaLwKv9xwyDxBiMx3GcT8nEdIa7XJbqlZo39jhfbQBXRM/

Which is the temporary password:
password123

You need to update your password when finished.

Properties
`email` is the email
`name` is the username
`x_shadow` is the encrypted password
*/
MongoClient _client = new MongoClient("mongodb://localhost:27117");

void Main()
{
	//TestConnection();
	//ListDb();
	//ListCollections("ace");
	ListDocuments("ace", "admin");
	//ListDocumentProperty("ace", "admin", "email");
	//ListDocumentProperty("ace", "admin", "x_shadow");
	//UpdateProperty("ace", "admin", "x_shadow", "$6$OzJJ0heL$XyD5qt4pviLieuj8CMFbnSc9VYvxDyzYpH7dHC8wmaLwKv9xwyDxBiMx3GcT8nEdIa7XJbqlZo39jhfbQBXRM/");
	//ListDocumentProperty("ace", "admin", "x_shadow");
}

public void UpdateProperty(
	string databaseName,
	string collectionName,
	string property,
	string value)
{
	var db = _client.GetDatabase(databaseName);
	var collection = db.GetCollection<BsonDocument>(collectionName);

	// Since the collection has exactly one document, match all
  var filter = FilterDefinition<BsonDocument>.Empty;

	// Set x_shadow = newValue
	var update = Builders<BsonDocument>.Update.Set(property, value);

	var result = collection.UpdateOne(filter, update);

	Console.WriteLine($"Matched: {result.MatchedCount}, Modified: {result.ModifiedCount}");
}

public void ListDocumentProperty(
	string databaseName,
	string collectionName,
	string property)
{
	var db = _client.GetDatabase(databaseName);
	var collection = db.GetCollection<BsonDocument>(collectionName);

	var projection = Builders<BsonDocument>.Projection.Include(property);
	
	var docs = collection
		.Find(FilterDefinition<BsonDocument>.Empty)
		.Project(projection) .ToList();
		
	foreach (var doc in docs) 
	{ 
		if (doc.Contains(property)) 
			Console.WriteLine(doc[property].AsString);
		else
			Console.WriteLine($"(no {property} field)");
	}
}

public void ListDocuments(string databaseName, string collectionName)
{
	var db = _client.GetDatabase(databaseName);
	var collection = db.GetCollection<BsonDocument>(collectionName);

	var documents = collection
		.Find(FilterDefinition<BsonDocument>.Empty)
		.ToList();

	foreach (var doc in documents)
	{
		foreach (var element in doc.Elements) 
		{ 
			Console.WriteLine($"{element.Name}: {element.Value}"); 
		}
	}
}

public void ListCollections(string databaseName)
{
	var db = _client.GetDatabase(databaseName); 
	
	var collections = db.ListCollections()
	.ToEnumerable()
	.OrderBy(x => x["name"].AsString)
	.ToList(); 
	
	foreach (var collection in collections) 
	{ 
		Console.WriteLine(collection["name"].AsString); 
	}
}

public void ListDb()
{
	var dbs = _client.ListDatabases().ToList();

	foreach (var db in dbs)
		Console.WriteLine(db["name"].AsString);

	//var db = client.GetDatabase("yourDatabaseName"); var collections = db.ListCollections().ToList(); foreach (var collection in collections) { Console.WriteLine(collection["name"]); }
}

public void TestConnection()
{
	try
	{
		// Ping the admin database to verify the connection
		var db = _client.GetDatabase("admin");
		var command = new BsonDocument("ping", 1);
		db.RunCommand<BsonDocument>(command);

		Console.WriteLine("Connection successful");
	}
	catch (Exception ex)
	{
		Console.WriteLine("Connection failed:");
		Console.WriteLine(ex.Message);
	}
}
