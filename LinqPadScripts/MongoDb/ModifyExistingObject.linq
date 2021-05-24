<Query Kind="Program">
  <NuGetReference>MongoDB.Driver</NuGetReference>
  <Namespace>MongoDB.Driver</Namespace>
  <Namespace>MongoDB.Bson</Namespace>
  <Namespace>MongoDB.Bson.Serialization</Namespace>
  <Namespace>MongoDB.Bson.Serialization.Attributes</Namespace>
</Query>

public MongoClient GetClient() => new MongoClient("mongodb://localhost:27017");

public IMongoDatabase GetDb() => GetClient().GetDatabase("ScratchSpace");

public IMongoCollection<BsonDocument> GetCollectionPerson() => GetDb().GetCollection<BsonDocument>("Person");

//Cockroach db - checkout later?
void Main()
{
	var bob = new Person
	{
		FirstName = "Bob"
	};

	//If a collection doesn't exist it will be created, no need to worry about its existence
	//if you don't mind automatically creating a collection
	var c = GetCollectionPerson();

	var docBob = bob.ToBsonDocument();

	//var f = new BsonDocumentFilterDefinition<BsonDocument>(docBob);
	var f = Builders<BsonDocument>.Filter.Eq("FirstName", "Bob");

	//var qDoc = c.FindOneAndUpdate(f, docBob);
	//var dbDoc = c.FindAs<Person>(doc);
	var qDoc = c.Find(f).SingleOrDefault();
	
	if(qDoc == null)
	{
		Console.WriteLine("Created document");

		c.InsertOne(docBob);
	}
	else
	{
		Console.WriteLine("Updated document");

		var sc2 = BsonSerializer.Deserialize<Person>(qDoc);

		sc2.Dump();

		var mary = new Person
		{
			PersonId = sc2.PersonId,
			FirstName = "Mary"
		};
		
		var docMary = mary.ToBsonDocument();

		//Used update initially, needed to use replace instead otherwise I kept getting this error:
		//https://stackoverflow.com/questions/40841393/mongodb-element-name-id-not-valid-update/57733201
		c.ReplaceOne(f, docMary).Dump();
	}
	
}

public class Person
{
	//https://codingcanvas.com/using-mongodb-_id-field-with-c-pocos/
	//There are ID generation options besides this one
	//To satisfy the _id field, add this attribute to your main Id:
	[BsonId]
	public ObjectId PersonId { get; set; }
	public string FirstName { get; set; }
}
