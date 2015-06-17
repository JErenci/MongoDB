using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace HW3
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
			var connectionString = "mongodb://localhost:27017";
			var client = new MongoClient( connectionString );
			var db = client.GetDatabase( "school" );
			var colStudent = db.GetCollection<Student>( "students" );
			var colBson = db.GetCollection<BsonDocument>( "students" );
			var studentBuilder = Builders<Student>.Filter;

			var document1 = new BsonDocument {
				{ "author", "joe" },
				{ "title", "yet another blog post" },
				{ "text", "here is the text..." },
				{ "tags", new BsonArray { "example", "joe" } },
				{ "comments", new BsonArray {
					new BsonDocument { { "author", "jim" }, { "comment", "I disagree" } },
					new BsonDocument { { "author", "nancy" }, { "comment", "Good post" } }
				}}
			};

			//MongoCursor 
			//var cursor = col.Find( new Student() ).ToCursorAsync();
			//var lista1 = await col.Find( new BsonDocument() ).
			//	Project( x => new { x.Name, x.score } ).
			//	ToListAsync();



			//var noFilter = studentBuilder.Gt( x => x.name, "0" );
			//var listGrade = await colStudent.Find<Student>( noFilter )
			//	.ToListAsync();


			var list = await colBson.Find( new BsonDocument() ).ToListAsync();


			var filter = new BsonDocument();


			List<Student> studentClass = new List<Student>();

			//var count = 0;
			using ( var cursor = await colBson.FindAsync( filter ) )
			{
				while ( await cursor.MoveNextAsync() )
				{
					var bsonDocumentCollection = cursor.Current;

					foreach ( var document in bsonDocumentCollection )
					{
						double docId = document["_id"].AsDouble;
						string docName = document["name"].AsString;
						BsonArray docScores = document["scores"].AsBsonArray;
						List<BsonValue> docScoresList = docScores.ToList<BsonValue>(); // .Cast<Scores>;

						List<Scores> scoreList = new List<Scores>();

						foreach ( var currentScore in docScoresList )
						{
							scoreList.Add( new Scores(){
								type = currentScore["type"].AsString,
								score = currentScore["score"].AsDouble
							});
						}


						Student newStudent = new Student()
						{
							_id = docId,
							name = docName,
							scores = scoreList
						};
						studentClass.Add( newStudent );
					}
				}
			}


			//foreach ( var doc in list )
			//{
			//	Console.WriteLine( doc );
			//}

			//foreach ( var elem in lista )
			//{
			//	if ( elem.Email == model.Email )
			//		user.Name = elem.Name;
			//}

			foreach ( var student in studentClass )
			{
				IEnumerable<Scores> hwScores = student.scores.Where( x => x.type == "homework");
				double minHwScore = 100.0;
				Scores worstHw = new Scores();

				foreach ( var score in hwScores )
				{
					if ( score.score < minHwScore )
					{
						minHwScore = score.score;
						worstHw = score;
					}
				}

				student.scores.Remove( worstHw );
			}

			Dictionary<double, double> studentMeanGrades = new Dictionary<double,double>();
			foreach ( var student in studentClass )
			{
				double sum = 0.0;
				foreach ( var grade in student.scores )
				{
					sum += grade.score;
				}
				double meanGrade = sum/3;
				studentMeanGrades.Add(student._id, meanGrade);
			}
			List<KeyValuePair<double,double>> orderedList = studentMeanGrades.OrderBy( x => x.Value ).ToList();
			//orderedList[200]
			Console.WriteLine( "End" );
		}
	}


	public class Grade
	{
		public ObjectId _id { get; set; }
		public int student_id { get; set; }
		public string type { get; set; }
		public double score { get; set; }

		public override string ToString()
		{
			string gradeString = String.Empty;

			gradeString += "_id: " + _id + ", student_id: " + student_id +
				", type: " + type + ", score: " + score;
			return gradeString;
		}
	}
}
