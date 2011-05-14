using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rholiver.Site.Models
{
    public class BlogPost
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public HtmlString Content { get; set; }

        [Required]
        public DateTime Created { get; set; }
    }
}