using System;
using Sitecore.Caching;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.Sites;

namespace Foundation.Caching.Pipelines.Renderings {
    public class AddRecordedHtmlToCache : Sitecore.Mvc.Pipelines.Response.RenderRendering.AddRecordedHtmlToCache {
        public override void Process(RenderRenderingArgs args) {
            Assert.ArgumentNotNull(args, nameof(args));
            if (!args.Rendered) { return; }
            string cacheKey = args.CacheKey;
            if (!args.Cacheable || string.IsNullOrEmpty(cacheKey)) { return; }
            UpdateCache(cacheKey, args);
        }

        protected override void AddHtmlToCache(string cacheKey, string html, RenderRenderingArgs args) {
            //Check if the Timeout key is in the cache key; if not, try the base/Sitecore version
            if (!cacheKey.Contains(Settings.TimeoutRenderingKey)) {
                base.AddHtmlToCache(cacheKey, html, args);
                return;
            }

            //Check if the Timeout value is present and a valid TimeSpan; if not, try the base/Sitecore version
            bool timeoutValue = TimeSpan.TryParse(args.Rendering.RenderingItem.InnerItem[Settings.TimeoutRenderingParam], out _);

            if (!timeoutValue) {
                base.AddHtmlToCache(cacheKey, html, args);
                return;
            }

            //Check if the caching site exists; if not, try the base/Sitecore version
            SiteContext cachingSite = SiteContext.GetSite(Settings.CachingSite);

            if (cachingSite == null) {
                base.AddHtmlToCache(cacheKey, html, args);
                return;
            }

            //Check if the HTML cache is available in the caching site; if not, try the base/Sitecore version
            HtmlCache htmlCache = CacheManager.GetHtmlCache(cachingSite);

            if (htmlCache == null) {
                base.AddHtmlToCache(cacheKey, html, args);
                return;
            }

            //If everything is good, set the timeout based on the rendering parameter and put the HTML in the caching site
            TimeSpan timeout = GetTimeout(args);
            htmlCache.SetHtml(cacheKey, html, timeout);
        }

        protected override TimeSpan GetTimeout(RenderRenderingArgs args) => TimeSpan.TryParse(args.Rendering.RenderingItem.InnerItem[Settings.TimeoutRenderingParam], out TimeSpan result) ? result : args.Rendering.Caching.Timeout;
    }
}
