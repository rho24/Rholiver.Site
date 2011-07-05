using System;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Rholiver.Site.Infrastructure
{
    public static class HtmlExtentions
    {
        public static MvcHtmlString KnockOutViewModel<T>(this HtmlHelper<T> htmlHelper) {
            var tagBuilder = new TagBuilder("script");
            tagBuilder.GenerateId("model");
            tagBuilder.Attributes.Add("type", "text/json");
            tagBuilder.InnerHtml = Json.Encode(htmlHelper.ViewData.Model);

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}