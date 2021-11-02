using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GCD0805App.Startup))]
namespace GCD0805App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
