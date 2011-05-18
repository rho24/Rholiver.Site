using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rholiver.Site.Models;

namespace Rholiver.Site.Infrastructure
{
    public class BlogRepository
    {
        public List<BlogPost> Posts { get; set; }

        public BlogRepository(List<BlogPost> posts)
        {
            Posts = posts;
        }
    }
}