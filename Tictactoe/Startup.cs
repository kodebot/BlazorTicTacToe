using Cloudcrate.AspNetCore.Blazor.Browser.Storage;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tictactoe.Model;

namespace Tictactoe
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddStorage();
            services.AddSingleton<Settings>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
