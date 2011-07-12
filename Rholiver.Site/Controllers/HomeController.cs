using System;
using System.Linq;
using System.Web.Mvc;
using PocoDb;
using Rholiver.Site.Infrastructure;
using Rholiver.Site.Models;

namespace Rholiver.Site.Controllers
{
    public class HomeController : Controller
    {
        public PocoDbClient PocoDb { get; private set; }

        public HomeController(PocoDbClient pocoDb) {
            PocoDb = pocoDb;
        }

        public ActionResult Index() {
            return View();
        }

        public ActionResult About() {
            return View();
        }
        
        [RequiresAuthorization]
        public ActionResult DashBoard() {
            using (var session = PocoDb.BeginSession()) {
                var stats = session.Get<SiteStats>().MapToPocos().FirstOrDefault();
                return View(stats);
            }
        }
    }
}