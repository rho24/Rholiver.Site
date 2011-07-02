using System;
using System.Linq;
using System.Web.Mvc;
using PocoDb;
using Rholiver.Site.Infrastructure;
using Rholiver.Site.Models;

namespace Rholiver.Site.Controllers
{
    public class BlogController : Controller
    {
        public PocoDbClient PocoDb { get; private set; }

        public BlogController(PocoDbClient pocoDb)
        {
            PocoDb = pocoDb;
        }

        //
        // GET: /Blog/

        public ActionResult Index()
        {
            using (var session = PocoDb.BeginSession())
            {
                var posts = session.Get<BlogPost>().ToList();
                return View(posts);
            }
        }

        public ActionResult Post(string id)
        {
            using (var session = PocoDb.BeginSession())
            {
                var post = session.Get<BlogPost>().Where(p => p.Id == id).FirstOrDefault();

                if (post == null)
                    return new HttpNotFoundResult();

                return View(post);
            }
        }

        [RequiresAuthorization]
        public ActionResult CreatePost()
        {
            return View(new BlogPost());
        }

        [HttpPost, RequiresAuthorization, ValidateInput(false)]
        public ActionResult CreatePost(BlogPost post)
        {
            if (!ModelState.IsValid)
                return View(post);

            using (var session = PocoDb.BeginWritableSession())
            {
                if (session.Get<BlogPost>().Where(p => p.Title == post.Title).FirstOrDefault() != null)
                    ModelState.AddModelError("Title", "Title is not unique");

                post.Id = post.Title.Replace(" ", "-");

                session.Add(post);

                session.SaveChanges();
            }
            
            return RedirectToAction("EditPost", new {Id = post.Id});
        }

        [RequiresAuthorization]
        public ActionResult EditPost(string id)
        {
            using (var session = PocoDb.BeginSession())
            {
                var post = session.Get<BlogPost>().Where(p => p.Id == id).FirstOrDefault();

                if (post == null)
                    return new HttpNotFoundResult();

                return View(post);
            }
        }
    }
}