using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCDungcuhoctap.Startup))]
namespace MVCDungcuhoctap
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
