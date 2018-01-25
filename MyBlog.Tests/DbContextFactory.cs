using MyBlog.Infrustructure;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBlog.Models;
using NSubstitute;

namespace MyBlog.Tests
{
    public static class DbContextFactory
    {

        private static IDbContext _context;

        
        static IList<Post> Posts;
        static IList<PostContent> PostContents ;
        static IList<PostView> PostViews  ;
        static IList<PostComment> PostComments ;
        static IList<PostTag> PostTags  ;
        static IList<Tag> Tags ;


        public static IDbContext context
        {
            get
            {

                if (_context != null)
                    return _context;
                else
                    throw new InvalidOperationException("Context not available");
            }
        }

        public static void SetContext ()
        {
            _context = Substitute.For<IDbContext>();

            //IQueryable<Post> Posts = new List<Post>().AsQueryable();
            //IQueryable<PostContent> PostContents = new List<PostContent>().AsQueryable();
            //IQueryable<PostView> PostViews = new List<PostView>().AsQueryable();
            //IQueryable<PostComment> PostComments = new List<PostComment>().AsQueryable();
            //IQueryable<PostTag> PostTags = new List<PostTag>().AsQueryable();
            //IQueryable<Tag> Tags = new List<Tag>().AsQueryable();

            Posts = new List<Post>();
            PostContents = new List<PostContent>();
            PostViews = new List<PostView>();
            PostComments = new List<PostComment>();
            PostTags = new List<PostTag>();
            Tags = new List<Tag>();

            var mockPosts = Substitute.For<DbSet<Post>, IQueryable<Post>>().GetDbSet(Posts.AsQueryable());
            var mockPostContent = Substitute.For<DbSet<PostContent>, IQueryable<PostContent>>().GetDbSet(PostContents.AsQueryable());
            var mockPostView = Substitute.For<DbSet<PostView>, IQueryable<PostView>>().GetDbSet(PostViews.AsQueryable());
            var mockPostComment = Substitute.For<DbSet<PostComment>, IQueryable<PostComment>>().GetDbSet(PostComments.AsQueryable());
            var mockPostTag = Substitute.For<DbSet<PostTag>, IQueryable<PostTag>>().GetDbSet(PostTags.AsQueryable());
            var mockTag = Substitute.For<DbSet<Tag>, IQueryable<Tag>>().GetDbSet(Tags.AsQueryable());

            _context.Posts.Returns(mockPosts);
            _context.PostContents.Returns(mockPostContent);
            _context.PostViews.Returns(mockPostView);
            _context.PostComments.Returns(mockPostComment);
            _context.PostTags.Returns(mockPostTag);
            _context.Tags.Returns(mockTag);

            _context.Posts.WhenForAnyArgs(x => x.Add(Arg.Any<Post>()))
                .Do(x => {
                    Posts.Add(x[0] as Post);
                    foreach(var i in (x[0] as Post).PostContents)
                    {
                        PostContent newPostContent = new PostContent();
                        newPostContent.PostId = (x[0] as Post).PostId;
                        newPostContent = i;
                        PostContents.Add(newPostContent);
                    }
                    mockPosts = Substitute.For<DbSet<Post>, IQueryable<Post>>().GetDbSet(Posts.AsQueryable<Post>());
                    mockPostContent = Substitute.For<DbSet<PostContent>, IQueryable<PostContent>>().GetDbSet(PostContents.AsQueryable<PostContent>());

                });
            _context.PostContents.WhenForAnyArgs(x => x.Add(Arg.Any<PostContent>()))
                .Do(x => {
                    PostContents.Add(x[0] as PostContent);
                    mockPostContent = Substitute.For<DbSet<PostContent>, IQueryable<PostContent>>().GetDbSet(PostContents.AsQueryable());
                });
            _context.PostViews.WhenForAnyArgs(x => x.Add(Arg.Any<PostView>()))
                .Do(x => {
                    PostViews.Add(x[0] as PostView);
                    mockPostView = Substitute.For<DbSet<PostView>, IQueryable<PostView>>().GetDbSet(PostViews.AsQueryable());
                });
            _context.PostComments.WhenForAnyArgs(x => x.Add(Arg.Any<PostComment>()))
                .Do(x => {
                    PostComments.Add(x[0] as PostComment);
                    mockPostComment = Substitute.For<DbSet<PostComment>, IQueryable<PostComment>>().GetDbSet(PostComments.AsQueryable());
                });
            _context.PostTags.WhenForAnyArgs(x => x.Add(Arg.Any<PostTag>()))
                .Do(x => {
                    PostTags.Add(x[0] as PostTag);
                    mockPostTag = Substitute.For<DbSet<PostTag>, IQueryable<PostTag>>().GetDbSet(PostTags.AsQueryable());
                });
            _context.Tags.WhenForAnyArgs(x => x.Add(Arg.Any<Tag>()))
                .Do(x => {
                    Tags.Add(x[0] as Tag);
                    mockTag = Substitute.For<DbSet<Tag>, IQueryable<Tag>>().GetDbSet(Tags.AsQueryable());
                });


            _context.Posts.WhenForAnyArgs(x => x.Remove(Arg.Any<Post>()))
               .Do(x => {
                   Posts.Remove(x[0] as Post);
                   mockPosts = Substitute.For<DbSet<Post>, IQueryable<Post>>().GetDbSet(Posts.AsQueryable());
               });

        }

        private static IDbSet<T> GetDbSet<T>(this IDbSet<T> dbSet, IQueryable<T> data)
            where T: class
        {

            ((IQueryable<T>)dbSet).Provider.Returns(data.Provider);
            ((IQueryable<T>)dbSet).Expression.Returns(data.Expression);
            ((IQueryable<T>)dbSet).ElementType.Returns(data.ElementType);
            ((IQueryable<T>)dbSet).GetEnumerator().Returns(data.GetEnumerator());
            ((IQueryable<T>)dbSet).GetEnumerator().Reset();



            return dbSet;
        }
            

    }

   
}
