using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace M101DotNet.WebApp.Models
{
	[BsonIgnoreExtraElements]
    public class Post
    {
        // XXX WORK HERE
        // add in the appropriate properties for a post
        // The homework instructions contain the schema.

		[BsonRepresentation( BsonType.ObjectId )]
		public String Id { get; set; }

		public String Author { get; set; }

		public String Title
		{
			get;
			set;
		}

		public String Content
		{
			get;
			set;
		}

		//[BsonRepresentation( BsonType.Array )]
		public List<String> Tags
		//public BsonArray Tags
		{
			get;
			set;
		}

		[BsonIgnore]
		public List<String> TagsList
		{
			get;
			set;
		}
		public DateTime CreatedAtUtc
		{
			get;
			set;
		}

		////[BsonRepresentation( BsonType.Array )]
		public List<Comment> Comments
		//public BsonArray Comments
		{
			get;
			set;
		}

		[BsonIgnore]
		public List<Comment> CommentsList
		{
			get;
			set;
		}
    }
}