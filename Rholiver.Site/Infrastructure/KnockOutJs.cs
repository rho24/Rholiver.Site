using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rholiver.Site.Infrastructure
{
    public static class KnockOutJs
    {
        public static IDictionary<string, object> Attr(string dataBind)
        {
            return new Dictionary<string, object> { { "data-bind", dataBind } };
        }
    }
}