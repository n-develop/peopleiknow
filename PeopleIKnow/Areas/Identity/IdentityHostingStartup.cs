using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(PeopleIKnow.Areas.Identity.IdentityHostingStartup))]

namespace PeopleIKnow.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => { });
        }
    }
}