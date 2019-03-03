using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VotingWeb.Startup))]
namespace VotingWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
