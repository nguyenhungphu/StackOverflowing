using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QAProject2.Startup))]
namespace QAProject2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
