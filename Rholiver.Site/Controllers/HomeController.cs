using System;
using System.Web.Mvc;
using Rholiver.Site.Infrastructure;

namespace Rholiver.Site.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() {
            return View();
        }

        public ActionResult About() {
            return View();
        }
        
        [RequiresAuthorization]
        public ActionResult DashBoard() {
            return View();
        }
    }
}