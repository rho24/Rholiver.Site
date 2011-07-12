using System;
using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;

namespace Rholiver.Site.Infrastructure
{
    public static class GeneralExtensions
    {
        public static string Fmt(this string format, params object[] args) {
            return String.Format(format, args);
        }
        
        public static IEnumerable<T> MapToPocos<T>(this IEnumerable<T> objs) where T : new() {
            return objs.Select(o => (T) (new T().InjectFrom(o)));
        }
    }
}