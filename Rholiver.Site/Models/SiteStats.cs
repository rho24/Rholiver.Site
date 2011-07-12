using System;
using System.Collections.Generic;

namespace Rholiver.Site.Models
{
    public class SiteStats
    {
        public virtual ICollection<UpTime> Uptimes { get; set; }

        public SiteStats() {
            Uptimes = new List<UpTime>();
        }
    }

    public class UpTime
    {
        public virtual DateTime Start { get; set; }
        public virtual DateTime Finish { get; set; }
    }
}