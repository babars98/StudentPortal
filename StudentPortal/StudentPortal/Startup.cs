using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudentPortal.Startup))]
namespace StudentPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
