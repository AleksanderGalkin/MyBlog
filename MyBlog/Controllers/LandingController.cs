﻿using MyBlog.Infrustructure;
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
            return View("Index");
        }

        // POST: Landing/Create
        [HttpPost]
        public ActionResult Create (LandingViewModel Model) //(FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = Model.Email;
                _unitOfWork.db.Users.Add(user);
                return RedirectToAction("Index");
            }
            return View("Index",Model);
        }


    }
}
