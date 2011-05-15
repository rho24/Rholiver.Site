using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rholiver.Site.Models;

namespace Rholiver.Site.Controllers
{
    public class BlogController : Controller
    {
        public List<BlogPost> Blogs { get; set; }

        public BlogController()
        {
            Blogs = new List<BlogPost>()
            {
                new BlogPost(){
                    Id = "first-post",
                    Title = "First post",
                    Body = new HtmlString("<p>This is my first post!</p>"),
                    CreatedAt = new DateTime(2011, 05, 14, 19, 42, 00)
                },
                new BlogPost(){
                    Id = "second-post",
                    Title = "Second post",
                    Body = new HtmlString("<p>This is my second post.</p><p>Lets try <b>styling</b>.</p>"),
                    CreatedAt = new DateTime(2011, 05, 14, 20, 01, 00)
                }
            };
        }

        //
        // GET: /Blog/

        public ActionResult Index()
        {
            return View(Blogs.OrderByDescending(b => b.CreatedAt));
        }

        public ActionResult Post(string id)
        {
            var blog = Blogs.Where(b => b.Id == id).FirstOrDefault();

            if (blog == null)
                return new HttpNotFoundResult();

            return View(blog);
        }

    }
}
