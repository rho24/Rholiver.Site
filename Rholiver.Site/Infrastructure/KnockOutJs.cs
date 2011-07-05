using System;
using System.Collections.Generic;

namespace Rholiver.Site.Infrastructure
{
    public static class KnockOutJs
    {
        public static IDictionary<string, object> Attr(string dataBind) {
            return new Dictionary<string, object> {{"data-bind", dataBind}};
        }
    }
}