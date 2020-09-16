using Sitecore.Data;

namespace Foundation.Caching {
    public struct Settings {
        public static ID TimeoutRenderingParam => new ID("{38A43E35-17D1-4D45-B399-9EA66C94B861}");
        public static string TimeoutRenderingKey => "timeout";
        public static string CachingSite => "cachingSite";
    }
}
