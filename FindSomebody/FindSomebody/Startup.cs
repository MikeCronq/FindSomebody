using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FindSomebody.Startup))]

namespace FindSomebody
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}