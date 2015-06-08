using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace M101DotNet
{
	class Program
	{
		static void Main( string[] args )
		{
			MainAsync( args ).Wait();
			Console.WriteLine( "Press enter!" );
			Console.ReadLine();
		}


		//// 1- // *** BASICS *** ////
	
		//	static async Task MainAsync( string[] args )
		//{
		//// Talking to 2 servers in port 27017 running local

		//var connectionString = "mongodb://localhost:27017";
		////var settings = new MongoClientSettings
		////{
		////}

		//var client = new MongoClient( connectionString );

		//// GetDatabase() has an override with the MongoClientSettings
		//var db = client.GetDatabase( "test" );
		//// GetCollection() has an override with the MongoClientSettings
		//var col = db.GetCollection<BsonDocument>( "people" );

		//}


		////2 - //  *** Adding key,value pairs to a Bson document *** //

		//static async Task MainAsync( string[] args )
		//{
		//// Creation a dictionary of keys,values
		//var doc = new BsonDocument
		//{
		//    { "name", "Jones" }
		//};

		//// Add method, takes a key and a "BsonValue" which maps value into their proper type
		//doc.Add( "age", 30 );

		//// Add using indexers
		//doc["profession"] = "hacker";

		//// Using BsonArrays
		//var nestedArray = new BsonArray();
		//nestedArray.Add( new BsonDocument( "color", "red" ) );

		//// Adding the BsonArray to the BsonDocument
		//doc.Add( "array", nestedArray );

		//Console.WriteLine( doc );

		//// Accessing the elements in the BsonDocument is also straightforward:
		//Console.WriteLine( doc["array"][0]["color"] );
		//}



		//// 3 - //  *** POCO representation (Mapping POCO to Bson ) *** ////

		//	static async Task MainAsync( string[] args )
		//	{
		//		// POCOs attributes can be customized in 3 ways:

		//		// 3.a) Using Attributes
		//		// [BsonElement( "name" )]
		//		// [BsonIgnore], [BsonIgnoreExtraElements], [BsonIgnoreDefault],
		//		// [BsonRepresentation] (change type of a field, so that matches DB representation)

		//		// 3.b) Using the BsonClassMap and setting elements using ClassMapInitializers
		//		// Also, this can be done directly in the code w/ BsonClassMap to register that class
		//		// RegisterClassMap() is some kind of global registry

		//		// 3.c) Using conventions
		//		// Create the ConventionPack class where you add a convention (e.g. camelCase)
		//		// Add ConventionPack to the ConventionRegistry


		//		// 3.c)
		//		var conventionPack = new ConventionPack();
		//		conventionPack.Add( new CamelCaseElementNameConvention() );
		//		// Applies camelCase convention to all variables (t => true)
		//		ConventionRegistry.Register( "camelCase", conventionPack, t => true );


		//		// 3.b)
		//		BsonClassMap.RegisterClassMap<Person>( cm =>
		//		{
		//			cm.AutoMap();
		//			// 3.b)
		//			cm.MapMember( x => x.Name ).SetElementName( "name" );
		//		} );


		//		var person = new Person
		//		{
		//			// Id is not initialized, the it is set by default by MongoDB
		//			Name = "Jones",
		//			Age = 30,
		//			Colors = new List<string> { "red", "blue" },
		//			Pets = new List<Pet> { new Pet { Name = "Fluffy", Type = "Pig" } },
		//			ExtraElements = new BsonDocument( "anotherName", "anotherValue" )
		//		};

		//		// This happens in MongoDB under the covers, but we explicitly write it so
		//		// it can be observed in the console
		//		using ( var writer = new JsonWriter( Console.Out ) )
		//		{
		//			BsonSerializer.Serialize( writer, person );
		//		}

		//	}

		//	class Person
		//	{
		//		public ObjectId Id { get; set; }

		//		// 3.a)
		//		//[BsonElement("name")]   
		//		public string Name { get; set; }

		//		// 3.a)
		//		//[BsonRepresentation (BsonType.String)]
		//		public int Age { get; set; }

		//		public List<string> Colors { get; set; }

		//		public List<Pet> Pets { get; set; }

		//		public BsonDocument ExtraElements { get; set; }
		//	}

		//	public class Pet
		//	{
		//		public string Name { get; set; }

		//		public string Type { get; set; }
		//	}


		//// 4- *** .NET Driver *** //

		//static async Task MainAsync( string[] args )
		//{
		//	var client = new MongoClient();
		//	var db = client.GetDatabase( "test" );

		//	var col = db.GetCollection<BsonDocument>( "people" );

		//	var doc = new BsonDocument
		//	{
		//		{ "Name", "Smith" },
		//		{ "Age", 30 },
		//		{ "Profession", "Hacker"}
		//	};

		//	//// 4.a) Insert one document in DB collection
		//	//await col.InsertOneAsync( doc );

		//	//// 4.b) Insert Many documents in a DB collection
		//	//var doc2 = new BsonDocument
		//	//{
		//	//	{ "SomethingElse", true }
		//	//};

		//	//await col.InsertManyAsync( new[] { doc, doc2 } );

		//	//// 4.c) Add an element of a type given by a class
		//	var doc3 = new Person
		//	{
		//		Name = "Jack",
		//		Age = 24,
		//		Profession = "Hacker"
		//	};

		//	// Note that the collection has to be changed to have elements of type Person, 
		//	// so that the document can be added.
		//	var colPeople = db.GetCollection<Person>( "people" );

		//	Console.WriteLine( doc3.Id );

		//	await colPeople.InsertOneAsync( doc3 );

		//	// We mutate the object identifier

		//	Console.WriteLine( doc3.Id );

		//	// Adding the same document twice will generate a Mongo key exception (duplicate key)
		//	//await colPeople.InsertOneAsync( doc3 );
		//}


		//// 5- *** .NET Driver, Find() *** //

		//// Retrieves the Documents from a MongoDB collection
		//static async Task MainAsync( string[] args )
		//{
		//	var client = new MongoClient();
		//	var db = client.GetDatabase( "test" );
		//	var col = db.GetCollection<BsonDocument>( "people" );

		//	// 5.a) CURSORS: Most difficult, but flexible way
		//	//using ( var cursor = await col.Find( new BsonDocument() ).ToCursorAsync() )
		//	//{
		//	//	while ( await cursor.MoveNextAsync() )
		//	//	{
		//	//		foreach ( var doc in cursor.Current )
		//	//		{
		//	//			Console.WriteLine( doc );
		//	//		}
		//	//	}
		//	//}

		//	// 5.b) Bring documents into memory first: Cleaner, but not reliable if DB Collection has changed.
		//	//var list = await col.Find( new BsonDocument() ).ToListAsync();

		//	//foreach ( var doc in list )
		//	//{
		//	//	Console.WriteLine( doc );
		//	//}

		//	// 5.c) ForEachAsync (4 overloads w/task: BsonDoc, Bsondoc+index, Bsondoc+task, Bsondoc+index+Task)
		//	await col.Find( new BsonDocument() ).
		//		ForEachAsync( doc => Console.WriteLine( doc ) );
		//}




		//// 6- *** .NET Driver, Find with filters *** //

		//// Retrieves the Documents from a MongoDB collection
		//static async Task MainAsync( string[] args )
		//{
		//	var client = new MongoClient();
		//	var db = client.GetDatabase( "test" );
		//	var col = db.GetCollection<BsonDocument>( "people" );

		//	//// 6.a) The filter is a BsonDocument
		//	var filter1 = new BsonDocument( "Name" , "Smith");
		//	// Find() is expecting a BsonDefinition as an argument
		//	// Although filter is a BsonDocument, there are some implicit conversions between BsonDocuments and filterDefinitions
			
		//	//// 6.b) The filter can be a more complex BsonDocument
		//	var filter2 = new BsonDocument( "$and", new BsonArray
		//		{
		//			new BsonDocument("Age", new BsonDocument("$lt", 31)),
		//			new BsonDocument("Name", "Smith")
		//		} );
			
		//	// 6.c) The filter can be used (optimal situation) with the filter builder 
		//	var builder = new FilterDefinitionBuilder<BsonDocument>();
		//	var simpleBuilder = Builders<BsonDocument>.Filter;

		//	var filter3 = simpleBuilder.Lt( "Age", 31 );
		//	var filter4 = simpleBuilder.And( simpleBuilder.Lt( "Age", 31 ), simpleBuilder.Eq( "Name", "Jack" ) );
		//	// using operators overloaded in Builders (|, &, !)
		//	var filter5 = simpleBuilder.Lt( "Age", 31 ) | simpleBuilder.Eq( "Name", "Jones" );


		//	//var list = await col.Find( filter5 ).ToListAsync();

		//	//foreach ( var doc in list )
		//	//{
		//	//	Console.WriteLine( doc );
		//	//}



		//	// 6.d) Using Class Types instead of BsonDocuments, where it is much easier to use expression trees
		//	var personCol = db.GetCollection<Person>( "people" );
		//	var builder2 = new FilterDefinitionBuilder<Person>();
		//	var personBuilder = Builders<Person>.Filter;

		//	var filter6 = personBuilder.Lt( x => x.Age, 31 ) | personBuilder.Eq( x => x.Name, "Jones" );


		//	var personList = await personCol.Find( filter6 ).ToListAsync();

		//	//foreach ( var doc in personList )
		//	//{
		//	//	Console.WriteLine( doc );
		//	//}

		//	// 6.e) Using Strongly Typed objects, filters are unnecessary, expression trees can be used in-line with filter info
		//	personList = await personCol.Find( x => x.Age < 30 && x.Name != "Jones" ).ToListAsync();

		//	foreach ( var doc in personList )
		//	{
		//		Console.WriteLine( doc );
		//	}

		
		//}

		//// 7- *** Sorts, Skips and limits *** //

		// Doings Limits and skips without sorts is dangerous!
		// You do NOT know the order where the documents are going to come out.

		static async Task MainAsync( string[] args )
		{
			var client = new MongoClient();
			var db = client.GetDatabase( "test" );
			var col = db.GetCollection<BsonDocument>( "people" );

			// Limit, puts a limit on to the amount of documents retrieved.
			// Skip, jumps a number of documents
			var list = await col.Find( new BsonDocument() ).Skip(1).Limit( 1 )
				.ToListAsync();

			// Sort, by Age ascending
			var list2 = await col.Find( new BsonDocument() ).Sort( "{Age : 1}" )
				.ToListAsync();
			// Sort, by Age ascending
			var list3 = await col.Find( new BsonDocument() ).
				Sort( new BsonDocument( "Age", 1 ) ).
				ToListAsync();
			// Sort, using the Builders helper (allows ascending and descending)
			var list4 = await col.Find( new BsonDocument() ).
				Sort( Builders<BsonDocument>.Sort.Ascending( "Age").Descending("Name") ).
				ToListAsync();
			




			// Using, strongly typed classes (Person)
			var colPerson = db.GetCollection<Person>( "people" );


			var list5 = await colPerson.Find( new BsonDocument() ).
				Sort( Builders<Person>.Sort.Ascending( x => x.Age ) ).
				ToListAsync();

			var list6 = await colPerson.Find( new BsonDocument() ).
				SortBy( x => x.Age ).
				ThenByDescending( x => x.Name ).
				ToListAsync();


			foreach ( var doc in list5 )
			{
				Console.WriteLine( doc );
			}

		}


	}

}
