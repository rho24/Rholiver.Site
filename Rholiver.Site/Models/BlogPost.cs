using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rholiver.Site.Models
{
    public class BlogPost
    {
        public virtual string Id { get; set; }
        
        [Required]
        public virtual string Title { get; set; }
        
        [Required]
        public virtual string Description { get; set; }
        
        [Required]
        [DataType(DataType.MultilineText)]
        public virtual string Body { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime CreatedAt { get; set; }
    }
}