using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Market.Startup))]
namespace Market
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
          
            ConfigureAuth(app);
            app.MapHubs();
        }
    }
}
