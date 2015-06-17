using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace HW3
{
	public class Student
	{
		public double _id { get; set; }				// double
		public string name { get; set; }			// string

		public List<Scores> scores { get; set; }	// BsonArray

		public override string ToString()
		{
			string studentString = String.Empty;
			string scoreString = String.Empty;

			//foreach ( var item in scores )
			//{
			//	scoreString += "type: " + item.type + ", score: " + item.score;
			//}

			//studentString += "_id: " + _id + ", scores: " + scoreString;

			return studentString;
		}
	}
}
