using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;

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

        static async Task MainAsync( string[] args )
        {
            // 1- // *** BASICS *** ////

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




            //2 - //  *** Adding key,value pairs to a Bson document *** //
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



            //3 - //  *** POCO representation (Mapping POCO to Bson ) *** ////

			// POCOs attributes can be customized in 3 ways:

            // 3.a) Using Attributes
            // [BsonElement( "name" )]
            // [BsonIgnore], [BsonIgnoreExtraElements], [BsonIgnoreDefault],
            // [BsonRepresentation] (change type of a field, so that matches DB representation)

			// 3.b) Using the BsonClassMap and setting elements using ClassMapInitializers
            // Also, this can be done directly in the code w/ BsonClassMap to register that class
            // RegisterClassMap() is some kind of global registry

			// 3.c) Using conventions
			// Create the ConventionPack class where you add a convention (e.g. camelCase)
			// Add ConventionPack to the ConventionRegistry

			
			// 3.c)
			var conventionPack = new ConventionPack();
			conventionPack.Add( new CamelCaseElementNameConvention() );
			// Applies camelCase convention to all variables (t => true)
			ConventionRegistry.Register( "camelCase", conventionPack, t => true );


			//// 3.b)
			//BsonClassMap.RegisterClassMap<Person>( cm =>
			//{
			//	cm.AutoMap();
			//	// 3.b)
			//	cm.MapMember( x => x.Name ).SetElementName( "name" );
			//} );


            var person = new Person
            {
                // Id is not initialized, the it is set by default by MongoDB
                Name = "Jones",
                Age = 30,
                Colors = new List<string> { "red", "blue" },
                Pets = new List<Pet> { new Pet { Name = "Fluffy", Type = "Pig" } },
                ExtraElements = new BsonDocument( "anotherName", "anotherValue")
            } ;

            // This happens in MongoDB under the covers, but we explicitly write it so
            // it can be observed in the console
            using ( var writer = new JsonWriter( Console.Out ) )
            {
                BsonSerializer.Serialize( writer, person );
            }

        }

        class Person
        {
            public ObjectId Id { get; set; }

			// 3.a)
			//[BsonElement("name")]   
            public string Name { get; set; }

			// 3.a)
			//[BsonRepresentation (BsonType.String)]
            public int Age { get; set; }
            
            public List<string> Colors { get; set; }

            public List<Pet> Pets { get; set; }

            public BsonDocument ExtraElements { get; set; }
        }

        public class Pet
        {
            public string Name  { get; set; }

            public string Type  { get; set; }
        }
    }

}
