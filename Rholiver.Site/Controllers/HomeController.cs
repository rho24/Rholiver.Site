﻿using System;
using System.Web.Mvc;

namespace Rholiver.Site.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index() {
            return View();
        }


        //
        // GET: /About/

        public ActionResult About() {
            return View();
        }
    }
}