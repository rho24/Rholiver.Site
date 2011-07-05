using System;

namespace Rholiver.Site.Infrastructure
{
    public static class GeneralExtensions
    {
        public static string Fmt(this string format, params object[] args) {
            return String.Format(format, args);
        }
    }
}