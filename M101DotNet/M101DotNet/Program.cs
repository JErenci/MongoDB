using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
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

        static async Task MainAsync( string[] args )
        {
            //// *** BASICS *** ////

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

            // Creation a dictionary of keys,values
            var doc = new BsonDocument
            {
                { "name", "Jones" }
            };

            // Add method, takes a key and a "BsonValue" which maps value into their proper type
            doc.Add( "age", 30 );

            // Add using indexers
            doc["profession"] = "hacker";

            // Using BsonArrays
            var nestedArray = new BsonArray();
            nestedArray.Add( new BsonDocument( "color", "red" ) );

            // Adding the BsonArray to the BsonDocument
            doc.Add( "array", nestedArray );

            Console.WriteLine( doc );

            // Accessing the elements in the BsonDocument is also straightforward:
            Console.WriteLine( doc["array"][0]["color"] );
            
        }
    }
}
