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

        static async Task MainAsync( string [] args )
        {


            // *** BASICS *** ////

            // Talking to 2 servers in port 27017 running local
            var connectionString = "mongodb://localhost:27017";
            //var settings = new MongoClientSettings
            //{
            //}

            var client = new MongoClient( connectionString );

            // GetDatabase() has an override with the MongoClientSettings
            var db = client.GetDatabase( "test" );
            // GetCollection() has an override with the MongoClientSettings
            var col = db.GetCollection<BsonDocument>( "people" );



        }
    }
}
