using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;


namespace Rholiver.Site.Infrastructure
{
    public static class HtmlExtentions
    {
        public static MvcHtmlString KnockOutViewModel<T>(this HtmlHelper<T> htmlHelper)
        {
            var tagBuilder = new TagBuilder("script");
            tagBuilder.GenerateId("model");
            tagBuilder.Attributes.Add("type", "text/json");
            tagBuilder.InnerHtml = Json.Encode(htmlHelper.ViewData.Model);

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}