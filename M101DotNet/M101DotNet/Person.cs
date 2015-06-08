using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace M101DotNet
{
	class Person
	{
		public ObjectId Id { get; set; }

		public string Name { get; set; }

		public int Age { get; set; }

		public string Profession { get; set; }

		public override string ToString()
		{
 			string objectString = "";

			objectString += "{ ";
			objectString += "Id: " + Id;
			objectString += " }\t";

			objectString += "{ ";
			objectString += "Name: " + Name;
			objectString += " }\t";

			objectString += "{ ";
			objectString += "Age: " + Age;
			objectString += " }\t";

			objectString += "{ ";
			objectString += "Profession: " + Profession;
			objectString += " }\t";

			return objectString;

		}

		//public List<string> Colors { get; set; }

		//public List<Pet> Pets { get; set; }

		//public BsonDocument ExtraElements { get; set; }
	}
}
