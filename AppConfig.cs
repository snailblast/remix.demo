namespace remix.demo
{
    public class AppConfig
    {
        public IConfiguration Configuration { get; }
        public string RemixApiDomain { get; set; }
        public string RemixApiKey { get; set; }
        public string RemixApiSecret { get; set; }

        public AppConfig(IConfiguration configuration)
        {
            Configuration = configuration;
            RemixApiDomain = Configuration["RemixApiDomain"];
            RemixApiKey = Configuration["RemixApiKey"];
            RemixApiSecret = Configuration["RemixApiSecret"];
        }
    }
}
