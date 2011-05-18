using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rholiver.Site.Models;
using Rholiver.Site.Infrastructure;

namespace Rholiver.Site.Controllers
{
    public class BlogController : Controller
    {
        public BlogRepository BlogRepo { get; set; }

        public BlogController(BlogRepository blogRepo)
        {
            BlogRepo = blogRepo;
        }

        //
        // GET: /Blog/

        public ActionResult Index()
        {
            return View(BlogRepo.Posts.OrderByDescending(b => b.CreatedAt));
        }

        public ActionResult Post(string id)
        {
            var blog = BlogRepo.Posts.Where(p => p.Id == id).FirstOrDefault();

            if (blog == null)
                return new HttpNotFoundResult();

            return View(blog);
        }


        public ActionResult CreatePost()
        {
            return View(new BlogPost());
        }

        [HttpPost]
        public ActionResult CreatePost(BlogPost post)
        {
            if (post == null)
                throw new ArgumentNullException("post");

            if (string.IsNullOrWhiteSpace(post.Title))
                throw new ArgumentNullException("post.Title");

            if (BlogRepo.Posts.Where(p => p.Title == post.Title).FirstOrDefault() != null)
                throw new ArgumentException("Post not unique");

            post.Id = post.Title.Replace(" ", "-");

            BlogRepo.Posts.Add(post);

            return RedirectToAction("EditPost", new { Id = post.Id });
        }

        public ActionResult EditPost(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException("id");

            var post = BlogRepo.Posts.Where(p => p.Title == id).FirstOrDefault();

            if (post == null)
                return HttpNotFound();

            return View(post);
        }
    }
}
