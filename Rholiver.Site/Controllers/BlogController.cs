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
        //
        // GET: /Blog/

        public ActionResult Index()
        {
            var blogs = new List<BlogPost>()
            {
                new BlogPost(){
                    Id = "first-post",
                    Title = "First post",
                    Content = new HtmlString("<p>This is my first post!</p>"),
                    Created = new DateTime(2011, 05, 14, 19, 42, 00)
                },
                new BlogPost(){
                    Id = "second-post",
                    Title = "Second post",
                    Content = new HtmlString("<p>This is my second post.</p><p>Lets try <b>styling</b>.</p>"),
                    Created = new DateTime(2011, 05, 14, 20, 01, 00)
                }
            };

            return View(blogs.OrderByDescending(b => b.Created));
        }

    }
}
