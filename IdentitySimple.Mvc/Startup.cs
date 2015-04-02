using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IdentitySimple.Mvc.Startup))]
namespace IdentitySimple.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
