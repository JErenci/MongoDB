using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Homework2_2
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
			var db = client.GetDatabase( "students" );
			var col = db.GetCollection<BsonDocument>( "grades" );

			var cursor = col.Find( new BsonDocument() ).ToCursorAsync();
			var list = await col.Find( new BsonDocument() ).ToListAsync();


			var colGrade = db.GetCollection<Grade>( "grades" );
			var gradeBuilder = Builders<Grade>.Filter;

			var noFilter = gradeBuilder.Gt( x => x.score, 0 );
			var listGrade = await colGrade.Find<Grade>( noFilter )
				.SortBy( x => x.student_id ).ThenBy( x => x.score )
				.ToListAsync();

			var gradeFilter = gradeBuilder.Eq<String>( x => x.type, "homework" );
			var listGradeHomework = await colGrade.Find<Grade>( gradeFilter )
				.SortBy( x => x.student_id).ThenBy( x=> x.score)
				.ToListAsync();

			//foreach ( var grad in list )
			//{
			//	Console.WriteLine( grad.ToString() );
			//}

			int currentStudentId = -1;
			var worstGradesRemoved = listGrade;

			List<Grade> discardedGradeList = new List<Grade>();
			foreach ( Grade grade in listGradeHomework )
			{
				if ( currentStudentId != grade.student_id )
				{
					Grade gradeInRemaining = worstGradesRemoved.Where ( x => x._id == grade._id ).FirstOrDefault();
					worstGradesRemoved.Remove( gradeInRemaining );

					discardedGradeList.Add( grade );
					currentStudentId = grade.student_id;
				}
			}


			List<Grade> orderedList = worstGradesRemoved.OrderByDescending( x => x.score ).ToList();
			Grade grade101 = orderedList[100];

			Console.WriteLine( "Number of grades: " + grade101.ToString() );

			List<Grade> topFive = worstGradesRemoved.OrderBy( x => x.student_id ).ThenBy( x => x.score ).Take(5).ToList();

			int counter = 1;
			foreach ( var element in worstGradesRemoved )
			{
				Console.WriteLine( "Grade #" + counter.ToString() + ": " + element.ToString() );
				counter++;
			}

			int studentId = 0;
			double studentSum = 0.0;
			double average = 0;
			List<averageStudent> averageGrades = new List<averageStudent>();
			
			
			foreach ( var student in worstGradesRemoved )
			{
				if ( studentId == student.student_id )
				{
					studentSum += student.score;
				}
				else
				{
					average = studentSum / 3;
					averageGrades.Add( new averageStudent
					{

						studentId = studentId, 
						averageScore = average 
					} );
					studentId++;
					studentSum = student.score;
				}
			}

			var orderAverageGrades = averageGrades.OrderBy( x => x.averageScore).ToList();

			foreach ( var item in orderAverageGrades )
			{
				Console.WriteLine( item.ToString() );
			}
		}
	}

	public class averageStudent
	{
		public int studentId;
		public double averageScore;

		public string ToString()
		{
			string gradeString = String.Empty;

			gradeString += " student_id: " + studentId +
				", avgScore: " + averageScore;
			return gradeString;
		}
	}
	public class Grade
	{
		public ObjectId _id {get;set;} 
		public int student_id { get; set; }
		public string type { get; set; }
		public double score { get; set; }

		public string ToString()
		{
			string gradeString = String.Empty;

			gradeString += "_id: " + _id + ", student_id: " + student_id + 
				", type: " + type + ", score: " + score;
			return gradeString;
		}
	}
}
