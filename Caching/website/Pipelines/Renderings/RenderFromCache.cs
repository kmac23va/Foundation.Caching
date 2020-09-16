using System.IO;
using Sitecore.Abstractions;
using Sitecore.Caching;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.Mvc.Presentation;
using Sitecore.Sites;

namespace Foundation.Caching.Pipelines.Renderings {
    public class RenderFromCache : Sitecore.Mvc.Pipelines.Response.RenderRendering.RenderFromCache {
        public RenderFromCache(RendererCache rendererCache, BaseClient baseClient) : base(rendererCache, baseClient) {
        }

        protected override bool Render(string cacheKey, TextWriter writer, RenderRenderingArgs args) {
            //Check if the Timeout key is in the cache key; if not, try the base/Sitecore version
            if (!cacheKey.Contains(Settings.TimeoutRenderingKey)) {
                return base.Render(cacheKey, writer, args);
            }

            //Check if the caching site exists; if not, try the base/Sitecore version
            SiteContext cachingSite = SiteContext.GetSite(Settings.CachingSite);

            if (cachingSite == null) {
                return base.Render(cacheKey, writer, args);
            }

            //Check if an HTML cache exists for the caching site and that the HTML is present; if not, try the base/Sitecore version
            HtmlCache htmlCache = CacheManager.GetHtmlCache(cachingSite);
            string html = htmlCache?.GetHtml(cacheKey);

            if (html == null) {
                return base.Render(cacheKey, writer, args);
            }

            //If everything is good, write the HTML from the cache
            writer.Write(html);
            
            return true;
        }
    }
}
