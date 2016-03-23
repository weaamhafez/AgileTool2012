using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EngineerWeb.Startup))]
namespace EngineerWeb
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
