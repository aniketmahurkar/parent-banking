using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bank_Application.Startup))]
namespace Bank_Application
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
