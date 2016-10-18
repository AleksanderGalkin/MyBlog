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
        public static IDbContext DbContext {
            get
            {
                return context();
            }
        }

        private static IDbContext context()
        {
            IDbContext context = Substitute.For<IDbContext>();

            IQueryable<Post> Posts = new List<Post>().AsQueryable();
            IQueryable<PostContent> PostContents = new List<PostContent>().AsQueryable();
            IQueryable<PostView> PostViews = new List<PostView>().AsQueryable();
            IQueryable<PostComment> PostComments = new List<PostComment>().AsQueryable();
            IQueryable<PostTag> PostTags = new List<PostTag>().AsQueryable();
            IQueryable<Tag> Tags = new List<Tag>().AsQueryable();



            var mockPosts = Substitute.For<DbSet<Post>, IQueryable<Post>>().GetDbSet(Posts);
            var mockPostContent = Substitute.For<DbSet<PostContent>, IQueryable<PostContent>>().GetDbSet(PostContents);
            var mockPostView = Substitute.For<DbSet<PostView>, IQueryable<PostView>>().GetDbSet(PostViews);
            var mockPostComment = Substitute.For<DbSet<PostComment>, IQueryable<PostComment>>().GetDbSet(PostComments);
            var mockPostTag = Substitute.For<DbSet<PostTag>, IQueryable<PostTag>>().GetDbSet(PostTags);
            var mockTag = Substitute.For<DbSet<Tag>, IQueryable<Tag>>().GetDbSet(Tags);

            context.Posts.Returns(mockPosts);
            context.PostContents.Returns(mockPostContent);
            context.PostViews.Returns(mockPostView);
            context.PostComments.Returns(mockPostComment);
            context.PostTags.Returns(mockPostTag);
            context.Tags.Returns(mockTag);

            return context;
        }

        private static IDbSet<T> GetDbSet<T>(this IDbSet<T> dbSet, IQueryable<T> data)
            where T: class
        {

            ((IQueryable<T>)dbSet).Provider.Returns(data.Provider);
            ((IQueryable<T>)dbSet).Expression.Returns(data.Expression);
            ((IQueryable<T>)dbSet).ElementType.Returns(data.ElementType);
            ((IQueryable<T>)dbSet).GetEnumerator().Returns(data.GetEnumerator());

            return dbSet;
        }
            

    }

   
}
