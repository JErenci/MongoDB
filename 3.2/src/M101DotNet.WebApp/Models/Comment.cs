using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace M101DotNet.WebApp.Models
{
    public class Comment
    {
        // XXX WORK HERE
        // Add in the appropriate properties.
        // The homework instructions have the
        // necessary schema.
		public string Author
		{
			get;
			set;
		}

		public String Content
		{
			get;
			set;
		}

		public DateTime CreatedAtUtc
		{
			get;
			set;
		}

		[BsonConstructor]
		public Comment( String author, string content, DateTime createAtUtc )
		{
			Author = author;
			Content = content;
			CreatedAtUtc = createAtUtc;
		}
    }
}