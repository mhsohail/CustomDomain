using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CustomDomain.Startup))]
namespace CustomDomain
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
