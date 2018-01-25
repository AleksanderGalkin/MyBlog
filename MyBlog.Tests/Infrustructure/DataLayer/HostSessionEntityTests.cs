using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlog.Infrustructure.DataLayer;
using MyBlog.Models;
using MyBlog.Tests;
using MyBlogContract;
using MyBlogContract.SessionEntity;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Infrustructure.DataLayer.Tests
{
    [TestClass()]
    public class HostSessionEntityTests
    {
        IUnitOfWork UoW;
        HostSessionEntity<IDsTag> host;

        IList<IDsTag> post_tags;

        [TestInitialize]
        public void Init()
        {
            UoW = Substitute.For<IUnitOfWork>();
            DbContextFactory.SetContext();
            //IDbContext context = DbContextFactory.context;
            DbContextFillFactory.SetDbContext(DbContextFactory.context);
            //DbContextFillFactory.SetPostData();
                        IDbContext context = DbContextFillFactory.CurrentContext;


            ((IUnitOfWork)UoW).db.Returns(context);

            post_tags = new List<IDsTag>();
            IDsTag newTag = new DsTag();
            newTag.TagId = 1;
            newTag.TagName = "old name";
            post_tags.Add(newTag);

        }

        /// <summary>
        /// Возвращает ли количество записей в ожидаемом количестве
        /// </summary>
        //[TestMethod()]
        //public void getEntitySetTest()
            
        //{
        //    DbContextFillFactory.SetPostData();

        //    var uow_entity = UoW.db.Posts.Select(x=>x).ToList();

        //    IList<Post> lPosts = uow_entity.Select(z => z).ToList();

        //    host = new HostSessionEntity<Post>(uow_entity);

        //    IList<Post> entity = host.getEntitySet();
            
        //    int lPosts_count = lPosts.Select(x => x).ToList().Count;
        //    int entity_count = entity.Select(x => x).ToList().Count;
        //    Assert.AreEqual(lPosts_count, entity_count);

        //    var entity_list = entity.Select(x => x).ToList();
        //    for (int i= 0; i < lPosts_count; i++)
        //    {
        //        Assert.AreEqual(lPosts[i], entity_list[i]);

        //    }


        //    uow_entity = UoW.db.Posts.Select(x => x).ToList();

        //    lPosts = uow_entity.Select(z => z).ToList();

        //    uow_entity.Remove(entity.Where(x => x.PostId == 1).Select(x => x).SingleOrDefault());

        //    host = new HostSessionEntity<Post>(uow_entity);
        //    entity = host.getEntitySet();

        //    lPosts_count = lPosts.Select(x => x).ToList().Count;
        //    entity_count = entity.Select(x => x).ToList().Count;
        //    Assert.AreNotEqual(lPosts_count, entity_count);

        //}

        //[TestMethod()]
        //public void getPluginSessionEntityTest()
        //{
        //    var uow_entity = UoW.db.Posts.Select(x => x).ToList();

        //    host = new HostSessionEntity<Post>(uow_entity);

        //    var plugin = host.getPluginSessionEntity();
        //    Assert.IsInstanceOfType(plugin, typeof(IPluginSessionEntity<Post>));
        //}

        [TestMethod()]
        public void updateTest()
        {

            //DbContextFillFactory.SetDbContext(UoW.db);
            //DbContextFillFactory.SetPostData();

            //var uow_entity = UoW.db.Posts.Select(x => x).ToList();

            //IList<Post> before_posts = uow_entity.Select(z => z).ToList();
            host = new HostSessionEntity<IDsTag>(post_tags);

            host.setKeyName("TagId");
            IPluginSessionEntity<IDsTag> pluginEntity =  host.getPluginSessionEntity();
            IDsTag newTag = new DsTag();
            newTag.TagId = 1;
            newTag.TagName = "Modified by union test";
            pluginEntity.modify(newTag);
            host.update(pluginEntity);
            IList<IDsTag> after = host.getEntitySet();

            string before_title = post_tags.Where(x => x.TagId == 1).Select(x => x.TagName).SingleOrDefault();
            string after_title = after.Where(x => x.TagId == 1).Select(x => x.TagName).SingleOrDefault();

            Assert.AreEqual(before_title, after_title);
            Assert.AreEqual("Modified by union test", after_title);



        }


    }
}