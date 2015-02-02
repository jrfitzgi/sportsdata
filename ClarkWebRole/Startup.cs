using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClarkWebRole.Startup))]
namespace ClarkWebRole
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
