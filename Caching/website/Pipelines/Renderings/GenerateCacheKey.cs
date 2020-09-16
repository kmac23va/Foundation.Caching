using Sitecore.Data.Items;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.Mvc.Presentation;

namespace Foundation.Caching.Pipelines.Renderings {
    public class GenerateCacheKey : Sitecore.Mvc.Pipelines.Response.RenderRendering.GenerateCacheKey {
        public GenerateCacheKey(RendererCache rendererCache) : base(rendererCache) {
        }

        protected override string GenerateKey(Rendering rendering, RenderRenderingArgs args) {
            string cacheKey = base.GenerateKey(rendering, args);
            Item renderingItem = rendering.RenderingItem.InnerItem;

            //If the timeout rendering parameter exists and has a value, add a token to the cache key that it's present
            if (!string.IsNullOrEmpty(renderingItem[Settings.TimeoutRenderingParam])) {
                cacheKey += $"_#{Settings.TimeoutRenderingKey}:1";
            }

            return cacheKey;
        }
    }
}
