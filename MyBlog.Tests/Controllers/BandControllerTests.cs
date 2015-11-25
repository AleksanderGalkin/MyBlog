using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyBlog.Controllers;
using MyBlog.Infrustructure;
using MyBlog.Infrustructure.Windsor;
using MyBlog.Models;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyBlog.Tests.Controllers
{
    [TestClass()]
    public class BandControllerTests
    {
        private static IWindsorContainer container;

        private Mock<IDbContext> mockIDbContext;





        



        [TestInitialize]
        public void Init()
        {
            container = new WindsorContainer().Install(FromAssembly.InThisApplication());
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            mockIDbContext = new Mock<IDbContext>();

            List<Post> listPost = new List<Post>();
            List<PostContent> listPostContent = new List<PostContent>();
            List<PostView> listPostView = new List<PostView>();
            List<PostComment> listPostComment = new List<PostComment>();
            List<PostTag> listPostTag = new List<PostTag>();
            List<Tag> listTag = new List<Tag>();

            Mock<IDbSet<Post>> mockIDbSetPost;
            Mock<IDbSet<PostContent>> mockIDbSetPostContent;
            Mock<IDbSet<PostView>> mockIDbSetPostView;
            Mock<IDbSet<PostComment>> mockIDbSetPostComment;
            Mock<IDbSet<PostTag>> mockIDbSetPostTag;
            Mock<IDbSet<Tag>> mockIDbSetTag;

            ApplicationUser user = new ApplicationUser();

            Post post = new Post() {
                ApplicationUserId = "qweqw"
                , PostId = 1
                , PubDate = DateTime.Now
                , RowVersion = new byte[] { 1, 2 }
                , Tittle = "тестовый пост"
            };
            listPost.Add(post);
            PostContent postContent = new PostContent() {
                 PostContentId = 1
                , Post = post
                , ContentData = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }
                , ContentDataType = ContentDataTypes.Text
                , LikeMinus = 1
                , LikePlus = 3
                , Comment = "Комментарий"
                , RowVersion = new byte[] { 1, 2 }
            };
            listPostContent.Add(postContent);
            PostView postView = new PostView(){
                 PostViewId = 1
                 , Post = post
                 , Date = DateTime.Now
                 , Ip = "192.168.0.1"
                 , RowVersion = new byte[] { 1, 2 }
            };
            listPostView.Add(postView);
            PostComment postComment = new PostComment() {
                 PostCommentId = 1
                 , Post = post
                 , Date = DateTime.Now
                 , Ip = "192.168.0.1"
                 , ApplicationUser = user
                 , RowVersion = new byte[] { 1, 2 }
            };
            listPostComment.Add(postComment);

            List<Tag> tags = new List<Tag> { new Tag { TagId = 1, TagName = "tag1", RowVersion = new byte[] { 1, 2 } } };

            PostTag postTag = new PostTag() {
                 Post = post
                 , TagId = 1
                 , RowVersion = new byte[] { 1, 2 }
            };

            listPostTag.Add(postTag);

            post.ApplicationUser = user;
            post.PostComments = listPostComment;
            post.PostContents = listPostContent;
            post.PostViews = listPostView;
            post.PostTags = listPostTag;

            mockIDbSetPost = new Mock<IDbSet<Post>>();
            mockIDbSetPostContent = new Mock<IDbSet<PostContent>>();
            mockIDbSetPostView = new Mock<IDbSet<PostView>>();
            mockIDbSetPostComment = new Mock<IDbSet<PostComment>>();
            mockIDbSetPostTag = new Mock<IDbSet<PostTag>>();
            mockIDbSetTag = new Mock<IDbSet<Tag>>();

            setDataForIDbSet(mockIDbSetPost, listPost.AsQueryable());
            setDataForIDbSet(mockIDbSetPostContent, listPostContent.AsQueryable());
            setDataForIDbSet(mockIDbSetPostView,listPostView.AsQueryable());
            setDataForIDbSet(mockIDbSetPostComment,listPostComment.AsQueryable());
            setDataForIDbSet(mockIDbSetPostTag,listPostTag.AsQueryable());
            setDataForIDbSet(mockIDbSetTag, listTag.AsQueryable());

            mockIDbContext.Setup(s => s.Posts).Returns(mockIDbSetPost.Object);
            mockIDbContext.Setup(s => s.PostContents).Returns(mockIDbSetPostContent.Object);
            mockIDbContext.Setup(s => s.PostViews).Returns(mockIDbSetPostView.Object);
            mockIDbContext.Setup(s => s.PostComments).Returns(mockIDbSetPostComment.Object);
            mockIDbContext.Setup(s => s.PostTags).Returns(mockIDbSetPostTag.Object);
            mockIDbContext.Setup(s => s.Tags).Returns(mockIDbSetTag.Object);


        }

        private void setDataForIDbSet<T>(Mock<IDbSet<T>> set, IQueryable<T> qdata)
            where T: class
        {
            set.Setup(s => s.Provider).Returns(qdata.Provider);
            set.Setup(s => s.Expression).Returns(qdata.Expression);
            set.Setup(s => s.ElementType).Returns(qdata.ElementType);
            set.Setup(s => s.GetEnumerator()).Returns(qdata.GetEnumerator());
        }

        [TestMethod()]
        public void IndexTest()
        {
            Mock<ControllerContext> mockControllerContext = new Mock<ControllerContext>();
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            Mock<ClaimsIdentity> mockIdentity = new Mock<ClaimsIdentity>();
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Author", "true"));
            mockIdentity.SetupGet(p => p.Claims).Returns(claims);
            mockPrincipal.SetupGet(p => p.Identity).Returns(mockIdentity.Object);
            mockControllerContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            BandController controller = new BandController(container.Resolve<IUnitOfWork>(new { DbContext = mockIDbContext.Object }));
            controller.ControllerContext = mockControllerContext.Object;
            ViewResult result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
            IEnumerable<PostVm> modelInView = result.Model as IEnumerable<PostVm>;
            Assert.AreEqual(1, modelInView.Count());
        }
        [TestMethod()]
        public void AuthorPartViewShown()
        {
            Mock<ControllerContext> mockControllerContext = new Mock<ControllerContext>();
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            Mock<ClaimsIdentity> mockIdentity = new Mock<ClaimsIdentity>();
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Author", "true"));
            mockIdentity.SetupGet(p => p.Claims).Returns(claims);
            mockPrincipal.SetupGet(p => p.Identity).Returns(mockIdentity.Object);
            mockControllerContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            BandController controller = new BandController(container.Resolve<IUnitOfWork>(new { DbContext = mockIDbContext.Object }));
            controller.ControllerContext = mockControllerContext.Object;
            ViewResult result = controller.AuthorControlCreate() as ViewResult;
            Assert.AreEqual("AuthorControlCreate", result.ViewName);
        }
        [TestMethod()]
        public void AuthorPartViewNotShown()
        {
            Mock<ControllerContext> mockControllerContext = new Mock<ControllerContext>();
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            Mock<ClaimsIdentity> mockIdentity = new Mock<ClaimsIdentity>();
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Author", "false"));
            mockIdentity.SetupGet(p => p.Claims).Returns(claims);
            mockPrincipal.SetupGet(p => p.Identity).Returns(mockIdentity.Object);
            mockControllerContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            BandController controller = new BandController(container.Resolve<IUnitOfWork>(new { DbContext = mockIDbContext.Object }));
            controller.ControllerContext = mockControllerContext.Object;
            ViewResult result = controller.AuthorControlCreate() as ViewResult;
            Assert.IsNull(result);
        }

    }
}
