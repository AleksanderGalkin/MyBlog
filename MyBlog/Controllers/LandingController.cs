using MyBlog.Infrustructure;
using MyBlog.Models;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class LandingController : AbstractController
    {
        public LandingController(IUnitOfWork UnitOfWork) :base(UnitOfWork)
        { }
        // GET: Landing
        public ActionResult Index()
        {
            return View();
        }

        // POST: Landing/Create
        [HttpPost]
        public ActionResult Create (LandingViewModel Model) //(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


    }
}
