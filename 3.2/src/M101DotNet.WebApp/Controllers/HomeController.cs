using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using M101DotNet.WebApp.Models;
using M101DotNet.WebApp.Models.Home;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace M101DotNet.WebApp.Controllers
{
    public class HomeController : Controller
    {
		public async Task<ActionResult> Index()
		{
			var blogContext = new BlogContext();
			// XXX WORK HERE
			// find the most recent 10 posts and order them
			// from newest to oldest

			var filter = new BsonDocument();
			var posts = blogContext.Posts;
			var num = await posts.CountAsync( filter );
			var allPosts = await blogContext.Posts.Find( filter ).ToListAsync();
			var last10Posts = allPosts.OrderBy( x => x.CreatedAtUtc ).Take( 10 ).ToList();

			var model = new IndexModel()
			{
				RecentPosts = last10Posts
			};
			return View( model );
		}

        [HttpGet]
        public ActionResult NewPost()
        {
            return View(new NewPostModel());
        }

        [HttpPost]
        public async Task<ActionResult> NewPost(NewPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var blogContext = new BlogContext();
            // XXX WORK HERE
            // Insert the post into the posts collection
			Post post = new Post()
				{
					//Id = Id ,
					//Comments = new Comment(),
					Author = "",
					CreatedAtUtc = DateTime.Now,
					//Tags = model.Tags,
					Title = model.Title
				};

			await blogContext.Posts.InsertOneAsync( post );
            return RedirectToAction( "Post", new { id = post.Id });
        }

		[HttpGet]
		public async Task<ActionResult> Post( string id )
		{
			var blogContext = new BlogContext();

			// XXX WORK HERE
			// Find the post with the given identifier

			//if ( post == null )
			//{
			//	return RedirectToAction( "Index" );
			//}

			//var model = new PostModel
			//{
			//	Post = post
			//};

			List<Post> allPosts = await blogContext.Posts.Find( new BsonDocument() ).ToListAsync();

			var selectedPost = allPosts.Where( x => x.Id.ToString().Equals( id ) ).SingleOrDefault();

			var commentArray = selectedPost.Comments.ToList();

			List<Comment> commentList = new List<Comment>();

			BsonArray bsonArr = new BsonArray();

			foreach (var comment in commentArray)
			{
				//BsonDocument bsonComment = comment.AsBsonDocument;

				//BsonDocument bsonDoc = new BsonDocument();

				//String author = String.Empty;
				//String content = String.Empty;
				//DateTime createdAtUtc = new DateTime();
				//foreach ( var element in bsonComment )
				//{
				//	switch ( element.Name )
				//	{
				//		case "Author":
				//			author = element.Value.AsString;
				//			break;
				//		case "Content":
				//			content = element.Value.AsString;
				//			break;
				//		case "CreatedAtUtc":
				//			createdAtUtc = element.Value.AsDateTime;
				//			break;
				//		default:
				//			break;
				//	}

				//	bsonDoc.Add( element );
				//}

				//Comment currentComment = new Comment( author, content, createdAtUtc );

				//commentList.Add( currentComment );
				//bsonArr.Add( bsonDoc );
		 
			}



			var model = new PostModel()
			{
				Post = new Post()
				{
					Id = selectedPost.Id,
					Author = selectedPost.Author,
					Content = selectedPost.Content,
					Title = selectedPost.Title,
					CreatedAtUtc = selectedPost.CreatedAtUtc,
					//Comments = bsonArr,
					Comments = commentList,
					Tags = selectedPost.Tags
				},
				NewComment = new NewCommentModel()
				{
				
					PostId = selectedPost.Id.ToString(),
					Content = selectedPost.Content
				}
			};
			return View( model );
		}

		[HttpGet]
		public async Task<ActionResult> Posts( string tag = null )
		{
			var blogContext = new BlogContext();

			// XXX WORK HERE
			// Find all the posts with the given tag if it exists.
			// Otherwise, return all the posts.
			// Each of these results should be in descending order.
			var posts = blogContext.Posts.Find( x => x.Tags.Equals( tag ) ).ToListAsync();

			return View( posts );
		}

		[HttpPost]
		public async Task<ActionResult> NewComment( NewCommentModel model )
		{
			if ( !ModelState.IsValid )
			{
				return RedirectToAction( "Post", new { id = model.PostId } );
			}

			var blogContext = new BlogContext();
			// XXX WORK HERE
			// add a comment to the post identified by model.PostId.
			// you can get the author from "this.User.Identity.Name"
			var foundPost = blogContext.Posts.Find( x => x.Id.ToString().Equals( model.PostId) );
			

			return RedirectToAction( "Post", new { id = model.PostId } );
		}
    }
}