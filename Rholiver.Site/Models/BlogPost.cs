using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rholiver.Site.Models
{
    public class BlogPost
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public HtmlString Body { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedAt { get; set; }
    }
}