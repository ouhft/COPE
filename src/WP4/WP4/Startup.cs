using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WP4.Startup))]
namespace WP4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
